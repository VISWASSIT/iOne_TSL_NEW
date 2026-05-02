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
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Enums;
using System.IO;

namespace ioneNet.ProductionManagement.Transactions
{

    public partial class frmForge_MO_List : Form
    {
        public static string MO_No, var, inv_No1;



        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static Boolean editMode;
        public frmForge_MO_List()
        {
            InitializeComponent();
        }

        private void frmForge_MO_List_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowForge_MOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
               
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Reviewed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.Green);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.Red);
                    }
                }
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["MO_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["MO_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["MO_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["MO_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Contains;
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

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            ProductionManagement.Transactions.frmManufacturingOrder frm = new frmManufacturingOrder();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            //BindOrderslist();
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
                var currentCellValue = (rowData.GetType().GetProperty("MO_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Manufacturing Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        MO_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Transactions.frmManufacturingOrder frm = new frmManufacturingOrder();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                        //frm.ShowDialog();

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

        private void sfButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                

                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                //var currentCellValue = (rowData.GetType().GetProperty("MO_NO").GetValue(rowData, null).ToString());
               
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if ((from u in db.Forge_ProdPlannings where u.Mo_No == cellVaue && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    MessageBox.Show("Planning Already Initiated Againist This MO, Cannot Be Deleted");
                    return;
                }
                else
                {
                    
                    db.sp_MO_Delete(cellVaue, logIn.company);
                    BindOrderslist();
                }
            }
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "MO List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\MOList.xlsx");
            string doc = Fname + "\\MoList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void viewSupplyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductionManagement.Reports.MoStatusReport frm = new ProductionManagement.Reports.MoStatusReport();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void mAPQAPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("MO_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                MO_No = cellVaue.ToString();
                ProductionManagement.Transactions.frmMapQAPtoMO frm = new ProductionManagement.Transactions.frmMapQAPtoMO();
               // frm.MdiParent = this.MdiParent;
                frm.ShowDialog();
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
    }
}
