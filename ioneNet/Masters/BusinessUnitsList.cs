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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class BusinessUnitsList : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string comp_id, comp_name;
        public BusinessUnitsList()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            comp_id = "";
            comp_name = "";

            Masters.BusinessUnitInformation frm = new Masters.BusinessUnitInformation();
            //frm.MdiParent = this.MdiParent;

            frm.ShowDialog();
        }

        private void CompanyList_Load(object sender, EventArgs e)
        {
            try

            {

                var d = (from data in db.ShowBusinessUnitList(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    agvCompList.DataSource = d;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void deActivateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //int i = agvCompList.SelectedIndex;
                int i = agvCompList.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID;
                        var rowData = agvCompList.GetRecordAtRowIndex(i);
                        var mappingName = agvCompList.Columns["id"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                        ProdID = Convert.ToInt32(cellVaue.ToString());
                        SqlCommand cmd1 = new SqlCommand("delete  from [Costing_Units] where id =@ProdID", con);
                        cmd1.Parameters.AddWithValue("@ProdID", ProdID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Business Unit Deleted Successfully");

                    }

                }
                else
                {
                    MessageBox.Show("Please Select Atleast One Business Unit to Delete");
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

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {

                
                    int i = agvCompList.CurrentCell.RowIndex;

                    if (i >= 0)
                    {

                        var currentCellValue = agvCompList.CurrentCell.CellRenderer.GetControlValue();
                        var rowData = agvCompList.GetRecordAtRowIndex(i);
                        var mappingName = agvCompList.Columns["id"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        //var currentCellV00alue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                        comp_id = cellVaue.ToString(); 
                    //    comp_name = agvCompList.Rows[agvCompList.CurrentRow.Index].Cells[1].Value.ToString();
                        Masters.BusinessUnitInformation frm = new Masters.BusinessUnitInformation();
                        //frm.MdiParent = this.MdiParent;

                        frm.ShowDialog();


                        //productCode = 
                        //var = "0";
                        //ioneNet.MaterialManagement.Masters.frmProductsNew frm = new ioneNet.MaterialManagement.Masters.frmProductsNew();
                        //frm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("Please Select Any One Record");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
