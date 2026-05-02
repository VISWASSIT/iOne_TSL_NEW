using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
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
    public partial class GP_Dialog : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static string party;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;
        public GP_Dialog()
        {
            InitializeComponent();
        }

        private void GP_Dialog_Load(object sender, EventArgs e)
        {
            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("GP_No", typeof(string));
            dtgetproducts.Columns.Add("Prod_Code", typeof(string));
            dtgetproducts.Columns.Add("Product_Description", typeof(string));           
            dtgetproducts.Columns.Add("UOM", typeof(string));
            dtgetproducts.Columns.Add("BalQty", typeof(string));
            dtgetproducts.Columns.Add("Basic_Price", typeof(string));
            dtgetfinalprducts.Rows.Clear();
            party = GoodsReceiptNoteJW.Suppname;
            var d = (from data in db.GP_Status_Views where data.Company_ID == logIn.company && data.Supplier_Name == party  
                     select new {
                                  GP_No  = data.GateVch_No,
                                  Item_Code= data.Prod_Code,
                                  Item_Description= data.Product_Description,
                                  GP_Qty =data.BalQty,
                                  Basic_Price = data.Price,
                                  data.UOM
                              }).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["GP_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["GP_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["GP_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["GP_No"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;

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
                    var mappingName = sfDataGrid1.Columns["GP_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Item_Code"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Item_Description"].MappingName;
                    var mappingName3 = sfDataGrid1.Columns["GP_Qty"].MappingName;
                    var mappingName4 = sfDataGrid1.Columns["UOM"].MappingName;
                    var mappingName5 = sfDataGrid1.Columns["Basic_Price"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        var cellVaue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        var cellVaue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
                        var cellVaue4 = (rowData.GetType().GetProperty(mappingName4).GetValue(rowData, null).ToString());
                        var cellVaue5 = (rowData.GetType().GetProperty(mappingName5).GetValue(rowData, null).ToString());
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["GP_No"] = cellVaue.ToString();
                        drgetproducts["Prod_Code"] = cellVaue1.ToString();
                        drgetproducts["Product_Description"] = cellVaue2.ToString();
                        drgetproducts["UOM"] = cellVaue4.ToString();
                        drgetproducts["BalQty"] = cellVaue3.ToString();
                        drgetproducts["Basic_Price"] = cellVaue5.ToString();
                        dtgetproducts.Rows.Add(drgetproducts);
                        dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                      
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
