using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.GroupingGridExcelConverter;
using Syncfusion.Grouping;
using Syncfusion.GridExcelConverter;
using Syncfusion.Drawing;
using Syncfusion.Windows.Forms.Grid;
using Ione_DAL;
using System.Data.SqlClient;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Diagnostics;

namespace ioneNet.FinanceManagement.Reports
{
  
    public partial class frmTrailBalance_Tree : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        //private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        public frmTrailBalance_Tree()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Summary = GetParentTable();
                DataTable Project_Transaction = GetChildTable();
                //DataTable grandChildTable = GetGrandChildTable();
                GridRelationDescriptor parentToChildRelationDescriptor = new GridRelationDescriptor();
                //Same as SourceListSetEntry.Name for Child Table.
                parentToChildRelationDescriptor.ChildTableName = "Account Data";


                parentToChildRelationDescriptor.RelationKind = RelationKind.RelatedMasterDetails;
                parentToChildRelationDescriptor.RelationKeys.Add("AccGroup", "AccGroup");

                //Adds relation to Parent Table.
                gridGroupingControl1.TableDescriptor.Relations.Add(parentToChildRelationDescriptor);
                GridRelationDescriptor childToGrandChildRelationDescriptor = new GridRelationDescriptor();
                //this.gridGroupingControl1.TableSummaryRows.Clear();
                this.gridGroupingControl1.Engine.SourceListSet.Add("Trail Blance - Group Wise", Summary);
                this.gridGroupingControl1.Engine.SourceListSet.Add("Account Data", Project_Transaction);

                gridGroupingControl1.DataSource = Summary;

                GridTableDescriptor tableDescriptor = this.gridGroupingControl1.GetTableDescriptor("Account Data");
                tableDescriptor.Appearance.AnyRecordFieldCell.BackColor = Color.FromArgb(223, 247, 252);
                tableDescriptor.Appearance.AlternateRecordFieldCell.BackColor = Color.FromArgb(255, 229, 201);

                //Column Header Cell styles.
                tableDescriptor.Appearance.ColumnHeaderCell.Interior = new BrushInfo(GradientStyle.Vertical, Color.FromArgb(203, 201, 202), Color.FromArgb(253, 247, 215));
                tableDescriptor.Appearance.ColumnHeaderCell.TextColor = Color.Black;
                //Group Caption Cell styles.
                tableDescriptor.Appearance.GroupCaptionCell.Interior = new BrushInfo(Color.FromArgb(255, 238, 220));
                tableDescriptor.Appearance.GroupCaptionCell.Borders.Bottom = new GridBorder(GridBorderStyle.Solid, Color.FromArgb(242, 158, 32), GridBorderWeight.Medium);
                if (checkBox1.Checked)
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
        }
        private int numberParentRows = 5;
        private int numberChildRows = 20;
        private DataTable GetParentTable()
        {
            DataTable dataTable = new DataTable("Summary");
            //dataTable.Columns.Add(new DataColumn("Parent_ID"));
            dataTable.Columns.Add(new DataColumn("AccGroup"));
            dataTable.Columns.Add(new DataColumn("Opening_Bal"));
            dataTable.Columns.Add(new DataColumn("Opening_Bal_Type"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            dataTable.Columns.Add(new DataColumn("Debit"));
            dataTable.Columns.Add(new DataColumn("Closing_Bal"));
            dataTable.Columns.Add(new DataColumn("Closing_Bal_Type"));
            
            DateTime dt = dpFromDate.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dt2 = dtpToDate.Value;
            string dt3 = dt2.ToString("yyyy/MM/dd");
            var d = (from data in db.sp_TrailBalance_GroupWise(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt3), logIn.fy_Start_Date,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                numberParentRows = d.Count;
                for (int i = 0; i < numberParentRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    //dataRow[0] = i;
                    //dataRow[0] = d[i].AccGroup;
                    //dataRow[1] = d[i].Opening_Bal;
                    //dataRow[2] = d[i].Opening_Bal_Type;
                    //dataRow[3] = d[i].Credit;
                    //dataRow[4] = d[i].Debit;
                    //dataRow[5] = string.Format(d[i].Closing_Bal.ToString(), i);
                    //dataRow[6] = string.Format(d[i].Closing_Bal_Type.ToString(), i);                   
                    //dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetChildTable()
        {
           
            DataTable dataTable = new DataTable("Project_Transaction");
            //dataTable.Columns.Add(new DataColumn("childID"));
            //dataTable.Columns.Add(new DataColumn("Project_Code"));
            dataTable.Columns.Add(new DataColumn("Acc_Id"));
            dataTable.Columns.Add(new DataColumn("AccGroup"));
            dataTable.Columns.Add(new DataColumn("AccName"));
            dataTable.Columns.Add(new DataColumn("Opening_Bal"));
            dataTable.Columns.Add(new DataColumn("OBType"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            dataTable.Columns.Add(new DataColumn("Debit"));
            dataTable.Columns.Add(new DataColumn("Closing_Bal"));
            dataTable.Columns.Add(new DataColumn("CBType"));
            //dataTable.Columns.Add(new DataColumn("Parent_ID"));

            DateTime dt = dpFromDate.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dt2 = dtpToDate.Value;
            string dt3 = dt2.ToString("yyyy/MM/dd");
            var d = (from data in db.sp_TrailBalance(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt3), logIn.fy_Start_Date,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                    try { 
                numberChildRows = d.Count;
                for (int i = 0; i < numberChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    //dataRow[0] = i.ToString();
                    //dataRow[1] = string.Format(d[i].Project_Code.ToString(), i);
                    dataRow[0] = string.Format(d[i].Acc_Id.ToString(), i);                   
                    dataRow[1] = string.Format(d[i].AccGroup.ToString(), i);
                    dataRow[2] = string.Format(d[i].AccName.ToString(), i);
                    dataRow[3] = string.Format(d[i].Opening_Bal.ToString(), i);
                   
                   
                    dataRow[4] = string.Format(d[i].OBType.ToString(), i);
                    dataRow[5] = string.Format(d[i].Credit.ToString(), i);
                    dataRow[6] = string.Format(d[i].Debit.ToString(), i);
                   
                 
                    dataRow[7] = string.Format(d[i].Closing_Bal.ToString(), i);
                    dataRow[8] = string.Format(d[i].CBType.ToString(), i);
                      
                   
                    
                  
                    dataTable.Rows.Add(dataRow);
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            return dataTable;
            
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            GroupingGridExcelConverterControl converter = new GroupingGridExcelConverterControl();
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = Fname + "\\TrailBalance_Group.xlsx";
            // Export the contents of the Grid to Excel
            converter.GroupingGridToExcel(this.gridGroupingControl1, doc, ConverterOptions.Visible);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                
               
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dt2 = dtpToDate.Value;
                string dt3 = dt2.ToString("yyyy/MM/dd");

                SqlCommand cmd = new SqlCommand("sp_TrailBalance_GroupWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(dt1));
                cmd.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt3));
                cmd.Parameters.AddWithValue("@fystartdate", Convert.ToDateTime(dt1));


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);

                SqlCommand cmd1 = new SqlCommand("sp_TrailBalance", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@compname", logIn.company);
                cmd1.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(dt1));
                cmd1.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt3));
                cmd1.Parameters.AddWithValue("@fystartdate", Convert.ToDateTime(dt1));


                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                DataTable Dt1 = new DataTable();

                da1.SelectCommand = cmd1;
                da1.Fill(Dt1);

                //if (Dt.Rows.Count > 0)
                //{
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company

                rep = new FinanceManagement.Reports.TrailBalance();

                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

                crParameterDiscreteValue.Value = logIn.company;
                crParameterFieldDefinitions = rep.DataDefinition.ParameterFields;
                crParameterFieldDefinition = crParameterFieldDefinitions[0];
                crParameterValues = crParameterFieldDefinition.CurrentValues;

                crParameterValues.Clear();
                crParameterValues.Add(crParameterDiscreteValue);
                crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);



                //rep.SetParameterValue("@compname", logIn.company);
                //rep.SetParameterValue("@fromDate", Convert.ToDateTime(dt1));
                //rep.SetParameterValue("@todate", Convert.ToDateTime(dt3));
                //rep.SetParameterValue("@fystartdate", Convert.ToDateTime(dt1));


                //rep.SetParameterValue("@compname", logIn.company, "AccountWiseData");
                //rep.SetParameterValue("@fromDate", Convert.ToDateTime(dt1), "AccountWiseData");
                //rep.SetParameterValue("@todate", Convert.ToDateTime(dt3), "AccountWiseData");
                //rep.SetParameterValue("@fystartdate", Convert.ToDateTime(dt1), "AccountWiseData");

                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }
                rep.SetDataSource(Dt);
                rep.OpenSubreport("AccountWiseData").SetDataSource(Dt1);


                //path = Path.Combine(Directory.GetCurrentDirectory(), "TB_GroupWise.pdf");
                //string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);
                //rep = new ioneNet.FinanceManagement.Reports.rptBalanceSheet();
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                viewer.crystalReportViewer1.ReportSource = rep;
                //viewer.crystalReportViewer1.Refresh();
                viewer.Show();
                //rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //cmd.Parameters.Clear();
                //Process.Start(path);
                //rep.SetParameterValue(1, logIn.company);
                //rep.SetParameterValue(2, dateTimePicker1.Value);

                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                
                //this.crystalReportViewer1.Refresh();
                //rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                // Process.Start(path);
                //}
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
    }
}
