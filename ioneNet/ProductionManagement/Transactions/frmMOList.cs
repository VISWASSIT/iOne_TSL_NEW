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
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmMOList : Form
    {
        public static string MO_No, var, inv_No1;

        

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static Boolean editMode;
        public frmMOList()
        {
            InitializeComponent();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("MO_NO").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns[6].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Manufacturing Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        MO_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Transactions.frmMO_Create frm = new frmMO_Create();
                        //OrderManagement.Transactions.
                        //frm.MdiParent = this.MdiParent;
                        frm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Manufacturing Order");
                        return;
                    }
                }                        
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowMOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();

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
                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Green);
                    }
                    if (cellVaue.ToString() == "Process Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Red);
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

        private void frmMOList_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            BindOrderslist();
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
                        //int ProdID;
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["MO_NO"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var sa = (from s in db.BOM_Projects
                                  
                                  where s.MO_No == cellVaue
                                  select new {s.MO_No }).ToList();
                        //var sa = (from a in db.Products where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) select new { a.Prod_Primary_UOM_Id, a.Prod_Group_Id,a.Prod_Unit_Wt }).ToList();
                        if (sa.Count > 0)
                        {
                            MessageBox.Show("MO Cannot Be Deleted As It is Already Processed");
                            return;
                        }
                        else
                        {
                            // ProdID = Convert.ToInt32(cellVaue.ToString());
                            SqlCommand cmd1 = new SqlCommand("delete  from [Engg_Mfg_Order] where MO_NO =@ProdID", con);
                            cmd1.Parameters.AddWithValue("@ProdID", cellVaue);

                            if (con.State != ConnectionState.Open)
                                con.Open();
                            //con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("MO Deleted Successfully");
                            BindOrderslist();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please Select MO No to Delete");
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

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            ProductionManagement.Transactions.frmMO_Create frm = new frmMO_Create();
            //frm.MdiParent = this.MdiParent;
            frm.ShowDialog();
            //BindOrderslist();
        }
        private void sfButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
