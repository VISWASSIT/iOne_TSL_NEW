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
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.MaterialManagement.Reports
{
   
    public partial class MRP_Stock_Hold_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public MRP_Stock_Hold_Report()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = null;
            var d = (from data in db.SP_MRP_Stock_Holding_Report(logIn.company,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["MRPNo"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["MRPNo"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["MRPNo"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
            }
            
            //this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;

           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaterialIssueRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dtpToDate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Material Indent Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Indent_Report.xlsx");
            string doc = Fname + "\\Indent_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void preCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["MRPNo"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Prod_Code"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                        string Prod_Code = cellVaue1.ToString();
                        string MRPNo = cellVaue.ToString();

                        SqlCommand cmd1 = new SqlCommand("update  [MRP]  set status = @status where MRPNo =@MRPNO and PartNo = @ProdID and Company_ID = @compname ", con);
                        cmd1.Parameters.AddWithValue("@status", "26");
                        cmd1.Parameters.AddWithValue("@ProdID", Prod_Code);
                        cmd1.Parameters.AddWithValue("@MRPNO", MRPNo);
                        cmd1.Parameters.AddWithValue("@compname", logIn.company);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();


                        //var ci = db.MRPs.Where(w => w.MRPNo == MRPNo && w.Company_ID == logIn.company && w.PartNo == Prod_Code).FirstOrDefault();
                        //{
                        //    ci.Status = 26;
                        //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                        //    db.SubmitChanges();
                        //}                               
                               
                          
                    }

                    //}
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
