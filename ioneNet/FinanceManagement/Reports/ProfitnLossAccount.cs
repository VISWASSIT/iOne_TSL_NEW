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
using Ione_DAL;

namespace ioneNet.FinanceManagement.Reports
{
    public partial class ProfitnLossAccount : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        SqlConnection objSqlConnection;
        SqlCommand objSqlCommand;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public ProfitnLossAccount()
        {
            InitializeComponent();
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


                SqlCommand cmd = new SqlCommand("sp_ProfitLoss_New_Rpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@fystartdate",logIn.fy_Start_Date);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company

                rep = new FinanceManagement.Reports.ProfitandLossAccount();

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
                this.crystalReportViewer1.Refresh();
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
