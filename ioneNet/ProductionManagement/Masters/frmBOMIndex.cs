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
using Syncfusion.Windows.Forms.Grid.Grouping;
using Syncfusion.GroupingGridExcelConverter;
using Syncfusion.Grouping;
using Syncfusion.GridExcelConverter;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Masters
{
    public partial class frmBOMIndex : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmBOMIndex()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Modules = GetParentTable();
                DataTable ProjectCodes = GetChildTable();
                //DataTable grandChildTable = GetGrandChildTable();
                GridRelationDescriptor parentToChildRelationDescriptor = new GridRelationDescriptor();
                //Same as SourceListSetEntry.Name for Child Table.
                parentToChildRelationDescriptor.ChildTableName = "Project Codes";
                parentToChildRelationDescriptor.RelationKind = RelationKind.RelatedMasterDetails;
                parentToChildRelationDescriptor.RelationKeys.Add("Project_ID", "Project_ID");

                //Adds relation to Parent Table.
                gridGroupingControl1.TableDescriptor.Relations.Add(parentToChildRelationDescriptor);
                GridRelationDescriptor childToGrandChildRelationDescriptor = new GridRelationDescriptor();
                //this.gridGroupingControl1.TableSummaryRows.Clear();
                this.gridGroupingControl1.Engine.SourceListSet.Add("Modules", Modules);
                this.gridGroupingControl1.Engine.SourceListSet.Add("Project Codes", ProjectCodes);
                //var d = (from data in db.Forge_Get_Mtrl_Availability_OrderQty(logIn.company) select data).ToList();
                //if (d.Count > 0)
                //{
                //dgProductsList.DataSource = d;
                gridGroupingControl1.DataSource = Modules;
                //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

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
            DataTable dataTable = new DataTable("ProjectCodes");
            dataTable.Columns.Add(new DataColumn("Parent_ID"));
            dataTable.Columns.Add(new DataColumn("Project_ID"));
            dataTable.Columns.Add(new DataColumn("Project_Code"));
            

            var d = (from data in db.Project_code_Masters where data.Company_ID== logIn.company select data).ToList();
            if (d.Count > 0)
            {
                numberParentRows = d.Count;
                for (int i = 0; i < numberParentRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i;
                    dataRow[1] = d[i].id;
                    dataRow[2] = string.Format(d[i].Project_Code.ToString(), i);
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private DataTable GetChildTable()
        {
            DataTable dataTable = new DataTable("Modules");
            dataTable.Columns.Add(new DataColumn("childID"));
            dataTable.Columns.Add(new DataColumn("Project_ID"));
            dataTable.Columns.Add(new DataColumn("Module_ID"));
            dataTable.Columns.Add(new DataColumn("Module_Name"));
            dataTable.Columns.Add(new DataColumn("Parent_ID"));

            var d = (from data in db.Project_Module_Masters where data.Company_ID == logIn.company select data).ToList();
            if (d.Count > 0)
            {
                numberChildRows = d.Count;
                for (int i = 0; i < numberChildRows; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = i.ToString();
                    dataRow[1] = string.Format(d[i].Project_ID.ToString(), i);
                    dataRow[2] = string.Format(d[i].ID.ToString(), i);
                    dataRow[3] = string.Format(d[i].Module_Name.ToString(), i);
                    dataRow[4] = (i % numberParentRows).ToString();
                    //dataRow[5] = (i % numberParentRows).ToString();
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

    }
}
