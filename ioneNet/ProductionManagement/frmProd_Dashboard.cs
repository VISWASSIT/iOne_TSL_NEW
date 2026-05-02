using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;

namespace ioneNet.ProductionManagement
{
    public partial class frmProd_Dashboard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        public frmProd_Dashboard()
        {
            InitializeComponent();
        }

        private void frmProd_Dashboard_Load(object sender, EventArgs e)
        {
            cmbYear.Text = "Current Month";
            bindDashBoard();
        }

        private void sfButton11_Click(object sender, EventArgs e)
        {
            bindDashBoard();
        }
        public void bindDashBoard()
        {

            int year, month, days, startmonth, startyear, days2;
            string endDate = "";
            string startdate = "";
            year = Convert.ToInt32(DateTime.Now.Year.ToString());
            month = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (cmbYear.Text == "Current Month")
            {
                days = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + month + "-" + 1;
                endDate = year + "-" + month + "-" + days;

            }
            if (cmbYear.Text == "Last Month")
            {

                startmonth = month - 1;
                days = DateTime.DaysInMonth(year, startmonth);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + startmonth + "-" + days;

            }
            if (cmbYear.Text == "Last 3 Months")
            {

                startmonth = month - 3;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }
            if (cmbYear.Text == "Last 6 Months")
            {

                startmonth = month - 6;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }
            if (cmbYear.Text == "Last 12 Months")
            {

                startmonth = month - 12;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }

            DateTime SDate = Convert.ToDateTime(startdate);

            DateTime EDate = Convert.ToDateTime(endDate);
            //Bind Sale Data Graph

            DateTime dt = logIn.fy_Start_Date;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dtt = logIn.fy_End_Date;
            string dt2 = dtt.ToString("yyyy/MM/dd");


            ////Bind Target Vs Actual Bar Chart
            //SqlCommand cmd = new SqlCommand("SP_Bind_Sale_TargetVsActual", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@FromDate", SDate);
            //cmd.Parameters.AddWithValue("@ToDate", EDate);
            //cmd.Parameters.AddWithValue("@compname", logIn.company);
            //cmd.Parameters.AddWithValue("@SaleExe", 12);
            //cmd.Parameters.AddWithValue("@para", 2);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable ds = new DataTable();
            //da.Fill(ds);

            //chart1.DataSource = ds;
            //chart1.Series["Tot_Target"].XValueMember = "Salesmen_Code";
            //chart1.Series["Tot_Target"].YValueMembers = "Tot_Target";
            //chart1.Series["Qty_Achieved"].YValueMembers = "Qty_Achieved";
            //chart1.DataBind();


            ////Bind Order Booking Trend (Line Chart)
            SqlCommand cmdOB = new SqlCommand("SP_Bind_Daywise_Production_Graph", con);
            cmdOB.CommandType = CommandType.StoredProcedure;
            cmdOB.Parameters.AddWithValue("@FromDate", SDate);
            cmdOB.Parameters.AddWithValue("@ToDate", EDate);
            cmdOB.Parameters.AddWithValue("@compname", logIn.company);
            cmdOB.Parameters.AddWithValue("@buid", logIn.BU_ID);

            SqlDataAdapter daOB = new SqlDataAdapter(cmdOB);
            DataTable dsOB = new DataTable();
            daOB.Fill(dsOB);

            chart6.DataSource = dsOB;
            chart6.Series["Order_Book_Qty"].XValueMember = "eDate";
            chart6.Series["Order_Book_Qty"].YValueMembers = "Production_Qty";

            chart6.DataBind();



            ////Bind Sales Funnel Chart Data
            //SqlCommand cmd5 = new SqlCommand("SP_Bind_Sales_Data_Graph", con);
            //cmd5.CommandType = CommandType.StoredProcedure;
            //cmd5.Parameters.AddWithValue("@FromDate", SDate);
            //cmd5.Parameters.AddWithValue("@ToDate", EDate);
            //cmd5.Parameters.AddWithValue("@compname", logIn.company);
            //cmd5.Parameters.AddWithValue("@para", 1);
            //cmd5.Parameters.AddWithValue("@saleExe", 12);

            //SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
            //DataTable ds5 = new DataTable();
            //da5.Fill(ds5);

            //chart3.DataSource = ds5;
            ////chart3.Series["Series1"].XValueMember = "Parameter";
            //chart3.Series["Series1"].YValueMembers = "Qty";
            //chart3.DataBind();



            ////Bind Product Wise Orders (PIE CHART)
            SqlCommand cmd6 = new SqlCommand("SP_Bind_Production_Ratio_Graph", con);
            cmd6.CommandType = CommandType.StoredProcedure;
            cmd6.Parameters.AddWithValue("@FromDate", SDate);
            cmd6.Parameters.AddWithValue("@ToDate", EDate);
            cmd6.Parameters.AddWithValue("@compname", logIn.company);
            cmd6.Parameters.AddWithValue("@buid", logIn.BU_ID);


            SqlDataAdapter da6 = new SqlDataAdapter(cmd6);
            DataTable ds6 = new DataTable();
            da6.Fill(ds6);

            chart4.DataSource = ds6;
            chart4.Series["Series1"].XValueMember = "PType";
            chart4.Series["Series1"].YValueMembers = "Prod_Qty";
            chart4.DataBind();





            //Order Count
            int cr, a, p, d, cl, v;
            var cnt = (from s in db.Production_Data_Dashboard(logIn.company, SDate, EDate, logIn.BU_ID) select s).ToList();
            //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            if (cnt.Count > 0)
            {
                linkLabel1.Text = cnt[0].Finished_Qty.ToString();
                linkLabel2.Text = cnt[0].Yield_Per.ToString();
                linkLabel3.Text = cnt[0].EC_Per.ToString();
                linkLabel4.Text = cnt[0].MR_Per.ToString();
                linkLabel5.Text = cnt[0].Coal_Per_MT.ToString();
                linkLabel7.Text = cnt[0].Power_Per_MT.ToString();
                linkLabel6.Text = cnt[0].Local_Qty.ToString();


                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            }

            //Quote Count

            


                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            


            //Order Count
            double pval;
           

            //Sale Value
            double sval;



            //SqlCommand cmd1 = new SqlCommand("Top5Customers", con);
            //cmd1.CommandType = CommandType.StoredProcedure;
            //cmd1.Parameters.AddWithValue("@fY_SDate", SDate);
            //cmd1.Parameters.AddWithValue("@fY_EDate", EDate);
            //cmd1.Parameters.AddWithValue("@compname", logIn.company);
            //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            //DataTable ds1 = new DataTable();
            //da1.Fill(ds1);

            //chart2.DataSource = ds1;
            //chart2.Series["Sale_Value"].XValueMember = "Customer";
            //chart2.Series["Sale_Value"].YValueMembers = "SaleValue";
            //chart2.DataBind();

        }
    }
}
