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
using System.Data.OleDb;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;

namespace ioneNet.ProductionManagement
{
    public partial class ProjectCodesList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        public static Boolean editMode;
        private void sfButton1_Click(object sender, EventArgs e)
        {
            BindProjects();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["id"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //if (cellVaue.ToString() == "Approved")
                    //{
                    SO_No = cellVaue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Masters.NewProjectCode frm = new ProductionManagement.Masters.NewProjectCode();
                         //OrderManagement.Transactions.
                        //frm.MdiParent = this.MdiParent;
                        frm.ShowDialog();
                        //FrmInv.ShowDialog();
                        //i1 = 0;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("The Order Cannot Be Modified Either Closed or Despatches Started");
                    //    //i1 = 0;
                    //}
                }
                else
                {
                    MessageBox.Show("No Project is Selected to Modify");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID;
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["id"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                        ProdID = Convert.ToInt32(cellVaue.ToString());
                        SqlCommand cmd1 = new SqlCommand("delete  from [Project_code_Master] where id =@ProdID", con);
                        cmd1.Parameters.AddWithValue("@ProdID", ProdID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Product Deleted Successfully");
                        BindProjects();
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Atleast One Project to Delete");
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    MessageBox.Show("The Master Record Already in Use, Cannot Be Deleted");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public ProjectCodesList()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ProductionManagement.Masters.NewProjectCode frm = new ProductionManagement.Masters.NewProjectCode();
            //frm.MdiParent = this.MdiParent;
            //var = "1";
            frm.ShowDialog();
        }

        private void ProjectCodes_Load(object sender, EventArgs e)
        {
            BindProjects();
        }
        public void BindProjects()
        {
            try
            {
                var d = (from data in db.Project_List(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Project_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Project_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Project_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Project_Code"].FilterRowCondition = FilterRowCondition.Contains;

                   
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Project_Type"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Project_Type"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Project_Type"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Project_Type"].FilterRowCondition = FilterRowCondition.Contains;
                }
                //this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                //string cellValue;
                //for (int i = 1; i < sfDataGrid1.RowCount; i++)
                //{
                //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //    var mappingName = sfDataGrid1.Columns[9].MappingName;
                //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //    if (cellVaue.ToString() == "Approved")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Green);
                //    }
                //    if (cellVaue.ToString() == "Despatches Started")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.LightSkyBlue);
                //    }
                //    if (cellVaue.ToString() == "Closed")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.SaddleBrown);
                //    }
                //    if (cellVaue.ToString() == "Pre-Closed")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Red);
                //    }
                //}
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
    }
}
