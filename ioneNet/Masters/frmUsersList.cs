using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using DAL;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class frmUsersList : Form
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string comp_name, var;
        public static int UserID;
        int Creation_Company = logIn.company;
        public frmUsersList()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

        }

        public void BindUserRole()
        {
            try

            {

                var d = (from data in db.ShowUserList(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
               
            }
            catch (Exception ex)
            {
            }
        }

        private void frmOrders_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
           
            BindUserRole();
        }

        int Parameter;
        
       

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindUserRole();
            //txtSearch.Clear();
            //cmbSearch.SelectedIndex = -1;
            //cmbSearch.Focus();
        }

        

       

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {

                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["User_ID"].MappingName;

                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    UserID = Convert.ToInt32(cellVaue.ToString());
                    var = "0";
                    ioneNet.Masters.frmUserCreation frm = new ioneNet.Masters.frmUserCreation();
                    frm.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Please Select Any One User");
                }


                //int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{

                //    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

                //    //var mappingName = sfDataGrid1.Columns["Alternative_Code"].MappingName;
                //    //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //    var mappingName = sfDataGrid1.Columns["User_ID"].MappingName;
                //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //    UserID = Convert.ToInt32(cellVaue.ToString());
                //    var = "0";
                //    ioneNet.Masters.frmUserCreation frm = new ioneNet.Masters.frmUserCreation();
                //    frm.ShowDialog();

                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One User");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmUserRoleList_Activated(object sender, EventArgs e)
        {
            BindUserRole();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Masters.frmUserCreation frm = new Masters.frmUserCreation();
            //frm.MdiParent = this.MdiParent;

            frm.ShowDialog();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deActivateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void frmUserRoleList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                    btnAddNew_Click(sender, e);                                

                if (e.Control && e.KeyCode == Keys.R)
                    btnReset_Click(sender, e);


                if (e.Control && e.KeyCode == Keys.Z)
                    btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    
    }
}
