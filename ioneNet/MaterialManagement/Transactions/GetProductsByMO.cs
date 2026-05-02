using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GetProductsByMO : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;
        public GetProductsByMO()
        {
            InitializeComponent();
        }

        private void GetProductsByMO_Load(object sender, EventArgs e)
        {
            try
            {

                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("prod_id", typeof(string));
                dtgetproducts.Columns.Add("MRPNo", typeof(string));
                dtgetproducts.Columns.Add("Indent_Qty", typeof(string));
                dtgetproducts.Columns.Add("Stock_Qty", typeof(string));
                dtgetproducts.Columns.Add("Price", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                DateTime t = DateTime.Now;
                string t1 = t.ToString("dd/MMM/yyyy");
                textBox1.Text = frmMaterialIssues.DocNo;
                var d = (from data in db.ProdutsToIssueByMO(logIn.company, textBox1.Text, t,logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["MRPNo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["MRPNo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["MRPNo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["MRPNo"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Int_Prod_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Int_Prod_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Int_Prod_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Int_Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;
                }
                else
                {
                    MessageBox.Show("Record Not Found");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            
            for (int i = 1; i < sfDataGrid1.RowCount; i++)
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
                    var mappingName = sfDataGrid1.Columns[1].MappingName;
                    var mappingName1 = sfDataGrid1.Columns[0].MappingName; //MRP
                    var mappingName2 = sfDataGrid1.Columns[5].MappingName;//Ind Qty
                    var mappingName3 = sfDataGrid1.Columns[6].MappingName; //StockQty
                    var mappingName4 = sfDataGrid1.Columns[7].MappingName; //Price
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var MRPNo = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        var Indent_Qty = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                        var Stock_Qty = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
                        var Price = (rowData.GetType().GetProperty(mappingName4).GetValue(rowData, null));
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["prod_id"] = cellVaue.ToString();
                        drgetproducts["MRPNo"] = MRPNo.ToString();
                        drgetproducts["Indent_Qty"] = Indent_Qty.ToString();
                        drgetproducts["Stock_Qty"] = Stock_Qty.ToString();
                        drgetproducts["Price"] = Price;                                               
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
