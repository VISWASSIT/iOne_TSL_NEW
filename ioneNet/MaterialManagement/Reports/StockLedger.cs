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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Ione_DAL;
using Syncfusion.Windows.Forms.Tools;

namespace ioneNet.MaterialManagement.Reports
{
    public partial class StockLedger : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        public StockLedger()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "")
                {
                    binddata();
                }
                else
                {
                    MessageBox.Show("Select Product Name");
                    comboBox1.Focus();
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void binddata()
        {
            try
            {
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("ShowStockLedger_new", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@prodcode", Convert.ToInt32(comboBox1.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);                
                dataGridView1.DataSource = ds2;
                double OPQty = 0, RecQty = 0, IssQty = 0;
                decimal rec, iss, op, opval,recval,issval, closing = 0, closingval = 0;
                //op = (dataGridView1.Rows[0].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value);
                //rec = (dataGridView1.Rows[0].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells["Debit"].Value);
                //iss = (dataGridView1.Rows[0].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells["Credit"].Value);
                //if (rec - iss > 0)
                //{
                //    closing = rec - iss;
                //    //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                //    dataGridView1.Rows[0].Cells["Balance"].Value = closing;
                //    dataGridView1.Rows[0].Cells["BalType"].Value = "Dr";
                //}
                //else
                //{
                //    closing = iss - rec;
                //    //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                //    dataGridView1.Rows[0].Cells["Balance"].Value = closing;
                //    dataGridView1.Rows[0].Cells["BalType"].Value = "Cr";
                //}
                //rec = 0;
                //iss = 0;
                //closing = 0;
                //this.sfDataGrid1.Columns.Add(new GridUnBoundColumn() { HeaderText = "Balance", MappingName = "Balance", Expression = "" });
                //this.sfDataGrid1.Columns.Add(new GridUnBoundColumn() { HeaderText = "Bal Type", MappingName = "Bal_Tye", Expression = "" });

                //Get The Balance

                //double OPQty = 0, RecQty = 0, IssQty = 0;
                //decimal rec, iss, op, closing = 0;
                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    
                    //if (dataGridView1.Rows[i - 1].Cells["BalType"].Value == "Cr")
                    //{
                        op = ((dataGridView1.Rows[i - 1].Cells["ClosingQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["ClosingQty"].Value));
                    opval = ((dataGridView1.Rows[i - 1].Cells["ClosingValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["ClosingValue"].Value));


                    //}
                    //else
                    //{
                    //    op = (dataGridView1.Rows[i - 1].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value);
                    //}
                    rec = (dataGridView1.Rows[i].Cells["ReceiptQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value);
                    recval = (dataGridView1.Rows[i].Cells["ReceiptValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptValue"].Value);
                    iss = (dataGridView1.Rows[i].Cells["IssuedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                    issval = (dataGridView1.Rows[i].Cells["IssueValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssueValue"].Value);
                    //op = Convert.ToDecimal(Balance);
                    //rec = Convert.ToDecimal(Debit);
                    //iss = Convert.ToDecimal(Credit);

                    //if (op + rec - iss > 0)
                    //{
                    closing = op + rec - iss;
                    closingval = opval + recval - issval;

                    //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                    //sfDataGrid1
                    dataGridView1.Rows[i].Cells["ClosingQty"].Value = closing;
                    dataGridView1.Rows[i].Cells["ClosingValue"].Value = closingval;
                    //dataGridView1.Rows[i].Cells["BalType"].Value = "Dr";
                    //}
                    //else
                    //{
                    //    //closing = op + rec - iss;
                    //    ////closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                    //    //dataGridView1.Rows[i].Cells["Balance"].Value = Math.Abs(closing);
                    //    //dataGridView1.Rows[i].Cells["BalType"].Value = "Cr";
                    //}
                    //RecQty += (dataGridView1.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Debit"].Value);
                    //IssQty += (dataGridView1.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Credit"].Value);
                    //ClsQty += (dgStockdata.Rows[i].Cells["ClosingQty"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgStockdata.Rows[i].Cells["ClosingQty"].Value);
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            //var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            //if (colorDict.ContainsKey(rowColumnIndex))
            //    e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            //if (!colorDict.ContainsKey(rowColumnIndex))
            //    colorDict.Add(rowColumnIndex, color);
            //else
            //    colorDict[rowColumnIndex] = color;
            //sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void StockLedger_Load(object sender, EventArgs e)
        {
            //dtpFrmDate.MinDate = logIn.fy_Start_Date;
            dtpToDate.MaxDate = logIn.fy_End_Date;
            var d = (from po in db.Products
                    
                     where po.Company_ID == logIn.company && po.Prod_Status_ID == 1
                     select new { po.prod_ID, po.Prod_Name }).Distinct().ToList();
            if (d.Count > 0)
            {
                comboBox1.DataSource = d;
                comboBox1.ValueMember = "prod_ID";
                comboBox1.DisplayMember = "Prod_Name";
                comboBox1.SelectedIndex = -1;
            }

            if(MaterialManagement.Reports.StockStatement.SO_No != null)
            {
                comboBox1.Text = MaterialManagement.Reports.StockStatement.SO_No;
                var sa = (from a in db.Products
                          join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                          where a.Prod_Name == comboBox1.Text && a.Company_ID == logIn.company && a.Prod_Status_ID ==1
                          select new { a.Prod_Name, u.Uom_Descr, a.Prod_Code }).ToList();
                if (sa.Count > 0)
                {

                    txtuom.Text = sa[0].Uom_Descr;
                    txtProdCode.Text = sa[0].Prod_Code.ToString();
                }

                dtpFrmDate.Value = logIn.fy_Start_Date;
                dtpToDate.Value = DateTime.Now;
                binddata();

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "")
                {
                    var sa = (from a in db.Products
                              join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                              where a.prod_ID == Convert.ToInt32(comboBox1.SelectedValue) && a.Company_ID == logIn.company && a.Prod_Status_ID ==1
                              select new { a.Prod_Name, u.Uom_Descr, a.Prod_Code }).ToList();
                    if (sa.Count > 0)
                    {

                        txtuom.Text = sa[0].Uom_Descr;
                        txtProdCode.Text = sa[0].Prod_Code.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    // creating new WorkBook within Excel application  
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                    // creating new Excelsheet in workbook  

                    //Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    //Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                    var data = (from s in db.Company_Infos
                                where s.Id == logIn.company
                                select new
                                {
                                    s.Company_Name,
                                    Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                                }).ToList();
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet.Cells[1, 1] = data[0].Company_Name.ToString();
                    worksheet.Cells[2, 1] = "STock Ledger";
                    worksheet.Cells[3, 4] = "Period :" + dtpFrmDate.Text + " To " + dtpToDate.Text;
                    worksheet.Cells[3, 1] = "Product Name : " + comboBox1.Text;
                    worksheet.Range["A1:d1"].MergeCells = true;
                    worksheet.Range["A2:d2"].MergeCells = true;
                    worksheet.Range["A3:c3"].MergeCells = true;
                    worksheet.Range["D3:F3"].MergeCells = true;



                    for (int j = 1; j < dataGridView1.Columns.Count+1; j++)
                    {
                        worksheet.Cells[4, j] = dataGridView1.Columns[j - 1].HeaderText;
                    }
                    ///*a*/pp.Visible = true;
                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < dataGridView1.Rows.Count; k++)
                    {
                        for (int l = 0; l < dataGridView1.Columns.Count; l++)
                        {
                           
                            worksheet.Cells[k + 5, l + 1] = (dataGridView1.Rows[k].Cells[l].Value == "" || dataGridView1.Rows[k].Cells[l].Value == null ||
                                dataGridView1.Rows[k].Cells[l].Value == DBNull.Value) ? "" : dataGridView1.Rows[k].Cells[l].Value.ToString();
                          
                        }
                    }



                    worksheet.PageSetup.PrintGridlines = true;
                    worksheet.PageSetup.LeftMargin = 1.00;
                    worksheet.PageSetup.RightMargin = 0.50;
                    worksheet.Columns.AutoFit();
                    app.Visible = true;

                    //ExportToExcel(dataGridView1, "Invoice_Report");
                }



                //if (dataGridView1.Rows.Count > 0)
                //{
                //    ExportToExcel(dataGridView1, "Account_Ledger");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //DateTime dtt = dtpToDate.Value;
                //string dt2 = dtt.ToString("yyyy/MM/dd");
                string dt2 = "2020/04/01";
                var Prodname = (from d in db.Material_Issue_Childs
                                join m in db.Material_Issue_Masters on d.Slip_Master_ID equals m.Id
                                where d.Prod_Code == Convert.ToInt32(comboBox1.SelectedValue.ToString())
                                && m.Slip_Date >= Convert.ToDateTime(dt2)
                                orderby m.Slip_Date
                                select new { d.Prod_Code, m.Slip_Date }).ToList();
                // DataTable dt = new DataTable();
                //dt.Columns.Add("Raw_Material");
                string dt4 = "";
                foreach (var item in Prodname)
                {

                    DateTime dt = item.Slip_Date.Value;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    DateTime dt3 = item.Slip_Date.Value;
                    if (dt4 == "")
                    {
                        dt4 = dt3.ToString("yyyy/MM/dd");
                    }





                    SqlDataReader rdr = null;

                    SqlCommand cmd2 = new SqlCommand("UpdateIssues_new", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@Company", logIn.company);
                    cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(comboBox1.SelectedValue.ToString()));
                    cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt1));
                    cmd2.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt4));
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Close();
                        con.Open();
                    }
                    //  con.Open();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    //da2.Fill(ds2);
                    rdr = cmd2.ExecuteReader();
                    con.Close();
                    dt4 = dt1;
                    //dataGridView1.Rows[k].Cells[l].Value
                    //var cellVaue = ds2.Rows[0].;
                    //var ci = db.Material_Issue_Childs.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                    //{
                    //    ci.Status = 26;
                    //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                    //    db.SubmitChanges();
                    //}

                    //DataSet ds2 = new DataSet();

                }

                if(logIn.company == 20)
                {


                    var Prodname1 = (from d in db.Invoice_Childs
                                    join m in db.Invoice_Masters on d.Inv_Master_ID equals m.Id
                                    where d.Prod_Code == comboBox1.SelectedValue.ToString()
                                    && m.InvDate >= Convert.ToDateTime(dt2)
                                     orderby m.InvDate
                                    select new { d.Prod_Code, m.InvDate }).ToList();
                    // DataTable dt = new DataTable();
                    //dt.Columns.Add("Raw_Material");
                    string dt5 = "";
                    foreach (var item in Prodname1)
                    {

                        DateTime dt = item.InvDate.Value;
                        string dt1 = dt.ToString("yyyy/MM/dd");

                        DateTime dt3 = item.InvDate.Value;
                        if (dt5 == "")
                        {
                            dt5 = dt3.ToString("yyyy/MM/dd");
                        }





                        SqlDataReader rdr = null;

                        SqlCommand cmd2 = new SqlCommand("UpdateInvValue_new", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@Company", logIn.company);
                        cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(comboBox1.SelectedValue.ToString()));
                        cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt1));
                        cmd2.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt5));
                        cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        if (con.State != ConnectionState.Open)
                        {
                            con.Close();
                            con.Open();
                        }
                        //  con.Open();
                        DataTable ds2 = new DataTable();
                        // da2.Fill(ds2, "x");
                        //da2.Fill(ds2);
                        rdr = cmd2.ExecuteReader();
                        con.Close();
                        dt5 = dt1;
                        

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //DateTime dtt = dtpToDate.Value;
            //string dt2 = dtt.ToString("yyyy/MM/dd");
            string dt2 = "2020/04/01";
            var Prodname = (from d in db.GatePass_Childs
                            join m in db.GatePass_Masters on d.GP_Master_ID equals m.Id
                            where d.Prod_Code == Convert.ToInt32(comboBox1.SelectedValue.ToString())
                            && m.GP_Date >= Convert.ToDateTime(dt2)
                            orderby m.GP_Date
                            select new { d.Prod_Code, m.GP_Date }).ToList();
            // DataTable dt = new DataTable();
            //dt.Columns.Add("Raw_Material");
            string dt4 = "";
            foreach (var item in Prodname)
            {

                DateTime dt = item.GP_Date.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dt3 = item.GP_Date.Value;
                if (dt4 == "")
                {
                    dt4 = dt3.ToString("yyyy/MM/dd");
                }





                SqlDataReader rdr = null;

                SqlCommand cmd2 = new SqlCommand("UpdateGatePassValue", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@Company", logIn.company);
                cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(comboBox1.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt4));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                //  con.Open();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                rdr = cmd2.ExecuteReader();
                con.Close();
                dt4 = dt1;
                //dataGridView1.Rows[k].Cells[l].Value
                //var cellVaue = ds2.Rows[0].;
                //var ci = db.Material_Issue_Childs.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                //{
                //    ci.Status = 26;
                //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                //    db.SubmitChanges();
                //}

                //DataSet ds2 = new DataSet();

            }
        }
    }
}
