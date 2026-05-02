using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class MaterialReceiptNoteList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
      
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode;

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    string cellValue;
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
                            var mappingName = sfDataGrid1.Columns[0].MappingName;
                            var mappingName1 = sfDataGrid1.Columns[8].MappingName;
                            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                            //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                            if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                                var cellStatus = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                                //if (cellStatus.ToString() != "Despatches Started")
                                //{
                                    SqlCommand cmd = new SqlCommand();

                                    SO_No = cellVaue.ToString();
                                    cmd.CommandText = "Update GoodsReceiptNote_Master set isdeleted = '1' where Grn_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    cmd.Parameters.Clear();
                                    string strT = logIn.username + "-" + DateTime.Now;
                                    cmd.CommandText = "Update GoodsReceiptNote_Master set Modified_By = @strT where Grn_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@strT", strT);
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                //}
                                //else
                                //{
                                //    MessageBox.Show("Selected GRN Cannot Be Deleted As Already Despatches Started, Pre-Close the order insted");
                                //}
                            }
                        }
                    }
                    MessageBox.Show("Selected GRN(s) Are Deleted Successfully");
                    BindOrderslist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewSupplyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialManagement.Reports.MRNReport frm = new MaterialManagement.Reports.MRNReport();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        public MaterialReceiptNoteList()
        {
            InitializeComponent();
        }

        private void GoodsReceiptNoteList_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.SP_ShowMRNlist(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                    }
                    if (cellVaue.ToString() == "Despatches Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                    }
                }
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    //txtSearch.Text = "";
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "1";           
            MaterialManagement.Transactions.MaterialReceiptNote frm = new MaterialManagement.Transactions.MaterialReceiptNote();
            frm.MdiParent = this.MdiParent;
            frm.Show();
          
                
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    SO_No = cellVaue;
                    var = "0";
                    editMode = true;
                    MaterialManagement.Transactions.MaterialReceiptNote frm = new MaterialManagement.Transactions.MaterialReceiptNote();
                    //OrderManagement.Transactions.
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    //FrmInv.ShowDialog();
                    //i1 = 0;
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
