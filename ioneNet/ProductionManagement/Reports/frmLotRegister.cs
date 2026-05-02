using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
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
//using Syncfusion.WinForms.DataGrid.DataGridConverter;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Interactivity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Syncfusion.WinForms.DataGrid.Events;
using ioneNet.Qulaity_Management.Transactions;


namespace ioneNet.ProductionManagement.Reports
{
    public partial class frmLotRegister: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmLotRegister()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                string fdate = "2024-04-01";
                var d1 = (from data in db.Sp_LotRegister(logIn.company) select data).ToList();
                if (d1.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d1;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;


                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["LOT_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["LOT_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["LOT_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["LOT_No"].FilterRowCondition = FilterRowCondition.Contains;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmLotRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            //DateTime dt = dtpFrmDate.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");



            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;

            workBook.Worksheets[0].Range["A2"].Value = "LOT Register";

            //workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = "";

            workBook.SaveAs(Fname + "\\Lot_Register.xlsx");
            doc = Fname + "\\Lot_Register.xlsx";


            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
        public string mtrlGrade;
        private void sfDataGrid1_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            try
            {

                dataGridView1.DataSource = null;
                //dataGridView1.ColumnCount = 0;
                //dataGridView1.Columns.Add("Sample_ID", "Sample_ID");

                int r = sfDataGrid1.CurrentCell.RowIndex;
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var c = sfDataGrid1.CurrentCell.Column.MappingName;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(r);
            var mappingName = sfDataGrid1.Columns["LOT_No"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["Prod_Grade"].MappingName;
            //var currentAmendValue = (rowData.GetType().GetProperty("Po_Amend_No").GetValue(rowData, null).ToString());
            //var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
            //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            //if (c == "Item_Name")
            //{
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

            mtrlGrade = cellVaue1;
             var sa = (from sq in db.Incoming_Chemical_Reports
                      join g in db.GoodsReceiptNote_Masters on sq.GRN_Master_Id equals g.Id
                      join gc in db.GoodsReceiptNote_Childs on g.Id equals gc.GRN_Master_ID
                      join su in db.Supplier_informations on g.SupplierName equals su.ID
                      join p in db.Products on gc.Prod_Code equals p.prod_ID
                      where sq.Company_Id == logIn.company && sq.New_Batch_No == cellVaue
                      orderby sq.id
                      select new
                      {                         
                          sq.Chemical_Readings,                         
                          gc.Prod_Grade,                          
                          sq.Accepted_Grade,
                          sq.New_Batch_No,
                          sq.Sample_ID
                      }).ToList();
            if (sa.Count > 0)
            {
                string p;
                DataTable dt = new DataTable();
                mtrlGrade = sa[0].Prod_Grade;
                
                //dataGridView1.Columns[0].HeaderText = "Sample_ID";

                CallChemParamters();
                System.Data.DataRow dr;
                dr = dt.NewRow();
                int k = dataGridView1.Rows.Count;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dt.Columns.Add(dataGridView1.Columns[i].Name);

                    dr[dataGridView1.Columns[i].Name] = dataGridView1.Rows[0].Cells[i].Value.ToString();

                }


                //for (int i = 0; i < dataGridView1.ColumnCount; i++)
                //{
                //    dr[dataGridView1.Columns[i].Name] = dataGridView1.Rows[0].Cells[i].Value.ToString();
                //}
                dt.Rows.Add(dr);
                int ColIndex = 0;
                int gridcolcount = dt.Columns.Count;
                for (int j = 0; j < sa.Count; j++)
                {

                    dr = dt.NewRow();
                    string s = sa[j].Chemical_Readings;
                    string[] values = s.Split(',');

                    dr[0] = sa[j].Sample_ID;
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        p = values[i].Trim();

                        //DataRow dr;

                        dr[i + 1] = p;
                        ColIndex = i + 1;

                    }
                    dr[gridcolcount - 2] = sa[j].Accepted_Grade;
                    dr[gridcolcount - 1] = sa[j].New_Batch_No;

                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;

                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;

            }

                //Lot Traceability Data
                SqlCommand cmd2 = new SqlCommand("Sp_LotTraceabilityReport", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@RM_Lot_No", cellVaue);
                //cmd2.Parameters.AddWithValue("@RM_Grade", cellVaue1);
                //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                this.sfDataGrid3.TableSummaryRows.Clear();
                //    var d = (from data in db.ShowStockReport_New(logIn.company, dt1, dt2, logIn.BU_ID) select data).ToList();
                if (ds2.Rows.Count > 0)
                {
                    sfDataGrid3.DataSource = ds2;

                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Issued Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "Total : {Sum}";
                    summaryColumn1.MappingName = "Lot_Issue_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                    this.sfDataGrid3.TableSummaryRows.Add(tableSummaryRow1);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CallChemParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_ChemicalTest_Parameters", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@mtrlgrade", mtrlGrade);

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dataGridView1.DataSource = ds2;
            dataGridView1.Rows[0].Cells["Sample_ID"].Value = "Spec";
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            //dataGridView1.ColumnCount = 0;
            //dataGridView1.Columns.Add("Sample_ID", "Sample_ID");
        }
    }
}
