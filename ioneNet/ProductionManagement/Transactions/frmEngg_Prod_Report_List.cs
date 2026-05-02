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
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmEngg_Prod_Report_List : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
             
        DataClasses1DataContext db = new DataClasses1DataContext();        
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode;
        public frmEngg_Prod_Report_List()
        {
            InitializeComponent();
        }
        private void frmEngg_Prod_Report_List_Load(object sender, EventArgs e)
        {
            BindJobCardslist();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            editMode = false;
            ProductionManagement.Transactions.frmEngg_Production_Report frm = new ProductionManagement.Transactions.frmEngg_Production_Report();
           
            //var = "1";
            frm.ShowDialog();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var currentCellValue = (rowData.GetType().GetProperty("Voucher_No").GetValue(rowData, null).ToString());
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Production Report" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        SO_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Transactions.frmEngg_Production_Report frm = new frmEngg_Production_Report();
                       
                        frm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Production Report");
                        return;
                    }
                }
                else
                {
                    SO_No = currentCellValue.ToString();
                    var = "0";
                    editMode = true;
                    ProductionManagement.Transactions.frmEngg_Production_Report frm = new frmEngg_Production_Report();
                    frm.ShowDialog();
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
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    //string cellValue;
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {
                        foreach (var item in sfDataGrid1.SelectedItems)
                        {
                            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                            var mappingName = sfDataGrid1.Columns[0].MappingName;
                             if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());                            
                                string myString = cellVaue.ToString();                               
                                db.Sp_delete_Production(logIn.company, myString,logIn.BU_ID);
                            }
                        }
                    }
                    MessageBox.Show("Selected Production Report Deleted Successfully");
                    BindJobCardslist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void sfButton2_Click(object sender, EventArgs e)
        {
           
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Production Report";           
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Production_Report.xlsx");
            string doc = Fname + "\\Production_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
        private void sfButton1_Click(object sender, EventArgs e)
        {
            BindJobCardslist();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void BindJobCardslist()
        {
            try
            {
                var d = (from data in db.ShowProductionReport(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
               
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
