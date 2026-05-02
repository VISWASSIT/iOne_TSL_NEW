using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using System.Data.OleDb;

namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmAttendanceReport_BioMetric : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        //SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Essl"].ConnectionString);
        public frmAttendanceReport_BioMetric()
        {
            InitializeComponent();
        }

        private void frmAttendanceReport_Load(object sender, EventArgs e)
        {

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.OleDb.OleDbConnection MyConnection;
                //System.Data.DataTable DtSet;
                //System.Data.OleDb.OleDbDataAdapter MyCommand;
                
                
                var user = (from m in db.Company_Infos
                            where m.Company == logIn.company
                            select new
                            {
                                m.Bank_Branch
                                
                            }).ToList();               
                string Filepath = user[0].Bank_Branch;
                //Import Data from Biometric 
                MyConnection = new OleDbConnection("Provider= Microsoft.Jet.OLEDB.4.0;Data Source=" + Filepath + "");
               
                MyConnection.Open();
                //OleDbCommand cmd1 = MyConnection.CreateCommand();
                //OleDbDataReader dr1;
                //cmd1.CommandText = "select * from Employees";
                //cmd1.ExecuteNonQuery();
                //dr1 = cmd1.ExecuteReader();

                //while (dr1.Read())
                //{
                //    string BID = (dr1.GetInt32(4)).ToString();
                //    string Emp_ID = (dr1.GetInt32(0)).ToString();

                //    if ((from u in db.HR_Employee_Master_Datas where u.BiometricID ==BID && u.Company_ID == logIn.company select u).Count() > 0)
                //    {


                //        var p1 = db.HR_Employee_Master_Datas.Where(w => w.BiometricID == BID && w.Company_ID == logIn.company).FirstOrDefault();

                //        p1.Emp_ID = Convert.ToInt32((dr1.GetInt32(0)).ToString());
                //        db.SubmitChanges();
                //    }
                //}

                //Import Attendance
                OleDbCommand cmd3 = MyConnection.CreateCommand();
                OleDbDataReader dr2;
                DateTime fDate = dateTimePicker1.Value;
                DateTime tDate = dateTimePicker1.Value;
                string dt2 = fDate.ToString("yyyy/MM/dd");
                string dt3 = tDate.ToString("yyyy/MM/dd");
                cmd3.CommandText = "select * from AttendanceLogs where (AttendanceDate  >= @fromdate and AttendanceDate <=@todate) ";
                cmd3.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(dt2));
                cmd3.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt3));
                cmd3.ExecuteNonQuery();
                dr2 = cmd3.ExecuteReader();
                SqlCommand cmd4 = new SqlCommand("delete  from [BIoMetric_AttendanceLogs] where (AttendanceDate  >= @fromdate and AttendanceDate <=@todate)", con);
                cmd4.Parameters.AddWithValue("@fromdate", Convert.ToDateTime(dt2));
                cmd4.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt3));

                if (con.State != ConnectionState.Open)
                    con.Open();
                //con.Open();
                cmd4.ExecuteNonQuery();
                while (dr2.Read())
                {
                    BIoMetric_AttendanceLog p = new BIoMetric_AttendanceLog();
                    p.AttendanceLogId = dr2.GetInt32(0);
                    p.AttendanceDate = (dr2.GetDateTime(1));
                    p.EmployeeId = dr2.GetInt32(2);
                    //string intime = (dr2.GetInt32(3)).ToString();
                    p.InTime = (dr2.GetString(3)).ToString(); 
                    p.InDeviceId = dr2.GetString(4).ToString();
                    p.OutTime = (dr2.GetString(5)).ToString();
                    p.OutDeviceId = dr2.GetString(6).ToString();
                    //p.Duration = cmbNationality.Text;                   
                    p.C1 = logIn.company;

                    db.BIoMetric_AttendanceLogs.InsertOnSubmit(p);
                    db.SubmitChanges();
                }
                    MyConnection.Close();


                //Provider = Microsoft.Jet.OLEDB.4.0; Data Source = D:\VehicleManager\VehicleManager.mdb"
                //providerName = "System.Data.OleDb"


                if (checkBox1.Checked == false)
                {
                    DateTime dt = dateTimePicker1.Value;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_AttendanceReport", con);
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
                    sfDataGrid1.DataSource = ds2;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Emp_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Emp_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Emp_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Emp_Code"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Emp_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Emp_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Emp_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Emp_Name"].FilterRowCondition = FilterRowCondition.Contains;
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_Monthly_AttendanceReport", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@edate", dateTimePicker1.Value);
                     cmd2.Parameters.AddWithValue("@tdate", dateTimePicker2.Value);


                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Emp_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Emp_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Emp_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Emp_Code"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Emp_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Emp_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Emp_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Emp_Name"].FilterRowCondition = FilterRowCondition.Contains;
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

        private void btnExcell_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.BU_Name;
            workBook.Worksheets[0].Range["A2"].Value = "Day Attendance Report";
            workBook.Worksheets[0].Range["D2"].Value = "Date :" + dateTimePicker1.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Day_Attendance_Report.xlsx");
            string doc = Fname + "\\Day_Attendance_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Rows.Clear();
            dt.Columns.Clear();
            dt.Columns.Add("ADate", typeof(string));
            dt.Columns.Add("aTime", typeof(string));
            string d1 = "2025-02-14";
            string AttLog = "13:42:in(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),13:42:(CRG),07:00:out(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),07:00:(CRG),08:53:in(CRG),08:53:(CRG),08:53:(CRG),08:53:(CRG),08:53:(CRG),08:53:(CRG),08:53:(CRG),08:54:(CRG),";
            string[] subStrings = AttLog.Split(',');

            foreach (string str in subStrings)
            {
                if (str.Length > 4)
                {
                    System.Data.DataRow dr;
                    dr = dt.NewRow();
                    dr["ADate"] = "2025-04-01";
                    dr["aTime"] = Mid(str, 1, 5);
                    dt.Rows.Add(dr);
                    Console.WriteLine(str);
                }
            }
        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
    }
}
