using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;

namespace ioneNet.FinanceManagement.Reports
{
    public partial class frmTrailBalance : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static int intAccCode = 0;
        public static DateTime toDate;
        public frmTrailBalance()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                binddata_GroupWise();
            }
            else
            {
                binddata();
            }
        }
        public void binddata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dt2 = dtpToDate.Value;
                string dt3 = dt2.ToString("yyyy/MM/dd");


                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();
                if (checkBox1.Checked)
                {
                    var Buyerblind = (from m in db.sp_TrailBalance_OB(logIn.company, Convert.ToDateTime(dt1), logIn.fy_Start_Date)
                                      select m).ToList();
                    sfDataGrid1.DataSource = Buyerblind;
                }
                else
                {
                    var Buyerblind = (from m in db.sp_TrailBalance(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt3), logIn.fy_Start_Date,logIn.BU_ID)
                                      select m).ToList();
                    sfDataGrid1.DataSource = Buyerblind;
                }

                this.sfDataGrid1.TableSummaryRows.Clear();

                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                //summaryColumn1.Name = "Total_OB";
                //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn1.Format = "{Sum}";
                //summaryColumn1.MappingName = "Opening_Bal";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_Debit";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Debit";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_Credit";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "Credit";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                //GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                //summaryColumn4.Name = "Total_CB";
                //summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn4.Format = "{Sum}";
                //summaryColumn4.MappingName = "Closing_Bal";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void binddata_GroupWise()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dt2 = dtpToDate.Value;
                string dt3 = dt2.ToString("yyyy/MM/dd");


                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();
                  var Buyerblind = (from m in db.sp_TrailBalance_GroupWise(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt3), logIn.fy_Start_Date, logIn.BU_ID)
                                      select m).ToList();
                 sfDataGrid1.DataSource = Buyerblind;
               

                this.sfDataGrid1.TableSummaryRows.Clear();

                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                //summaryColumn1.Name = "Total_OB";
                //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn1.Format = "{Sum}";
                //summaryColumn1.MappingName = "Opening_Bal";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_Debit";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Debit";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_Credit";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "Credit";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                //GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                //summaryColumn4.Name = "Total_CB";
                //summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn4.Format = "{Sum}";
                //summaryColumn4.MappingName = "Closing_Bal";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dpFromDate.Value;
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;            
            
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            string dt1 = dt.ToString("dd/MM/yyyy");
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Trail Balance";
            workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1 ;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Portrait;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);                      
           
            workBook.SaveAs(Fname + "\\TB.xlsx");
            string doc = Fname + "\\TB.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            var document = this.sfDataGrid1.ExportToPdf();           
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);



            document.Save(Fname + "\\TB.xlsx");
        }

        private void frmTrailBalance_Load(object sender, EventArgs e)
        {
            dpFromDate.MinDate = logIn.fy_Start_Date;
            dpFromDate.MaxDate = logIn.fy_End_Date;
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {

            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["Acc_Id"].MappingName;
            var acc_id = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            intAccCode = Convert.ToInt32(acc_id);
            toDate = dpFromDate.Value;
            //if (dataGridView1.Rows[0].Cells["BalType"].Value == "Cr")
            //vchedit = true;
            //vchno = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Voucher_No"].Value.ToString();
            //vchtype = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["TransType"].Value.ToString();
            ioneNet.FinanaceManagement.Reports.frmAccountLedger frm = new ioneNet.FinanaceManagement.Reports.frmAccountLedger();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
