using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using ioneNet.OrderManagement.Transactions;
using ioneNet.Qulaity_Management.Masters;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using Syncfusion.Windows.Forms.Tools.Win32API;

namespace ioneNet.Qulaity_Management
{
    public partial class frmQA_MaterialGradesList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static string Grade_ID, var;
        public static Boolean editMode;

        public frmQA_MaterialGradesList()
        {
            InitializeComponent();
        }

        private void Productionvouchersearch_Load(object sender, EventArgs e)
        {
         BindGradeslist();
        }     

       

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int i = sfDataGrid1.CurrentCell.RowIndex;
            ////if (i >= 0)
            ////{
            //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            ////var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            //var currentCellValue = (rowData.GetType().GetProperty("Material_Grade").GetValue(rowData, null).ToString());

            //var mappingName = sfDataGrid1.Columns["Material_Grade"].MappingName;
            //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //Voucherno = cellVaue;
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

            editMode = false;
            ioneNet.Qulaity_Management.Masters.frmQA_MaterialGrades frm = new frmQA_MaterialGrades();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

            BindGradeslist();
        }

        private void BindGradeslist()
        {
            try
            {
                var d = (from data in db.SP_Forge_Get_MaterialGrades(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    //this.sfDataGrid1.Columns["MaterialGroup"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["MaterialGroup"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["MaterialGroup"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["MaterialGroup"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;


                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindGradeslist();
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
            workBook.Worksheets[0].Range["A2"].Value = "Material Grades List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\MaterialGradesList.xlsx");
            string doc = Fname + "\\MaterialGradesList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
               
                var mappingName = sfDataGrid1.Columns["id"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Grade" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        Grade_ID = cellVaue.ToString();                       
                        var = "0";
                        editMode = true;
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                        frmQA_MaterialGrades frm = new frmQA_MaterialGrades();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;


                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Material Grades");
                        return;
                    }
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
