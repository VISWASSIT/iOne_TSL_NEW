using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Styles;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class MODialog : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;

        public MODialog()
        {
            InitializeComponent();
        }

        private void MODialog_Load(object sender, EventArgs e)
        {
            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("MO_No", typeof(string));
            dtgetproducts.Columns.Add("MO_Sno", typeof(string));
            dtgetproducts.Columns.Add("stage", typeof(string));
            dtgetfinalprducts.Rows.Clear();
            var d = (from data in db.SP_Forge_Pending_For_Planning(logIn.company, frmForge_ProductionPlanning.FPress, frmForge_ProductionPlanning.MGroup) select data).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["MO_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["MO_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["MO_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["MO_No"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["stage"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["stage"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["stage"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["stage"].FilterRowCondition = FilterRowCondition.Contains;

            }
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < sfDataGrid1.RowCount; i++)
            {
                foreach (var item in sfDataGrid1.SelectedItems)
                {

                    //foreach (var col in sfDataGrid1.Columns)
                    //{
                    //if (col.MappingName == "Alternative_Code")
                    //{
                    //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                    //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["MO_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["MO_Sno"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["stage"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        var cellVaue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["MO_No"] = cellVaue.ToString();
                        drgetproducts["MO_Sno"] = cellVaue1.ToString();
                        drgetproducts["stage"] = cellVaue2.ToString();
                        dtgetproducts.Rows.Add(drgetproducts);
                        dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                        //}
                        //}
                    }
                }
            }

            this.Close();
        }
    }
}
