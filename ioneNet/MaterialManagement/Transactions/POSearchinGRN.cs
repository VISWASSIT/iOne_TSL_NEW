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
namespace ioneNet.MaterialManagement
{
   
public partial class POSearchinGRN : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();


        public POSearchinGRN()
        {
            InitializeComponent();
        }

        private void ProdSearch_Load(object sender, EventArgs e)
        {
            try
            {
               
                dtgetproducts.Clear();
                dtgetfinalprducts.Clear();
                if (ioneNet.Masters.ProdSearch.frmName == "GRN")
                {
                    frmName = "GRN";
                }
                //string pcode = ioneNet.MaterialManagement.Transactions.GoodsReceiptNote.Suppname;

                //var d = (from data in db.SP_GetOrders_Sel(logIn.company, pcode,1,"") select data).ToList();

                //if (d.Count > 0)
                //{
                //    sfDataGrid1.DataSource = d;
                //}
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    txtSearch.Text = "";
                //}

            }
            catch (Exception ex)
            {

                
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
                string sName = ioneNet.MaterialManagement.Transactions.PurchaseReturns.Suppname;
                var d = (from data in db.SP_GetOrders_Sel (logIn.company, sName,0, pcode) select data).ToList();

                if (d.Count > 0)
                {
                    sfDataGrid1.DataSource = d;
                }
                else
                {
                    MessageBox.Show("Record Not Found");
                    txtSearch.Text = "";                                        
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
        DataRow drgetproducts;
        private void dgProductsList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               
                //if (dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["Prod_Code"].Value.ToString() != "")
                //{
                //    int i = dgProductsList.CurrentRow.Index;
                //    //OrderManagement.Transactions.frmNewOrder obj = new OrderManagement.Transactions.frmNewOrder();
                //    MaterialManagement.Transactions.GoodsReceiptNote objpo = new MaterialManagement.Transactions.GoodsReceiptNote();

                //    dtgetproducts.Columns.Clear();
                //    dtgetproducts.Rows.Clear();
                //    dtgetproducts.Columns.Add("Prod_Code",typeof);
                //    dtgetproducts.Columns.Add("PO_NO", typeof);
                //    dtgetproducts.Columns.Add("PO_Date", typeof);
                //    drgetproducts = dtgetproducts.NewRow();
                //    drgetproducts["Prod_Code"] = dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["Prod_Code"].Value.ToString();
                //    drgetproducts["PO_NO"] = dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["PO_NO"].Value.ToString();
                //    drgetproducts["PO_Date"] = dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["PO_Date"].Value.ToString();
                //    dtgetproducts.Rows.Add(drgetproducts);
                //    dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                //    dgProductsList.Rows[i].DefaultCellStyle.BackColor = Color.BurlyWood;
                    


                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One Custome");

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (frmName == "GRN")
            {
                this.Close();                
                //MaterialManagement.GoodsReceiptNote obj = new MaterialManagement.GoodsReceiptNote();
                //obj.Show();
            }

        }

        private void dgProductsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["Prod_Code"].Value.ToString() != "")
            //{

            //    for (int k = 0; k < dtgetfinalprducts.Rows.Count; k++)
            //    {
            //        DataRow recRow = dtgetfinalprducts.Rows[k];
            //        if (Convert.ToString(dtgetfinalprducts.Rows[k]["Prod_Code"]) == dgProductsList.Rows[dgProductsList.CurrentRow.Index].Cells["Prod_Code"].Value.ToString())
            //        {
            //            recRow[0] = string.Empty;
            //            recRow.Delete();
            //            dtgetfinalprducts.AcceptChanges();
            //        }
            //    }
            //    int i = dgProductsList.CurrentRow.Index;

            //    dgProductsList.Rows[i].DefaultCellStyle.BackColor = Color.White;
            //}
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            //   if (frmName == "POrder")
            // {

            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("Item_Code", typeof(string));
            dtgetproducts.Columns.Add("Item_Description", typeof(string));
            dtgetproducts.Columns.Add("Item_Grade", typeof(string));
            dtgetproducts.Columns.Add("UOM", typeof(string));
            dtgetproducts.Columns.Add("PO_Qty", typeof(decimal));
            dtgetproducts.Columns.Add("ReceivedQty", typeof(decimal));
            dtgetproducts.Columns.Add("RejectedQty", typeof(decimal));
            dtgetproducts.Columns.Add("AcceptedQty", typeof(decimal));
            dtgetproducts.Columns.Add("Basic_Price", typeof(decimal));
            dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(decimal));
            dtgetproducts.Columns.Add("Disc_Per", typeof(decimal));
            dtgetproducts.Columns.Add("Disc_Amt", typeof(decimal));
            dtgetproducts.Columns.Add("Taxable_Value", typeof(decimal));
            dtgetproducts.Columns.Add("CGST_Per", typeof(decimal));
            dtgetproducts.Columns.Add("CGST_Amt", typeof(decimal));
            dtgetproducts.Columns.Add("SGST_Per", typeof(decimal));
            dtgetproducts.Columns.Add("SGST_Amt", typeof(decimal));
            dtgetproducts.Columns.Add("IGST_Per", typeof(decimal));
            dtgetproducts.Columns.Add("IGST_Amt", typeof(decimal));
            dtgetproducts.Columns.Add("Total_Amount", typeof(decimal));
            dtgetproducts.Columns.Add("PO_No", typeof(string));
            dtgetproducts.Columns.Add("PR_No", typeof(string));
            dtgetproducts.Columns.Add("Heat_No", typeof(string));
            dtgetproducts.Columns.Add("TCNo", typeof(string));
            dtgetproducts.Columns.Add("Remarks", typeof(string));
            dtgetfinalprducts.Rows.Clear();
            //listBox.Items.Clear();
            // Get the selected items of SfDataGrid
            //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
            //var row = this.sfDataGrid1.SelectedItem;

            string cellValue;
            for (int i = 1; i < sfDataGrid1.RowCount; i++)
            {
                foreach (var item in sfDataGrid1.SelectedItems)
                {

                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                    var SONoCol = sfDataGrid1.Columns[0].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                    if (rowData == item)
                    {
                        var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
                        var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());
                        var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
                        var PO_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
                        var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
                        var SO_Ref_No = (rowData.GetType().GetProperty("PO_No").GetValue(rowData, null).ToString());


                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["Item_Code"] = Item_Code.ToString();
                        drgetproducts["Item_Description"] = Item_Description.ToString();
                        drgetproducts["Item_Grade"] = "";
                        drgetproducts["UOM"] = UOM.ToString();
                        drgetproducts["PO_Qty"]= PO_Qty.ToString();
                        drgetproducts["ReceivedQty"] =0;
                        drgetproducts["RejectedQty"] = 0;
                        drgetproducts["AcceptedQty"] = 0;                       
                        drgetproducts["Basic_Price"] = Basic_Price.ToString();
                        drgetproducts["Disc_Per"] = 0;
                        drgetproducts["Disc_Amt"] = 0;
                        drgetproducts["Taxable_Value"] = 0;
                        drgetproducts["CGST_Per"] = 0;
                        drgetproducts["CGST_Amt"] = 0;
                        drgetproducts["SGST_Per"] = 0;
                        drgetproducts["SGST_Amt"] = 0;
                        drgetproducts["IGST_Per"] = 0;
                        drgetproducts["IGST_Amt"] = 0;
                        drgetproducts["Total_Amount"] = 0;
                        drgetproducts["PO_No"] = SO_Ref_No.ToString();
                        drgetproducts["PR_No"] = "";
                        drgetproducts["Heat_No"] = "";
                        drgetproducts["TCNo"] = "";
                        drgetproducts["Remarks"] = "";
                        
                        dtgetproducts.Rows.Add(drgetproducts);
                        dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                        //}
                        //}
                        dtgetproducts.Rows.Clear();
                    }
                }
            }
            this.Close();
        }
    }
}
