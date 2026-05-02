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
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frnPRDailog : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;
        public frnPRDailog()
        {
            InitializeComponent();
        }

        private void frnPRDailog_Load(object sender, EventArgs e)
        {
            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("PR_No", typeof(string));
            dtgetproducts.Columns.Add("prod_id", typeof(string));
            dtgetproducts.Columns.Add("BalQty", typeof(string));
            dtgetfinalprducts.Rows.Clear();

            var d = (from data in db.SP_GetPR_Sel(logIn.company,logIn.BU_ID) select data).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["PR_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["PR_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["PR_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["PR_No"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Product_Description"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Product_Description"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Product_Description"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Product_Description"].FilterRowCondition = FilterRowCondition.Contains;                

            }
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
                    var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["prod_id"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["BalQty"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        var cellVaue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["PR_No"] = cellVaue.ToString();
                        drgetproducts["prod_id"] = cellVaue1.ToString();
                        drgetproducts["BalQty"] = cellVaue2.ToString();
                        dtgetproducts.Rows.Add(drgetproducts);
                        dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                        //}
                        //}
                    }
                }
            }
            label2.Text = "Total Products Selected(" + dtgetproducts.Rows.Count + ")";
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
