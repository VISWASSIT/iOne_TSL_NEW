using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
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
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid.Interactivity;
using ioneNet.QulaityManagement.Transactions;
using ioneNet.Qulaity_Management.Transactions;

namespace ioneNet.QulaityManagement.Reports
{
    public partial class FG_Lot_Register : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string ReportRef;
        public FG_Lot_Register()
        {
            InitializeComponent();
        }

        private void FG_Lot_Register_Load(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");
                var d = (from data in db.Sp_FG_LotRegister(logIn.company, dtpFrmDate.Value, dtpToDate.Value) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    //  this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Section_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Section_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Section_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Section_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["FG_Batch_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["FG_Batch_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["FG_Batch_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["FG_Batch_No"].FilterRowCondition = FilterRowCondition.Contains;

                   


                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dtpToDate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "FG Lot Register";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\FG_LOT_Register.xlsx");
            string doc = Fname + "\\FG_LOT_Register.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfDataGrid1_CurrentCellActivated(object sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellActivatedEventArgs e)
        {
            try
            {

                //dataGridView1.DataSource = null;
                //dataGridView1.ColumnCount = 0;
                //dataGridView1.Columns.Add("Sample_ID", "Sample_ID");

                int r = sfDataGrid1.CurrentCell.RowIndex;
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                if (r >= 2)
                {
                    var c = sfDataGrid1.CurrentCell.Column.MappingName;
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(r);
                    var mappingName = sfDataGrid1.Columns["FG_Batch_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Section_Name"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Material_Grade"].MappingName;
                    var mappingName3 = sfDataGrid1.Columns["Batch_Qty"].MappingName;


                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                    var cellVaue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());

                    txtBatchNo.Text = cellVaue.ToString();
                    txtSectionName.Text = cellVaue1.ToString();
                    txtGrade.Text = cellVaue2.ToString();
                    txtBatchQty.Text = cellVaue3.ToString();




                    //Lot Traceability Data
                    SqlCommand cmd2 = new SqlCommand("SP_Get_RM_Lots_FGBatch", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@FG_Batch_No", txtBatchNo.Text);
                    //cmd2.Parameters.AddWithValue("@RM_Grade", cellVaue1);
                    //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    ;
                    if (ds2.Rows.Count >= 0)
                    {
                        dataGridView1.DataSource = ds2;
                    }


                    //Lot TC issued  Data
                    SqlCommand cmd3 = new SqlCommand("SP_Get_TC_FGBatch", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@compname", logIn.company);
                    cmd3.Parameters.AddWithValue("@FG_Batch_No", txtBatchNo.Text);
                    //cmd2.Parameters.AddWithValue("@RM_Grade", cellVaue1);
                    //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    //DataSet ds2 = new DataSet();
                    DataTable ds3 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da3.Fill(ds3);
                    ;
                    if (ds3.Rows.Count >= 0)
                    {
                        dataGridView2.DataSource = ds3;
                    }

                    //Get Report Ref
                    linkLabel1.Text = "Not AVailable";
                    linkLabel2.Text = "Not AVailable";
                    linkLabel3.Text = "Not AVailable";
                    var da = (from obj in db.QA_Chmical_Report_Finals
                             
                              where obj.FG_Batch_No == txtBatchNo.Text && obj.Company_Id == logIn.company
                              select new { obj.Report_Ref_No }).ToList();

                    if (da.Count > 0)
                    {
                        linkLabel1.Text = da[0].Report_Ref_No;
                       

                    }
                    var da1 = (from obj in db.Inprocess_Dimensional_Reports

                              where obj.Fg_Lot_No == txtBatchNo.Text && obj.Company_Id == logIn.company && obj.Test_Type == "Mechanical"
                              select new { obj.Report_Ref_No }).ToList();

                    if (da1.Count > 0)
                    {
                        linkLabel2.Text = da1[0].Report_Ref_No;


                    }
                    var d3 = (from obj in db.Inprocess_Dimensional_Reports

                               where obj.Fg_Lot_No == txtBatchNo.Text && obj.Company_Id == logIn.company && obj.Test_Type == "Dimensional"
                               select new { obj.Report_Ref_No }).ToList();

                    if (d3.Count > 0)
                    {
                        linkLabel3.Text = d3[0].Report_Ref_No;


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel1.Text != "Not AVailable")
            {
                ReportRef = linkLabel1.Text;
                frmFinal_ChemcialReport frm = new frmFinal_ChemcialReport();
                //OrderManagement.Transactions.
                //frm.MdiParent = this.MdiParent;
                frm.ShowDialog();
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel2.Text != "Not AVailable")
            {
                ReportRef = linkLabel2.Text;
                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                frmFinalInspReport_Mechanical frm = new frmFinalInspReport_Mechanical();
                //OrderManagement.Transactions.
                //frm.MdiParent = this.MdiParent;
                frm.ShowDialog();
                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel3.Text != "Not AVailable")
            {
                ReportRef = linkLabel3.Text;
                frmFinal_DimensionalTest frm = new frmFinal_DimensionalTest();
                //OrderManagement.Transactions.
                //frm.MdiParent = this.MdiParent;
                frm.ShowDialog();
            }
        }
    }
}
