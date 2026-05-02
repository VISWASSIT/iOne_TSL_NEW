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
namespace ioneNet.MaterialManagement.Reports
{
   
    public partial class StockStatement_SubContractor : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public static int prodcode = 0;
        public static int subcontractor = 0;
        public static DateTime toDate;
        public StockStatement_SubContractor()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                sfDataGrid1.DataSource = null;
               var d = (from data in db.StockReport_SubContractor(logIn.company,dtpFrmDate.Value,dateTimePicker1.Value, Convert.ToInt32(cmbCustomer.SelectedValue),logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[11].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        decimal stk = Convert.ToDecimal(cellVaue);
                        if (stk < 1000)
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 11), Color.Red);
                        }
                        else
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 11), Color.White);
                        }
                    }
                    this.sfDataGrid1.TableSummaryRows.Clear();
                    //this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "OBValue";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "OBValue";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                GridSummaryColumn summaryColumnOBQty1 = new GridSummaryColumn();
                summaryColumnOBQty1.Name = "OBQty";
                summaryColumnOBQty1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnOBQty1.Format = "{Sum}";
                summaryColumnOBQty1.MappingName = "OBQty";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnOBQty1);


                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "ReceiptValue";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "ReceiptValue";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumnReceipt2 = new GridSummaryColumn();
                summaryColumnReceipt2.Name = "ReceiptQty";
                summaryColumnReceipt2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt2.Format = "{Sum}";
                summaryColumnReceipt2.MappingName = "ReceiptQty";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "IssueValue";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "IssueValue";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumnIss3 = new GridSummaryColumn();
                summaryColumnIss3.Name = "IssueQty";
                summaryColumnIss3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnIss3.Format = "{Sum}";
                summaryColumnIss3.MappingName = "IssuedQty";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnIss3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "ClosingValue";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "ClosingValue";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                GridSummaryColumn summaryColumnCBQty = new GridSummaryColumn();
                summaryColumnCBQty.Name = "ClosingQty";
                summaryColumnCBQty.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnCBQty.Format = "{Sum}";
                summaryColumnCBQty.MappingName = "ClosingQty";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnCBQty);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "OBValue";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "OBValue";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);

                GridSummaryColumn GsummaryColumnOBQty = new GridSummaryColumn();
                GsummaryColumnOBQty.Name = "OBQty";
                GsummaryColumnOBQty.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumnOBQty.Format = "{Sum}";
                GsummaryColumnOBQty.MappingName = "OBQty";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumnOBQty);

                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "ReceiptValue";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "ReceiptValue";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                GridSummaryColumn GsummaryColumnRQTY = new GridSummaryColumn();
                GsummaryColumnRQTY.Name = "ReceiptQty";
                GsummaryColumnRQTY.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumnRQTY.Format = "{Sum}";
                GsummaryColumnRQTY.MappingName = "ReceiptQty";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumnRQTY);

                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                GsummaryColumn3.Name = "IssueValue";
                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn3.Format = "{Sum}";
                GsummaryColumn3.MappingName = "IssueValue";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                GridSummaryColumn GsummaryColumniSS = new GridSummaryColumn();
                GsummaryColumniSS.Name = "IssuedQty";
                GsummaryColumniSS.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumniSS.Format = "{Sum}";
                GsummaryColumniSS.MappingName = "IssuedQty";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumniSS);

                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                GsummaryColumn4.Name = "ClosingValue";
                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn4.Format = "{Sum}";
                GsummaryColumn4.MappingName = "ClosingValue";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                GridSummaryColumn GsummaryColumncbqTY = new GridSummaryColumn();
                GsummaryColumncbqTY.Name = "ClosingQty";
                GsummaryColumncbqTY.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumncbqTY.Format = "{Sum}";
                GsummaryColumncbqTY.MappingName = "ClosingQty";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumncbqTY);

                // Adds the summary row in the GroupSummaryRows collection.
                this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dateTimePicker1.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Stock Statement";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\StockStatement.xlsx");
            string doc = Fname + "\\StockStatement.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void StockStatement_Load(object sender, EventArgs e)
        {
            var sa = (from k in db.Supplier_informations where k.Company_ID == logIn.company && k.Supplier_Category == 32 select new { k.Supplier_Name, k.ID }).ToList();
            if (sa.Count > 0)
            {
                cmbCustomer.DataSource = sa;
                cmbCustomer.DisplayMember = "Supplier_Name";
                cmbCustomer.ValueMember = "ID";
                if (cmbCustomer.Items.Count > 0)
                {
                    cmbCustomer.SelectedIndex = -1;
                }
                else
                {
                    cmbCustomer.SelectedIndex = -1;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["prod_ID"].MappingName;
            var prod_id = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            prodcode = Convert.ToInt32(prod_id);
            subcontractor = Convert.ToInt32(cmbCustomer.SelectedValue);
            toDate = dateTimePicker1.Value;
             ioneNet.MaterialManagement.Reports.StockLedger_SubContracto frm = new ioneNet.MaterialManagement.Reports.StockLedger_SubContracto();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
