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
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.GroupingGridExcelConverter;
using Syncfusion.Grouping;
using Syncfusion.GridExcelConverter;
using Syncfusion.Drawing;
using Syncfusion.Windows.Forms.Grid;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Reports
{
   
    public partial class Lot_Allotment_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public Lot_Allotment_Report()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable Summary = GetParentTable();
                DataTable Project_Transaction = GetChildTable();
                //DataTable grandChildTable = GetGrandChildTable();
                GridRelationDescriptor parentToChildRelationDescriptor = new GridRelationDescriptor();
                //Same as SourceListSetEntry.Name for Child Table.
                parentToChildRelationDescriptor.ChildTableName = "FG Wise Allotment Data";
               

                parentToChildRelationDescriptor.RelationKind = RelationKind.RelatedMasterDetails;
                parentToChildRelationDescriptor.RelationKeys.Add("RM_Lot_No", "RM_Lot_No");

                //Adds relation to Parent Table.
                gridGroupingControl1.TableDescriptor.Relations.Add(parentToChildRelationDescriptor);
                GridRelationDescriptor childToGrandChildRelationDescriptor = new GridRelationDescriptor();
                //this.gridGroupingControl1.TableSummaryRows.Clear();
                this.gridGroupingControl1.Engine.SourceListSet.Add("LOT_Summary", Summary);
                this.gridGroupingControl1.Engine.SourceListSet.Add("FG Wise Allotment Data", Project_Transaction);
              
                gridGroupingControl1.DataSource = Summary;

                GridTableDescriptor tableDescriptor = this.gridGroupingControl1.GetTableDescriptor("FG Wise Allotment Data");
                tableDescriptor.Appearance.AnyRecordFieldCell.BackColor = Color.FromArgb(223, 247, 252);
                tableDescriptor.Appearance.AlternateRecordFieldCell.BackColor = Color.FromArgb(255, 229, 201);

                //Column Header Cell styles.
                tableDescriptor.Appearance.ColumnHeaderCell.Interior = new BrushInfo(GradientStyle.Vertical, Color.FromArgb(203, 201, 202), Color.FromArgb(253, 247, 215));
                tableDescriptor.Appearance.ColumnHeaderCell.TextColor = Color.Black;
                //Group Caption Cell styles.
                tableDescriptor.Appearance.GroupCaptionCell.Interior = new BrushInfo(Color.FromArgb(255, 238, 220));
                tableDescriptor.Appearance.GroupCaptionCell.Borders.Bottom = new GridBorder(GridBorderStyle.Solid, Color.FromArgb(242, 158, 32), GridBorderWeight.Medium);
                if(checkBox1.Checked)
                {
                    this.gridGroupingControl1.Table.ExpandAllRecords();
                }
                else
                {
                    this.gridGroupingControl1.Table.CollapseAllRecords();
                }    
                
                //foreach (Group g in this.gridGroupingControl1.Table.TopLevelGroup.Groups) foreach (Record r in g.Records) if (r.NestedTables[0].ChildTable.Records.Count > 0) g.IsExpanded = true; r.IsExpanded = true;
            

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }





            //if (cmbMONo.Text != "")
            //{
            //    var d = (from data in db.ProjectStatusReport(logIn.company, cmbMONo.Text) select data).ToList();
            //    if (d.Count > 0)
            //    {
            //        //dgProductsList.DataSource = d;
            //        sfDataGrid1.DataSource = d;

            //        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
            //        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
            //        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
            //        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
            //        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

            //        this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
            //        this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
            //        this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
            //        this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Select MO No to Generate The Report");
            //}
        }

        private int numberParentRows = 5;
        private int numberChildRows = 20;
        private DataTable GetParentTable()
        {
            DataTable dataTable = new DataTable("Summary");
            //dataTable.Columns.Add(new DataColumn("Parent_ID"));
            dataTable.Columns.Add(new DataColumn("prod_name"));
            dataTable.Columns.Add(new DataColumn("material_grade"));
            dataTable.Columns.Add(new DataColumn("RM_Lot_No"));
            dataTable.Columns.Add(new DataColumn("Lot_Qty"));
            dataTable.Columns.Add(new DataColumn("Qty_Alloted"));
            dataTable.Columns.Add(new DataColumn("Lot_Balance_Qty"));
            

            var d = (from data in db.Sp_LotAllotment_Report(logIn.company) select data).ToList();
            if (d.Count > 0)
            {
                numberParentRows = d.Count;
                for (int i = 0; i < numberParentRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    //dataRow[0] = i;
                    dataRow[0] = d[i].prod_name;
                    dataRow[1] = d[i].material_grade;
                    dataRow[2] = d[i].RM_Lot_No;                   
                    dataRow[3] = string.Format(d[i].Lot_Qty.ToString(), i);
                    dataRow[4] = string.Format(d[i].Qty_Alloted.ToString(), i);
                    dataRow[5] = string.Format(d[i].Lot_Balance_Qty.ToString(), i);
                    
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetChildTable()
        {
            DataTable dataTable = new DataTable("Lot_Allotment_ProductWIse");
            //dataTable.Columns.Add(new DataColumn("childID"));
            //dataTable.Columns.Add(new DataColumn("Project_Code"));
            dataTable.Columns.Add(new DataColumn("RM_Lot_No"));
            dataTable.Columns.Add(new DataColumn("Allocation_Date"));

            dataTable.Columns.Add(new DataColumn("FG_Prod_Name"));
            dataTable.Columns.Add(new DataColumn("Qty_Alloted"));
            
            var d = (from data in db.Sp_LotAllotment_Report_Child(logIn.company) select data).ToList();
            if (d.Count > 0)
            {
                numberChildRows = d.Count;
                for (int i = 0; i < numberChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    //dataRow[0] = i.ToString();
                    //dataRow[1] = string.Format(d[i].Project_Code.ToString(), i);
                    dataRow[0] = string.Format(d[i].RM_Lot_No.ToString(), i);
                    if (d[i].FG_Prod_Name != null)
                    {
                        dataRow[1] = string.Format(d[i].prod_date.ToString(), i);
                        dataRow[2] = string.Format(d[i].FG_Prod_Name.ToString(), i);
                        dataRow[3] = string.Format(d[i].Qty_Alloted.ToString(), i);
                    }
                    
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {

            GroupingGridExcelConverterControl converter = new GroupingGridExcelConverterControl();
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = Fname + "\\Project_Status_Report.xlsx";
            // Export the contents of the Grid to Excel
            converter.GroupingGridToExcel(this.gridGroupingControl1, doc, ConverterOptions.Visible);

            //DateTime dt = dtpFrmDate.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");



            //var options = new ExcelExportingOptions();
            //options.StartRowIndex = 5;
            //var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            //var workBook = excelEngine.Excel.Workbooks[0];
            //var ws = excelEngine.Excel.Worksheets[1];
            //workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            //workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            //workBook.Worksheets[0].Range["A2"].Value = "Project Status Report";
            ////workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            //workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            //workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.Zoom = 85;
            //workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            //string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //workBook.SaveAs(Fname + "\\Project_Status_Report.xlsx");
            //string doc = Fname + "\\Project_Status_Report.xlsx";
            //Process prc = new Process();
            //prc.StartInfo.FileName = doc;
            //prc.Start();
        }

        private void ProjectStatusReport_Load(object sender, EventArgs e)
        {
            //Bind Products
            //var sa = (from a in db.Engg_Mfg_Orders
            //          where a.Company_ID == logIn.company
            //          select new { a.MO_No }).ToList();
            //if (sa.Count > 0)
            //{
            //    cmbMONo.DataSource = sa;
            //    cmbMONo.DisplayMember = "MO_No";
            //    cmbMONo.ValueMember = "MO_No";
            //    if (cmbMONo.Items.Count > 0)
            //    {
            //        cmbMONo.SelectedIndex = -1;
            //    }
            //    else
            //    {
            //        cmbMONo.SelectedIndex = -1;
            //    }
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
