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
    public partial class GetProductsforReturn : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;
        public GetProductsforReturn()
        {
            InitializeComponent();
        }

        private void GetProductsforReturn_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = frmMaterialReturs.DocNo;
                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("prod_id", typeof(string));
                dtgetproducts.Columns.Add("MRPNo", typeof(string));
                dtgetproducts.Columns.Add("Issued_Qty", typeof(string));              
                dtgetproducts.Columns.Add("Price", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                DateTime t = DateTime.Now;
                string t1 = t.ToString("dd/MMM/yyyy");               
                var d = (from data in db.Get_Item_for_IssueReturns (logIn.company, logIn.BU_ID, textBox1.Text) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["MRP_Indent"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["MRP_Indent"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["MRP_Indent"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["MRP_Indent"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["MO_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["MO_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["MO_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["MO_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;
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
                    var mappingName = sfDataGrid1.Columns[2].MappingName;
                    var mappingName1 = sfDataGrid1.Columns[0].MappingName; //MRP
                    var mappingName2 = sfDataGrid1.Columns[4].MappingName;//Return Qty                   
                    var mappingName4 = sfDataGrid1.Columns[5].MappingName; //Price
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var MRPNo = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        var Return_Qty = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());                        
                        var Price = (rowData.GetType().GetProperty(mappingName4).GetValue(rowData, null));
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["prod_id"] = cellVaue.ToString();
                        drgetproducts["MRPNo"] = MRPNo.ToString();
                        drgetproducts["Issued_Qty"] = Return_Qty.ToString();                       
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
