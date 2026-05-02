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
using System.Data.Linq.SqlClient;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using System.Data.OleDb;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ioneNet.HumanResourceManagement
{
    public partial class HRMDashboard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public static string var;
        public static int Div_ID; 
        public HRMDashboard()
        {
            InitializeComponent();
        }

        private void HRMDashboard_Load(object sender, EventArgs e)
        {
            var pscrap = (from m in db.Costing_Units where m.Company == logIn.company select new { m.id, m.BU_Name }).Distinct().ToList();
            if (pscrap.Count > 0)
            {
                sfComboBox1.DataSource = pscrap;
                sfComboBox1.ValueMember = "id";
                sfComboBox1.DisplayMember = "BU_Name";
                sfComboBox1.SelectedIndex = -1;
            }

            sfComboBox1.SelectedValue = logIn.BU_ID;
            BindDashBoard();


        }
        public void BindDashBoard()
        {
            try
            {
             
                DateTime dt = dateTimePicker1.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                int cr, a, p, d1, cl, c2;
                var cnt = (from s in db.employeeCount(dt, Convert.ToInt32(sfComboBox1.SelectedValue), logIn.company) select s).ToList();
                //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
                if (cnt.Count > 0)
                {

                    btnTotalEmployees.Text = "Total Employees " + "\n" + (cnt[0].Total_Employees);
                    btnMonthJoinees.Text = "Joined Current Month " + "\n" + (cnt[0].Emp_Joined_Month);
                    btnYearJoinees.Text = "Joined Current Year " + "\n" + (cnt[0].Emp_Joined_Year);
                    btnMonthResigns.Text = "Resigned Current Month " + "\n" + (cnt[0].Emp_Resigned_Month);
                    btnYearResigns.Text = "Resigned Current Year " + "\n" + (cnt[0].Emp_Resigned_Year);
                    btnAbove58.Text = "Employee >58 Yrs " + "\n" + (cnt[0].AgeAbove58);
                    sfButton1.Text = "Service >6 Months " + "\n" + (cnt[0].Six_MonthsService);

                    btnJoinApproval.Text = "Joinings To Approve " + "\n" + (cnt[0].Joining_ToApprove);
                    btnResignAccept.Text = "Resignations To Accept " + "\n" + (cnt[0].Resign_To_Accept);
                    btnLeaveApproval.Text = "Leave Applications To Approve " + "\n" + (cnt[0].LeavesToApprove);

                }


                if (logIn.company == 4 || logIn.company == 1030)
                {
                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_Attendance_Count", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@edate", dt1);
                    // cmd2.Parameters.AddWithValue("@todate", dateTimePicker1.Value);


                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    if (ds2.Rows.Count >= 0)
                        btnEmpDayPresent.Text = "Employees Present " + "\n" + ds2.Rows[0]["Employees_Presnt"].ToString();
                    btnEmpDayAbsent.Text = "Employees Absent " + "\n" + ds2.Rows[0]["Employees_Absent"].ToString();
                    btnEmpDayLeaves.Text = "Employees on Leave " + "\n" + ds2.Rows[0]["Emp_on_Leave"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton7_Click(object sender, EventArgs e)
        {
            BindDashBoard();
        }

        private void btnLeaveReport_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "LeaveAvailedReport.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                
                //DateTime dt = dateTimePicker1.Value;
                //string dt1 = dt.ToString("yyyy/MM/dd");
                int Year = dateTimePicker1.Value.Year;
                string dt = Year + "/01/01";
               rep = new HumanResourceManagement.Reports.LeaveAvailedReport();
                SqlCommand cmd = new SqlCommand("SP_HR_GetLeaveAvailedReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@frmDate", dt);
                cmd.Parameters.AddWithValue("@todate", dateTimePicker1.Value);


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);

                if (Dt.Rows.Count > 0)
                {


                    crConnectionInfo.ServerName = frmMain.ServerIP;
                    crConnectionInfo.DatabaseName = frmMain.Database;
                    crConnectionInfo.UserID = frmMain.DBUserID;
                    crConnectionInfo.Password = frmMain.Password;


                    crDatabase = rep.Database;
                    crTables = crDatabase.Tables;
                    //Loop through all tables in the report and apply the connection information for each table.
                    for (int k = 0; k < crTables.Count; k++)
                    {
                        //  crTable = crTables[i];
                        crTableLogOnInfo = crTables[k].LogOnInfo;
                        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                        crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                    }
                    rep.SetDataSource(Dt);


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    Process.Start(path);
                }
                else
                {
                    MessageBox.Show("No Data Found");
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnalaryRevisionReport_Click(object sender, EventArgs e)
        {

            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "SalaryRevisionReport.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();


                rep = new HumanResourceManagement.Reports.SalaryRevisionReprot();
                SqlCommand cmd = new SqlCommand("Sp_HR_Salary_IncrementReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@buid", Convert.ToInt32(sfComboBox1.SelectedValue));



                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);

                if (Dt.Rows.Count > 0)
                {


                    crConnectionInfo.ServerName = frmMain.ServerIP;
                    crConnectionInfo.DatabaseName = frmMain.Database;
                    crConnectionInfo.UserID = frmMain.DBUserID;
                    crConnectionInfo.Password = frmMain.Password;


                    crDatabase = rep.Database;
                    crTables = crDatabase.Tables;
                    //Loop through all tables in the report and apply the connection information for each table.
                    for (int k = 0; k < crTables.Count; k++)
                    {
                        //  crTable = crTables[i];
                        crTableLogOnInfo = crTables[k].LogOnInfo;
                        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                        crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                    }
                    rep.SetDataSource(Dt);


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    Process.Start(path);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLOPReport_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "LOPReport.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();

                rep = new HumanResourceManagement.Reports.LOPReport();




                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }

                int Year = dateTimePicker1.Value.Year;
                string dt = Year + "/01/01";
                rep.SetParameterValue("buid", logIn.BU_ID);
                rep.SetParameterValue("startDate", dt);
                rep.SetParameterValue("EndDate", dateTimePicker1.Text);
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

        private void btnTotalEmployees_Click(object sender, EventArgs e)
        {
            Div_ID = Convert.ToInt32(sfComboBox1.SelectedValue);
            ioneNet.HumanResourceManagement.Reports.frmEmployee_Master_Data frm = new ioneNet.HumanResourceManagement.Reports.frmEmployee_Master_Data();
            frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void btnMonthJoinees_Click(object sender, EventArgs e)
        {
            var = "NewJoiness_Month";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();
           
            frm.ShowDialog();
        }

        private void btnYearJoinees_Click(object sender, EventArgs e)
        {
            var = "NewJoiness_Year";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnMonthResigns_Click(object sender, EventArgs e)
        {
            var = "Resigned_Month";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnYearResigns_Click(object sender, EventArgs e)
        {
            var = "Resigned_Year";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnAbove58_Click(object sender, EventArgs e)
        {
            var = "Above58";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnJoinApproval_Click(object sender, EventArgs e)
        {
            var = "Joinings_To_Approve";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnResignAccept_Click(object sender, EventArgs e)
        {
            var = "Resigns_To_Accept";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnEmpDayPresent_Click(object sender, EventArgs e)
        {
            var = "Present";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnEmpDayLeaves_Click(object sender, EventArgs e)
        {
            var = "On_Leave";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void btnEmpDayAbsent_Click(object sender, EventArgs e)
        {
            var = "Absent";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            var = "6MonthsService";
            ioneNet.HumanResourceManagement.EmpDialog frm = new ioneNet.HumanResourceManagement.EmpDialog();

            frm.ShowDialog();
        }
    }
}
