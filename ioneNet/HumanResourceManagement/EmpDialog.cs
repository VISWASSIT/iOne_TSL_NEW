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
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
using System.IO;
using Syncfusion.WinForms.DataGrid.Interactivity;
namespace ioneNet.HumanResourceManagement
{
    public partial class EmpDialog : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public EmpDialog()
        {
            InitializeComponent();
        }

        private void EmpDialog_Load(object sender, EventArgs e)
        {
            try
            {

                label12.Text = HRMDashboard.var;

                if (HRMDashboard.var == "Present" || HRMDashboard.var == "Absent")
                {
                    DateTime dt = DateTime.Now;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_Attendance_Present ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@edate", dt1);
                    cmd2.Parameters.AddWithValue("@attnstatus", HRMDashboard.var);



                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                    if (HRMDashboard.var == "Absent")
                    {
                        sfButton3.Text = "Approve Attn";
                        sfButton3.Visible = true;
                    }
                }
                else


                if (HRMDashboard.var == "Joinings_To_Approve" || HRMDashboard.var == "Resigns_To_Accept")
                {
                    sfButton3.Visible = true;
                    if (HRMDashboard.var == "Joinings_To_Approve")
                    {
                        var d = (from data in db.ShowEmployees_ForApproval(logIn.company,logIn.BU_ID) select data).ToList();

                        if (d.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            sfDataGrid1.DataSource = d;
                        }
                    }                    
                    if (HRMDashboard.var == "Resigns_To_Accept")
                    {
                        var d = (from data in db.ShowEmployees_ForResign(logIn.company, logIn.BU_ID) select data).ToList();

                        if (d.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            sfDataGrid1.DataSource = d;
                        }
                    }

                    if (HRMDashboard.var == "6MonthsService")
                    {
                        DateTime dt = DateTime.Now;
                        string dt1 = dt.ToString("yyyy/MM/dd");

                        SqlCommand cmd2 = new SqlCommand("ShowEmployees_SixMonthService ", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@compname", logIn.company);
                        cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        cmd2.Parameters.AddWithValue("@edate", dt1);
                        



                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        //DataSet ds2 = new DataSet();
                        DataTable ds2 = new DataTable();
                        // da2.Fill(ds2, "x");
                        da2.Fill(ds2);
                        sfDataGrid1.DataSource = ds2;
                    }
                }
                else
                { 
                    SqlCommand cmd2 = new SqlCommand("employeesonDashBoard ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@Division", logIn.company);
                    cmd2.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@ReportReq", HRMDashboard.var);



                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
          
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = label12.Text;
            workBook.Worksheets[0].Range["D2"].Value = "Date :" + DateTime.Now;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\" + label12.Text+ ".xlsx");
            string doc = Fname + "\\" + label12.Text + ".xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            try
            {

                if (HRMDashboard.var == "Joinings_To_Approve")
                {
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {

                        //foreach (var item in sfDataGrid1.SelectedItems)
                        //{
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Emp_Id"].MappingName;
                        var mappingName2 = sfDataGrid1.Columns["Approve"].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        if (cellVaue3 == "True")
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            //var cellVaue1 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());

                            //string Status = cellVaue1.ToString();
                            string OrdNo = cellVaue.ToString();

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Employee Master" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Approve_Role == true)
                                {
                                    var ci = db.HR_Employee_Master_Datas.Where(w => w.id == Convert.ToInt32(OrdNo) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {
                                        ci.Join_Approved = true;
                                        ci.Join_Approved_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The Employee Joinings");
                                    return;
                                }
                            }


                        }
                    }
                }


                if (HRMDashboard.var == "Resigns_To_Accept")
                {
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {

                        //foreach (var item in sfDataGrid1.SelectedItems)
                        //{
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Emp_Id"].MappingName;
                        var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        if (cellVaue3 == "True")
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            //var cellVaue1 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());

                            //string Status = cellVaue1.ToString();
                            string OrdNo = cellVaue.ToString();

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Employee Master" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Approve_Role == true)
                                {
                                    var ci = db.HR_Employee_Master_Datas.Where(w => w.id == Convert.ToInt32(OrdNo) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {
                                        ci.Resign_Accepted = true;
                                        ci.Resign_Accepted_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Accept The Resignations");
                                    return;
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
    }
}
