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
namespace ioneNet.Masters
{
   
public partial class ProdSearch : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        System.Data.DataRow drgetproducts;
        public ProdSearch()
        {
            InitializeComponent();
        }

        private void ProdSearch_Load(object sender, EventArgs e)
        {
            //dtgetproducts.Columns.Clear(); 
            //dtgetproducts.Rows.Clear();
            //dtgetfinalprducts.Clear();

            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("prod_id", typeof(string));
            dtgetproducts.Columns.Add("prod_code", typeof(string));
            dtgetfinalprducts.Rows.Clear();
            if (ioneNet.Masters.ProdSearch.frmName == "POrder")
            {
                frmName = "POrder";
            }
            if (ioneNet.Masters.ProdSearch.frmName == "SOrder")
            {
                frmName = "SOrder";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string pcode = "%" + txtSearch.Text + "%";
                if (logIn.company == 1044) //
                {
                    var d = (from data in db.SearchProducts(logIn.BU_ID, 0, pcode) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                        txtSearch.Text = "";
                    }
                }
                else
                {
                    var d = (from data in db.SearchProducts(logIn.company, 1, pcode) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                        txtSearch.Text = "";
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



       public void CHengeColurGird()
        {
            //for(int i = 0; i < dgProductsList.Rows.Count; i++)
            //{
                
            //}
        }
        
        private void dgProductsList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               
                //if (dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["prod_id"].Value.ToString() != "")
                //{
                //    int i = dgProductsList.CurrentRow.Index;
                //    //OrderManagement.Transactions.frmNewOrder obj = new OrderManagement.Transactions.frmNewOrder();
                //    MaterialManagement.PurchaseOrder objpo = new MaterialManagement.PurchaseOrder();

                //    dtgetproducts.Columns.Clear();
                //    dtgetproducts.Rows.Clear();
                //    dtgetproducts.Columns.Add("prod_id", typeof);
                //    drgetproducts = dtgetproducts.NewRow();
                //    drgetproducts["prod_id"] = dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["prod_id"].Value.ToString();
                //    dtgetproducts.Rows.Add(drgetproducts);
                //    dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                //    dgProductsList.Rows[i].DefaultCellStyle.BackColor = Color.BurlyWood;
                //    dgProductsList.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);


                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One Product");

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //   if (frmName == "POrder")
            // {


            //listBox.Items.Clear();
            // Get the selected items of SfDataGrid
            //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
            //var row = this.sfDataGrid1.SelectedItem;

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
                    var mappingName = sfDataGrid1.Columns[0].MappingName;
                    var mappingName1 = sfDataGrid1.Columns[1].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["prod_id"] = cellVaue.ToString();
                        drgetproducts["prod_code"] = cellVaue1.ToString();
                        dtgetproducts.Rows.Add(drgetproducts);
                        dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                        //}
                        //}
                    }
                }
            }
            label2.Text = "Total Products Selected(" + dtgetproducts.Rows.Count +")";

            //for (int i = 0; i < sfDataGrid1.SelectedItems.Count; i++)
            //{

            //    int rowIndex = i;
            //    int columnIndex = 0;
            //        var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
            //        var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
            //        if (recordIndex < 0)
            //            return;
            //        if (sfDataGrid1.View.TopLevelGroup != null)
            //        {
            //            var record = sfDataGrid1.View.TopLevelGroup.DisplayElements[recordIndex];
            //            if (!record.IsRecords)
            //                return;
            //            var data = (record as RecordEntry).Data;
            //            var cellVaue = (data.GetType().GetProperty(mappingName).GetValue(data, null).ToString());
            //        }
            //        else
            //        {
            //            //sfDataGrid1.SelectedItems.
            //            var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
            //            var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());

            //            drgetproducts = dtgetproducts.NewRow();
            //            drgetproducts["prod_id"] = cellVaue.ToString();
            //            dtgetproducts.Rows.Add(drgetproducts);
            //            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
            //            //dgProductsList.Rows[i].DefaultCellStyle.BackColor = Color.BurlyWood;
            //            //dgProductsList.Rows[i].DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);


            //            //    //MessageBox.Show(cellVaue);
            //            //}
            //            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            //            //var selectedItem = sfDataGrid1.SelectedItems[0];
            //            //var dataRow = (selectedItem as DataRowView).Row;
            //            //var cellValue = dataRow["ID"].ToString();
            //        }

            //}

            //for (int i = 0; i < sfDataGrid1.SelectedItems.Count; i++) ;
            //{


            //    var selectedItem = sfDataGrid1.SelectedItems[0];
            //    var dataRow = (selectedItem as DataRowView).Row;
            //    //var cellValue = dataRow["Prod_Name"].ToString();
            //    DataRow recRow = (selectedItem as DataRowView).Row;
            //    //{

            //    //}
            //}
            //MaterialManagement.PurchaseOrder obj = new ioneNet.MaterialManagement.PurchaseOrder();
            //obj.Show();

            //}
            // this.Close();
        }

        private void dgProductsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgProductsList_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        //    if (dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["prod_id"].Value.ToString() != "")
        //    {

        //        for (int k = 0; k < dtgetfinalprducts.Rows.Count; k++)
        //        {
        //            DataRow recRow = dtgetfinalprducts.Rows[k];
        //            if (Convert.ToString(dtgetfinalprducts.Rows[k]["prod_id"]) == dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["prod_id"].Value.ToString())
        //            {
        //                recRow[0] = string.Empty;
        //                recRow.Delete();
        //                dtgetfinalprducts.AcceptChanges();
        //            }
        //        }
        //        int i = dgProductsList.CurrentRow.Index;

        //        dgProductsList.Rows[i].DefaultCellStyle.BackColor = Color.White;
        //    }
        }

        private void dgProductsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}
