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
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.GroupingGridExcelConverter;
using Syncfusion.Grouping;
using Syncfusion.GridExcelConverter;
using Ione_DAL;

namespace ioneNet.FinanceManagement.Reports
{
    public partial class BalanceSheet : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        //System.Data.Common.DbTransaction transaction;
        //SqlConnection objSqlConnection;
        //SqlCommand objSqlCommand;
        private Database crDatabase;
        private Tables crTables;
        //private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();



        public BalanceSheet()
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

                //  DataTable GroupType = GetParentTable();
                //  DataTable AccGroups = GetChildTable();
                //  DataTable AccSubGroups = GetSubGroupTable();
                //  DataTable Accounts = GetAccountTable();
                //  //DataTable grandChildTable = GetGrandChildTable();
                //  GridRelationDescriptor parentToChildRelationDescriptor = new GridRelationDescriptor();
                //  //Same as SourceListSetEntry.Name for Child Table.
                //  parentToChildRelationDescriptor.ChildTableName = "Account Groups";
                //  parentToChildRelationDescriptor.RelationKind = RelationKind.RelatedMasterDetails;
                //  parentToChildRelationDescriptor.RelationKeys.Add("GroupType", "GroupType");

                //  //Adds relation to Parent Table.
                //  gridGroupingControl1.TableDescriptor.Relations.Add(parentToChildRelationDescriptor);




                //  GridRelationDescriptor childToGrandChildRelationDescriptor = new GridRelationDescriptor();
                //  childToGrandChildRelationDescriptor.ChildTableName = "Account Sub Groups";
                //  childToGrandChildRelationDescriptor.RelationKind = RelationKind.RelatedMasterDetails;
                //  childToGrandChildRelationDescriptor.RelationKeys.Add("BalSheetHead", "BalSheetHead");
                //  parentToChildRelationDescriptor.ChildTableDescriptor.Relations.Add(childToGrandChildRelationDescriptor);

                //  GridRelationDescriptor childToGrandChildRelationDescriptor1 = new GridRelationDescriptor();
                //  childToGrandChildRelationDescriptor1.ChildTableName = "Accounts";
                //  childToGrandChildRelationDescriptor1.RelationKind = RelationKind.RelatedMasterDetails;
                //  childToGrandChildRelationDescriptor1.RelationKeys.Add("SubGroup", "SubGroup");
                //  childToGrandChildRelationDescriptor.ChildTableDescriptor.Relations.Add(childToGrandChildRelationDescriptor1);




                //  //this.gridGroupingControl1.TableSummaryRows.Clear();
                //  this.gridGroupingControl1.Engine.SourceListSet.Add("Account Type", GroupType);
                //  this.gridGroupingControl1.Engine.SourceListSet.Add("Account Groups", AccGroups);
                //  this.gridGroupingControl1.Engine.SourceListSet.Add("Account Sub Groups", AccSubGroups);
                //  this.gridGroupingControl1.Engine.SourceListSet.Add("Accounts", Accounts);
                //  //var d = (from data in db.Forge_Get_Mtrl_Availability_OrderQty(logIn.company) select data).ToList();
                //  //if (d.Count > 0)
                //  //{
                //  //dgProductsList.DataSource = d;
                //  gridGroupingControl1.DataSource = GroupType;

                ////  this.gridGroupingControl1.TableDescriptor.VisibleColumns.RemoveAt(5);
                //  this.gridGroupingControl1.TableDescriptor.VisibleColumns.RemoveAt(0);
                //  this.gridGroupingControl1.NestedTableGroupOptions.ShowColumnHeaders = false;

                // this.gridGroupingControl1.ChildGroupOptions.ShowColumnHeaders = true;

                //this.gridGroupingControl1.TopLevelGroupOptions.ShowColumnHeaders = false;

                //string path = Path.Combine(Directory.GetCurrentDirectory(), "BalanceSheet.pdf");
                ////string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);


                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}

                DateTime dt = dateTimePicker1.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");
                

                SqlCommand cmd = new SqlCommand("sp_BalanceSheet_New_1123", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@fystartdate", Convert.ToDateTime(logIn.fy_Start_Date));

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //Get Invoice Format Mapped to the Company

                    rep = new FinanceManagement.Reports.BalanceSheet_Asset_SCh();

                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


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

                    //rep = new ioneNet.FinanceManagement.Reports.rptBalanceSheet();
                    
                    //rep.SetParameterValue(1, logIn.company);
                    //rep.SetParameterValue(2, dateTimePicker1.Value);

                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    this.crystalReportViewer1.ReportSource = rep;
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
        private int numberParentRows = 5;
        private int numberChildRows = 20;
        private int numberGrandChildRows = 50;
        private int numberGrandGrandChildRows = 50;
        private DataTable GetParentTable()
        {
            DataTable dataTable = new DataTable("GroupType");
            dataTable.Columns.Add(new DataColumn("Parent_ID"));
            dataTable.Columns.Add(new DataColumn("GroupType"));
            dataTable.Columns.Add(new DataColumn("Debit"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            

            var d = (from data in db.sp_BalanceSheet(logIn.company, dateTimePicker1.Value) select data).ToList();
            if (d.Count > 0)
            {
                numberParentRows = d.Count;
                for (int i = 0; i < numberParentRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i;
                    dataRow[1] = d[i].GroupType;
                    dataRow[2] = string.Format(d[i].Debit.ToString(), i);
                    dataRow[3] = string.Format(d[i].Credit.ToString(), i);
                    
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetChildTable()
        {
            DataTable dataTable = new DataTable("AccGroups");
            dataTable.Columns.Add(new DataColumn("childID"));
            dataTable.Columns.Add(new DataColumn("GroupType"));
            dataTable.Columns.Add(new DataColumn("BalSheetHead"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            dataTable.Columns.Add(new DataColumn("Debit"));           
            dataTable.Columns.Add(new DataColumn("Parent_ID"));

            var d = (from data in db.sp_BalanceSheet_BHeads_Sum(logIn.company, dateTimePicker1.Value) select data).ToList();
            if (d.Count > 0)
            {
                numberChildRows = d.Count;
                for (int i = 0; i < numberChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i.ToString();
                    dataRow[1] = string.Format(d[i].GroupType.ToString(), i);
                    dataRow[2] = string.Format(d[i].BalSheetHead.ToString(), i);
                    dataRow[3] = string.Format(d[i].Credit.ToString(), i);
                    dataRow[4] = string.Format(d[i].Debit.ToString(), i);                   
                    dataRow[5] = (i % numberParentRows).ToString();
                    //dataRow[5] = (i % numberParentRows).ToString();
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetSubGroupTable()
        {
            DataTable dataTable = new DataTable("AccSubGroups");
            dataTable.Columns.Add(new DataColumn("G_childID"));
            dataTable.Columns.Add(new DataColumn("BalSheetHead"));
            dataTable.Columns.Add(new DataColumn("SubGroup"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            dataTable.Columns.Add(new DataColumn("Debit"));
            dataTable.Columns.Add(new DataColumn("childID"));

            var d = (from data in db.sp_BalanceSheet_SubGroups_Sum(logIn.company, dateTimePicker1.Value) select data).ToList();
            if (d.Count > 0)
            {
                numberGrandChildRows = d.Count;
                for (int i = 0; i < numberGrandChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i.ToString();
                    dataRow[1] = string.Format(d[i].BalSheetHead.ToString(), i);
                    dataRow[2] = string.Format(d[i].SubGroup.ToString(), i);
                    dataRow[3] = string.Format(d[i].Credit.ToString(), i);
                    dataRow[4] = string.Format(d[i].Debit.ToString(), i);
                    dataRow[5] = (i % numberChildRows).ToString();
                    //dataRow[5] = (i % numberParentRows).ToString();
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetAccountTable()
        {
            DataTable dataTable = new DataTable("Accounts");
            dataTable.Columns.Add(new DataColumn("G_GchildID"));
            dataTable.Columns.Add(new DataColumn("SubGroup"));
            dataTable.Columns.Add(new DataColumn("AccName"));
            dataTable.Columns.Add(new DataColumn("Credit"));
            dataTable.Columns.Add(new DataColumn("Debit"));
            dataTable.Columns.Add(new DataColumn("G_childID"));

            var d = (from data in db.sp_BalanceSheet_Account_Sum(logIn.company, dateTimePicker1.Value) select data).ToList();
            if (d.Count > 0)
            {
                numberGrandGrandChildRows = d.Count;
                for (int i = 0; i < numberGrandGrandChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i.ToString();
                    dataRow[1] = string.Format(d[i].SubGroup.ToString(), i);
                    dataRow[2] = string.Format(d[i].AccName.ToString(), i);
                    dataRow[3] = string.Format(d[i].Credit.ToString(), i);
                    dataRow[4] = string.Format(d[i].Debit.ToString(), i);
                    dataRow[5] = (i % numberGrandChildRows).ToString();
                    //dataRow[5] = (i % numberParentRows).ToString();
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }
    }
}
