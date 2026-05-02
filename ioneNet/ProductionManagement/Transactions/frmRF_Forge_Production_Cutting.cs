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
using Syncfusion.WinForms.DataGrid.Enums;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmRF_Forge_Production_Cutting : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmRF_Forge_Production_Cutting()
        {
            InitializeComponent();
        }

        private void frmForge_Production_Cutting_Load(object sender, EventArgs e)
        {


            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            AutoincrementId();
            var machine = (from k in db.Forging_MachineMasters select new { k.Machine_Name, k.ID }).ToList();
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
            var Oprn = (from k in db.Forging_Employees where k.Department == "Cutting" && k.Designation == "Operator" select new { k.Employee_Name}).Distinct().ToList();
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

            var Sup = (from k in db.Forging_Employees where k.Department == "Cutting" && k.Designation == "Supervisor" select new { k.Employee_Name }).Distinct().ToList();
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

                var result = db.Sp_autoincrement_Forging_CuttingReport(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtvchno.Text = result.FirstOrDefault().Report_No;
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

        private void cmbSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgJobCardData_Enter(object sender, EventArgs e)
        {

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

                if (tb3 != null && columnName == "Blade Condition")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Machine_Name")
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
                    var Prodname = (from d in db.Forge_Get_JobCards_Cutting(logIn.company)                                

                                    select new { d.Job_CardNo}).ToList();
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
                            if (columnName == "Blade Condition")
                            {
                                //  var Prodname = (from d in db.Forging_JobCardRMs where d.Job_CardNo == R1.Cells["Job_Card"].Value.ToString() select new { d.Heat_Code }).ToList();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Blade_Cond");
                                dt.Rows.Add("Slow Cutting");
                                dt.Rows.Add("Taper Cutting");
                                dt.Rows.Add("Teeth Breakage");
                                dt.Rows.Add("Teeth Wornout");
                                dt.Rows.Add("Blade Brakage");

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    coll.Add(dt.Rows[i][0].ToString());
                                }
                            }
                            else
                            {
                                if (columnName == "Machine_Name")
                                {
                                    var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Cutting" select new { d.Machine_Name }).ToList();
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("Heat_Code");
                                    foreach (var item in Prodname)
                                    {
                                        dt.Rows.Add(item.Machine_Name);
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

        private void dgJobCardData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgJobCardData.Rows[dgJobCardData.CurrentRow.Index];
                int columnIndex = dgJobCardData.CurrentCell.ColumnIndex;
                string columnName = dgJobCardData.Columns[columnIndex].Name;
                if (columnName == "Job_Card" && R1.Cells["Job_Card"].Value != null)
                {

                    decimal x = 0, y=0;
                    
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        string jcno = R1.Cells["Job_Card"].Value.ToString();
                        string jcnoE = dgJobCardData.Rows[i].Cells["Job_Card"].Value.ToString();
                        if (jcnoE == jcno)
                        {
                            x += ( dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == "" || dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == null || dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                        }
                    }


                    var getDetails = (from s in db.Forge_Get_JobCards_Process(logIn.company, R1.Cells["Job_Card"].Value.ToString(), "Cutting")
                                      select new { s.j_date, s.FG_Item_Code , s.FG_Item_Name, s.Job_CardQty,s.Heat_Code,s.RM_Name,s.QtyPending ,s.Input_Length}).FirstOrDefault();

                    if (getDetails != null)
                    {
                        y = (getDetails.QtyPending.ToString() == "" || getDetails.QtyPending.ToString() == null  ? Convert.ToDecimal(0) : Convert.ToDecimal(getDetails.QtyPending.ToString()));
                      //  y = Convert.ToDecimal(getDetails.QtyPending.ToString());
                        R1.Cells["Job_card_date"].Value = getDetails.j_date.ToString();
                        R1.Cells["Item_Code"].Value = getDetails.FG_Item_Code.ToString();
                        R1.Cells["Item_Name"].Value = getDetails.FG_Item_Name.ToString();
                        R1.Cells["Heat_Code"].Value = getDetails.Heat_Code.ToString();
                        R1.Cells["Job_Card_Qty"].Value = getDetails.Job_CardQty.ToString();
                        R1.Cells["Pending_Qty"].Value = (y - x);
                        R1.Cells["RM_Sec"].Value = getDetails.RM_Name.ToString();

                        R1.Cells["Cutting_Length"].Value = getDetails.Input_Length.ToString();
                        //

                    }

                    else
                    {
                       MessageBox.Show("Invalid Job Card Entered");
                    }                    
                }

                if (columnName == "Qty_Produced" || columnName == "Qty_Rejected")
                {
                    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["Job_Card"].Value != null)

                    {
                        decimal Qty_Pending = ( R1.Cells["Pending_Qty"].Value == "" || R1.Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Pending_Qty"].Value);
                        
                        decimal Qty_Produced = ( R1.Cells["Qty_Produced"].Value == "" || R1.Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Produced"].Value);
                        decimal Qty_Rejected = ( R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);
                       
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
                        
                        //decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        //decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);

                        
                        
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cmbMachineID.Text == "")
                //{
                //    MessageBox.Show("Machine Name Cannot Be Blank");
                //    return;
                //}
                //else
                if (cmbShift.Text == "")
                {
                    MessageBox.Show("Shift Cannot Be Blank");
                    return;
                }
                else
                {
                    String myString = "";
                    myString = txtvchno.Text;
                    if ((from a in db.Forging_CuttingReport_Masters where a.Company_ID == logIn.company && a.Report_No == txtvchno.Text select a).Count() > 0)
                    {
                        myString = txtvchno.Text;
                        db.Sp_delete_Production_Cutting(logIn.company, txtvchno.Text);

                        Forging_CuttingReport_Master pb = new Forging_CuttingReport_Master();
                        pb.Report_No = txtvchno.Text;
                        pb.Report_Date = dpdate.Value;
                        pb.Shift = cmbShift.Text;
                        pb.Machine_Name = Convert.ToInt32(cmbMachineID.SelectedValue);

                        pb.Operator = (cmbOperator.Text == null) ? "" : cmbOperator.Text;
                        pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                        pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                        pb.Process = "Cutting";
                        pb.Company_ID = logIn.company;
                        pb.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.Forging_CuttingReport_Masters.InsertOnSubmit(pb);
                        db.SubmitChanges();

                        //}

                        //db.SubmitChanges();

                        //Save Child Data (Job Card)
                        for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                        {
                            Forging_Cutting_Child SC = new Forging_Cutting_Child();
                            var d1 = (from a in db.Forging_CuttingReport_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                            SC.Report_Master_ID = d1[0].id;
                            SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();

                            if (dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString() != "")
                            {
                                DateTime t = Convert.ToDateTime(dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString());
                                string t1 = t.ToString("dd/MM/yyyy");
                                SC.J_Date = t;// DateTime.ParseExact(eDate, "MM/dd/yyyy", null); ;
                            }
                            SC.FG_Item_Code = (dgJobCardData.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Code"].Value).ToString();
                            //SC.FG_Item_Name = (dgJobCardData.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Name"].Value).ToString();
                            SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                            SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                            SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                            SC.RM_Name = (dgJobCardData.Rows[i].Cells["RM_Sec"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["RM_Sec"].Value).ToString();
                            SC.Machine_Name = (dgJobCardData.Rows[i].Cells["Machine_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Machine_Name"].Value).ToString();
                            SC.RM_Cutting_Length = (dgJobCardData.Rows[i].Cells["Cutting_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Cutting_Length"].Value);
                            SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                            SC.Qty_Rejected = (dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value);
                            SC.Qty_Accepted = (dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value);
                            SC.Cutting_Blade_No = (dgJobCardData.Rows[i].Cells["Blade_No"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Blade_No"].Value).ToString();
                            SC.Blade_Cond = (dgJobCardData.Rows[i].Cells["Blade_Cond"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Blade_Cond"].Value).ToString();
                            SC.RM_End_Piece = (dgJobCardData.Rows[i].Cells["RM_End_Piece"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["RM_End_Piece"].Value);
                            SC.Disposal = (dgJobCardData.Rows[i].Cells["Disposal"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Disposal"].Value).ToString();
                            SC.Remarks = (dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Remarks"].Value).ToString();
                            
                            db.Forging_Cutting_Childs.InsertOnSubmit(SC);
                        }
                        db.SubmitChanges();
                        MessageBox.Show("Record Updated Sucessfully");
                        clear();
                        return;
                    }
                    else
                    {
                        AutoincrementId();
                        Forging_CuttingReport_Master pb = new Forging_CuttingReport_Master();
                        pb.Report_No = txtvchno.Text;
                        pb.Report_Date = dpdate.Value;
                        pb.Shift = cmbShift.Text;
                        pb.Machine_Name = Convert.ToInt32(cmbMachineID.SelectedValue);

                        pb.Operator = (cmbOperator.Text == null) ? "" : cmbOperator.Text;
                        pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                        pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                        pb.Process = "Cutting";
                        pb.Company_ID = logIn.company;
                        pb.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.Forging_CuttingReport_Masters.InsertOnSubmit(pb);
                        db.SubmitChanges();


                        for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                        {
                            Forging_Cutting_Child SC = new Forging_Cutting_Child();
                            var d1 = (from a in db.Forging_CuttingReport_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                            SC.Report_Master_ID = d1[0].id;
                            SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();

                            if (dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString() != "")
                            {
                                DateTime t = Convert.ToDateTime(dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString());
                                string t1 = t.ToString("dd/MM/yyyy");
                                SC.J_Date = t;// DateTime.ParseExact(eDate, "MM/dd/yyyy", null); ;
                            }
                            SC.FG_Item_Code = (dgJobCardData.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Code"].Value).ToString();
                           // SC.FG_Item_Name = (dgJobCardData.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Name"].Value).ToString();
                            SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                            SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                            SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                            SC.RM_Name = (dgJobCardData.Rows[i].Cells["RM_Sec"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["RM_Sec"].Value).ToString();
                            SC.Machine_Name = (dgJobCardData.Rows[i].Cells["Machine_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Machine_Name"].Value).ToString();

                            SC.RM_Cutting_Length = (dgJobCardData.Rows[i].Cells["Cutting_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Cutting_Length"].Value);
                            SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                            SC.Qty_Rejected = (dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value);
                            SC.Qty_Accepted = (dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value);
                            SC.Cutting_Blade_No = (dgJobCardData.Rows[i].Cells["Blade_No"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Blade_No"].Value).ToString();
                            SC.Blade_Cond = (dgJobCardData.Rows[i].Cells["Blade_Cond"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Blade_Cond"].Value).ToString();

                            SC.RM_End_Piece = (dgJobCardData.Rows[i].Cells["RM_End_Piece"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["RM_End_Piece"].Value);
                            SC.Disposal = (dgJobCardData.Rows[i].Cells["Disposal"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Disposal"].Value).ToString();
                            SC.Remarks = (dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Remarks"].Value).ToString();

                            db.Forging_Cutting_Childs.InsertOnSubmit(SC);
                        }
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Sucessfully");
                        clear();
                        return;
                    }
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
            cmbOperator.Text = "";
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionManagement.frmForging_Production_CuttingList obj = new frmForging_Production_CuttingList();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    int Report_ID=0;
                    txtvchno.Text = ProductionManagement.frmForging_Production_CuttingList.Voucherno;
                    var sa = (from sq in db.Forging_CuttingReport_Masters
                              where sq.Company_ID == logIn.company && sq.Report_No == txtvchno.Text
                              select new
                              {
                                  sq.id,
                                  sq.Report_Date,
                                  sq.Shift,
                                  sq.Machine_Name,
                                  sq.Operator,
                                  sq.Supervisor,
                                  sq.Remarks,
                                  sq.Created_By,                                  
                                  sq.Modified_By
                                  
                              }).ToList();
                    if (sa.Count > 0)
                    {
                        Report_ID = sa[0].id;
                        dpdate.Value = Convert.ToDateTime(sa[0].Report_Date);
                        cmbMachineID.SelectedValue = sa[0].Machine_Name;
                        cmbShift.Text = sa[0].Shift;
                        cmbOperator.Text = sa[0].Operator;
                        cmbSupervisor.Text = sa[0].Supervisor;
                        txtRemarks.Text = sa[0].Remarks.ToString();
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_By;


                    }
                    var ca = (from sq in db.Forging_Cutting_Childs

                              where  sq.Report_Master_ID == Report_ID
                              select new
                              {

                                  Job_Card= sq.Job_CardNo,
                                  Job_card_date=sq.J_Date,
                                  Item_Code=sq.FG_Item_Code,
                                  Item_Name="",
                                  Heat_Code = sq.Heat_Code,
                                  Job_Card_Qty = sq.Job_CardQty,
                                  Pending_Qty = sq.Balance_Qty,
                                  Qty_Produced = sq.Qty_Produced,
                                  Qty_Rejected = sq.Qty_Rejected,
                                  Qty_Accepted = sq.Qty_Accepted,
                                  RM_Sec =sq.RM_Name,
                                  Cutting_Length=sq.RM_Cutting_Length,
                                  Blade_No  = sq.Cutting_Blade_No,
                                  RM_End_Piece = sq.RM_End_Piece,
                                  sq.Disposal,
                                  sq.Remarks,
                                  sq.Blade_Cond,
                                  sq.Machine_Name

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                db.Sp_delete_Production_Cutting(logIn.company, txtvchno.Text);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnGetJobCards_Click(object sender, EventArgs e)
        {
            
        }

        private void dgJobCardData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                var Buyerblind = (from m in db.Forge_Get_JobCards_Process(logIn.company, "0", "Cutting")
                                  select m).ToList();
                if (Buyerblind.Count > 0)
                {
                    //cmb_fg_Item_Code.DataSource = Buyerblind;
                    //cmb_fg_Item_Code.ValueMember = "Cust_Item_Code";
                    //cmb_fg_Item_Code.DisplayMember = "Cust_Item_Code";
                    sfDataGrid1.DataSource = Buyerblind;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Job_CardNo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Job_CardNo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Job_CardNo"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["RM_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["RM_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["RM_Name"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["FG_Item_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["FG_Item_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["FG_Item_Code"].ImmediateUpdateColumnFilter = true;



                    groupBox2.Visible = true;

                }
                else
                {
                    MessageBox.Show("No Records Found");
                }
            }
        }

        private void btnCancelItemSelection_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void dgJobCardData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
