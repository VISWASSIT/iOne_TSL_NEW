using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using Ione_DAL;
namespace ioneNet.FinanceManagement.Reports
{
    public partial class frmGSTR3B : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmGSTR3B()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t1 = dateTimePicker1.Value;
                DateTime t2 = dateTimePicker2.Value;
                string dt1 = t1.ToString("dd/MMM/yyyy");
                string dt2 = t2.ToString("dd/MMM/yyyy");

                // string t1 = t.ToString("dd/MMM/yyyy");
                SqlCommand com = new SqlCommand("GSTRSaleData", con);
                com.Parameters.AddWithValue("@compname", logIn.company);
                com.Parameters.AddWithValue("@fromdate", dt1);
                com.Parameters.AddWithValue("@todate", dt2);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dtr = new DataTable();
                da.Fill(dtr);
                //string excelFilename = "GSTR-3B"
                //            string filelocation = AppDomain.CurrentDomain.BaseDirectory + "\GSTR-3B.xlsx";
               // string filelocation = Path.Combine(Directory.GetCurrentDirectory(), "GST-3B.xlsx");
                string filelocation = Path.Combine(Directory.GetCurrentDirectory(), "GST-3B.xlsx");
                var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
               // string filelocation = Path.Combine(projectFolder, @"FinanceManagement\Reports\GST-3B.xlsx");
                // string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\MyOtherFile.xml"; 
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWb = default(Excel.Workbook);
                Excel.Worksheet xlsht = default(Excel.Worksheet);
                //Excel.Worksheet xlsht1 = default(Excel.Worksheet);
                //Excel.Worksheet xlsht2 = default(Excel.Worksheet);
                //xlWorkSheetToExport = (Excel.Worksheet)xlApp.Sheets["SALES"];
                // Excel.Worksheet xlsht = new Excel.Worksheet();
                xlWb = xlApp.Application.Workbooks.Open(filelocation);
                xlsht = xlWb.Worksheets["FORM-3B"];
                xlsht.Cells[5, 17] = t1.ToString("MMMM");
                xlsht.Cells[6, 17] = t1.ToString("Y");
                var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No, a.State_Code, a.Alias_Name }).ToList();
                if (d1.Count > 0)
                {
                    xlsht.Cells[8, 4] = d1[0].GST_No;
                    xlsht.Cells[8, 5] = d1[0].Alias_Name;
                }
                xlsht = xlWb.Worksheets["SALES"];
                int rNum = 2;
                foreach (DataRow dr1 in dtr.Rows)
                {
                    //    Excel.Range range7 = xlsht.Cells[i + 7, j + 1] as Excel.Range;

                    //  range7.NumberFormat = "@";

                    xlsht.Cells[rNum, 1] = dr1["InvDate"].ToString();
                    xlsht.Cells[rNum, 2] = dr1["Prod_HSN_Code"].ToString();
                    xlsht.Cells[rNum, 3] = dr1["Inv_No"].ToString();
                    xlsht.Cells[rNum, 4] = dr1["Supplier_Name"].ToString();
                    xlsht.Cells[rNum, 6] = dr1["Taxable_Value"].ToString();
                    xlsht.Cells[rNum, 7] = dr1["CGST_Amnt"].ToString();
                    xlsht.Cells[rNum, 8] = dr1["SGST_Amnt"].ToString();
                    xlsht.Cells[rNum, 9] = dr1["IGST_Amnt"].ToString();
                    rNum = rNum + 1;

                }

                //Purchase Data - IGST
                SqlCommand com1 = new SqlCommand("GSTRPurchaseData", con);
                com1.Parameters.AddWithValue("@compname", logIn.company);
                com1.Parameters.AddWithValue("@fromdate", dt1);
                com1.Parameters.AddWithValue("@todate", dt2);
                com1.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da1 = new SqlDataAdapter(com1);
                DataTable dtr1 = new DataTable();
                da1.Fill(dtr1);
                xlsht = xlWb.Worksheets["PURCHASE_IGST"];
                //xlsht1 = xlApp.Application.Workbooks.Open(filelocation).Worksheets["PURCHASE_IGST"];
                rNum = 13;
                foreach (DataRow dr1 in dtr1.Rows)
                {
                    //    Excel.Range range7 = xlsht.Cells[i + 7, j + 1] as Excel.Range;

                    //  range7.NumberFormat = "@";
                    if (dr1["Place_of_Supply"].ToString() == "Inter")
                    {
                        xlsht.Cells[rNum, 3] = dr1["Supplier_Name"].ToString();
                        xlsht.Cells[rNum, 4] = dr1["GSTIN_NO"].ToString();
                        xlsht.Cells[rNum, 5] = dr1["Prod_Name"].ToString();
                        xlsht.Cells[rNum, 7] = dr1["Grn_Date"].ToString();
                        xlsht.Cells[rNum, 8] = dr1["AcceptedQty"].ToString();
                        xlsht.Cells[rNum, 6] = dr1["Supplier_InvNo"].ToString();
                        xlsht.Cells[rNum, 9] = dr1["Uom"].ToString();
                        xlsht.Cells[rNum, 10] = dr1["Price"].ToString();
                        xlsht.Cells[rNum, 11] = dr1["Disc_Amount"].ToString();
                        xlsht.Cells[rNum, 13] = dr1["Taxable_Value"].ToString();
                        xlsht.Cells[rNum, 14] = dr1["Gst_Rate"].ToString();
                        xlsht.Cells[rNum, 15] = dr1["IGST_Amnt"].ToString();
                        //  xlsht.Cells[rNum, 13] = dr1["IGST_Amnt"].ToString();
                        rNum = rNum + 1;
                    }

                }

                //Purchase Data - LOCAL

                xlsht = xlWb.Worksheets["PURCHASE_LOCAL"];
                //    xlsht2 = xlApp.Application.Workbooks.Open(filelocation).Worksheets["PURCHASE_LOCAL"];
                rNum = 13;
                foreach (DataRow dr1 in dtr1.Rows)
                {
                    //    Excel.Range range7 = xlsht.Cells[i + 7, j + 1] as Excel.Range;

                    //  range7.NumberFormat = "@";
                    if (dr1["Place_of_Supply"].ToString() == "Intra")
                    {
                        xlsht.Cells[rNum, 3] = dr1["Supplier_Name"].ToString();
                        xlsht.Cells[rNum, 4] = dr1["GSTIN_NO"].ToString();
                        xlsht.Cells[rNum, 5] = dr1["Prod_Name"].ToString();
                        xlsht.Cells[rNum, 7] = dr1["Grn_Date"].ToString();
                        xlsht.Cells[rNum, 8] = dr1["AcceptedQty"].ToString();
                        xlsht.Cells[rNum, 6] = dr1["Supplier_InvNo"].ToString();
                        xlsht.Cells[rNum, 9] = dr1["Uom"].ToString();
                        xlsht.Cells[rNum, 10] = dr1["Price"].ToString();
                        xlsht.Cells[rNum, 11] = dr1["Disc_Amount"].ToString();
                        xlsht.Cells[rNum, 13] = dr1["Taxable_Value"].ToString();
                        xlsht.Cells[rNum, 14] = dr1["Gst_Rate"].ToString();
                        xlsht.Cells[rNum, 15] = dr1["CGST_Amnt"].ToString();
                        xlsht.Cells[rNum, 16] = dr1["SGST_Amnt"].ToString();
                        //  xlsht.Cells[rNum, 13] = dr1["IGST_Amnt"].ToString();
                        rNum = rNum + 1;
                    }

                }
                xlApp.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
