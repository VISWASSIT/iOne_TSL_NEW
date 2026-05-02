using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using ioneNet.Qulaity_Management.Masters;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGridConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class frmQA_InomingInspReportList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string Report_No, var;
        public static Boolean editMode;

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public frmQA_InomingInspReportList()
        {
            InitializeComponent();
        }

        private void frmQA_InomingInspReportList_Load(object sender, EventArgs e)
        {
            BindReportlist();
            var bindLoc = (from m in db.User_Roles
                           where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Incoming Inspection Report"
                           select new
                           {
                               m.Review_Role,
                               m.Approve_Role,
                               m.View_Role,
                               m.Modify_Role,
                               m.Create_Role,
                               m.Delete_Role
                           }).ToList();
            this.modifyToolStripMenuItem.Enabled = false;
            if (bindLoc[0].Modify_Role == true)
            {
                this.modifyToolStripMenuItem.Enabled = true;
            }
            this.approveToolStripMenuItem.Enabled = false;
            if (bindLoc[0].Approve_Role == true)
            {
                this.approveToolStripMenuItem.Enabled = true;
            }


            this.deleteToolStripMenuItem.Enabled = false;
            if (bindLoc[0].Delete_Role == true)
            {

                this.deleteToolStripMenuItem.Enabled = true;
            }
            btnAddNew.Enabled = false;
            if (bindLoc[0].Create_Role == true)
            {
                btnAddNew.Enabled = true;
            }
        }
        private void BindReportlist()
        {
            try
            {
                var d = (from data in db.sp_Get_QA_IncomingInspectionReportList (logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    //this.sfDataGrid1.Columns["MaterialGroup"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["MaterialGroup"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["MaterialGroup"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["MaterialGroup"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Report_Ref_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Report_Ref_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Int_Lot_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Int_Lot_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Int_Lot_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Int_Lot_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Supplier_TC_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Supplier_TC_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Supplier_TC_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Supplier_TC_No"].FilterRowCondition = FilterRowCondition.Contains;


                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            workBook.SaveAs(Fname + "\\IncomingInspReport.xlsx");
            string doc = Fname + "\\IncomingInspReport.xlsx";
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

                var mappingName = sfDataGrid1.Columns["Report_Ref_No"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
               
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Incoming Inspection Report" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        Report_No = cellVaue.ToString();
                        var = "0";
                        editMode = true;
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                        frmQA_IncomingInspectionReport frm = new frmQA_IncomingInspectionReport();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;


                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Incoiming Inspection Report");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindReportlist();
        }

        private void printQRCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            //if (i >= 0)
            //{
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["Report_Ref_No"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            Report_No = cellVaue;


            var sa = (from sq in db.Incoming_Chemical_Reports
                      join g in db.GoodsReceiptNote_Masters on sq.GRN_Master_Id equals g.Id
                      join gc in db.GoodsReceiptNote_Childs on g.Id equals gc.GRN_Master_ID
                      join su in db.Supplier_informations on g.SupplierName equals su.ID
                      join p in db.Products on gc.Prod_Code equals p.prod_ID
                      where sq.Company_Id == logIn.company && sq.Report_Ref_No == Report_No && sq.Sample_ID.Length>0
                      orderby sq.id
                      select new
                      {
                          sq.Insp_Date,
                          sq.Int_Lot_No,
                          sq.Supplier_Heat_No,
                          sq.Supplier_TC_No,
                          sq.No_Of_Pcs,
                          sq.Inspected_By,
                          sq.Approved_By,
                          sq.Created_By,
                          sq.Modified_BY,
                          sq.Visual_Inspection,
                          sq.Result,
                          sq.Comments_Remarks,
                          sq.Chemical_Readings,
                          g.Grn_NO,
                          p.Prod_Name,
                          gc.Prod_Grade,
                          su.Supplier_Name,
                          grnID = g.Id,
                          sq.Doc_Link,
                          g.Supplier_InvNo,
                          gc.Int_Batch_No,
                          sq.Accepted_Grade,
                          sq.New_Batch_No,
                          sq.Sample_ID
                      }).ToList();
            if (sa.Count > 0)
            {

                //dpInvDate.Text = sa[0].Insp_Date.Value.ToString();
                //txtProdName.Text = sa[0].Prod_Name.ToString();
                //txtMtrlGrade.Text = sa[0].Accepted_Grade;
                //txtIntLotNo.Text = sa[0].New_Batch_No;
                //txtSupplierHeat.Text = sa[0].Supplier_Heat_No;
                //txtSupplierTCno.Text = sa[0].Supplier_TC_No;

            }
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_ChemicalTest_Parameters_ForLabel", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@reportno", Report_No);

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            string pval = "";

            SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd5.Parameters.AddWithValue("@comp", logIn.company);
            cmd5.ExecuteNonQuery();


            //for (int k = 0; k < ds2.Columns.Count; k++)
            //{

            for (int j = 0; j < sa.Count; j++)
            {

                   string s = sa[j].Chemical_Readings;
                    string[] values = s.Split(',');

                    //dr[0] = sa[j].Sample_ID;
                    for (int n = 0; n < values.Length; n++)
                    {
                        values[n] = values[n].Trim();
                        if (pval == "")
                        {
                            pval = ds2.Columns[n].ColumnName +":"+values[n].Trim();
                        }
                        else
                        {
                            pval = pval + "; " + ds2.Columns[n].ColumnName + ":" + values[n].Trim();
                        }
                   
                    }
                string QRCODE = "";

                QRCODE = "Sample ID : "+ sa[j].Sample_ID +  " RM Lot No :" + sa[j].New_Batch_No + " Receipt Date : " + sa[j].Insp_Date + " Product :" + sa[j].Prod_Name + " Prod Grade:" + sa[j].Accepted_Grade + " Chem Comp:" + pval;

                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
                var result = writer.Write(QRCODE);
                var barcodeBitmap = new Bitmap(result);

                SqlCommand cmd1 = con.CreateCommand();

                cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Company_ID,QR_COde,Inv_No) VALUES  (" + logIn.company + ",@Qrcode,@Inv_No)";
                //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                Image img = barcodeBitmap;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();
                cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                cmd1.Parameters.AddWithValue("@Inv_No", sa[j].Sample_ID + " - " + sa[j].New_Batch_No);
                cmd1.ExecuteNonQuery();

                pval = "";


            }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            //rep = new ioneNet.Qulaity_Management.Transactions.QRCodePrint_RM();
            rep = new ioneNet.Reports.rptPrintBarCode();
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), "QR_Label.pdf");
            //string path = @"D:\Invoice.pdf";
            FileInfo fi1 = new FileInfo(path);


            //SqlCommand cmd2 = new SqlCommand("select *  from [temp_inv_QRCOde] where Company_ID =@comp", con);
            //cmd2.Parameters.AddWithValue("@comp", logIn.company);

            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataTable dtr = new DataTable();
            //da2.Fill(dtr);



            //rep.SetDataSource(dtr);


            crConnectionInfo.ServerName = frmMain.ServerIP;
            crConnectionInfo.DatabaseName = frmMain.Database;
            crConnectionInfo.UserID = frmMain.DBUserID;
            crConnectionInfo.Password = frmMain.Password;

            crDatabase = rep.Database;
            crTables = crDatabase.Tables;
            //Loop through all tables in the report and apply the connection information for each table.
            for (int k = 0; k < crTables.Count; k++)
            {
                //  crTable = crTables[i];
                crTableLogOnInfo = crTables[k].LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

            }






            ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
            // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
            viewer.crystalReportViewer1.ReportSource = rep;
            viewer.crystalReportViewer1.Refresh();
            rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

            //cmd.Parameters.Clear();
            Process.Start(path);

            //string p = ds2.Rows[0][k].ToString();

            //}

            //MessageBox.Show(pval);






            //ioneNet.Qulaity_Management.Transactions.FrmPrintQRCode_Lables frm = new FrmPrintQRCode_Lables();            
            //frm.ShowDialog();
        }

        private void disposeNCPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

                var mappingName = sfDataGrid1.Columns["Int_Lot_No"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Incoming Inspection Report" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Approve_Role == true)
                    {
                        Report_No = cellVaue.ToString();
                        var = "0";
                        editMode = true;
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                        frmNonConfirmingProduct_Incoming frm = new frmNonConfirmingProduct_Incoming();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                        sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;


                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Handle Non Confirming Product");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

            editMode = false;
            ioneNet.Qulaity_Management.Transactions.frmQA_IncomingInspectionReport frm = new frmQA_IncomingInspectionReport();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

            BindReportlist();
        }
    }
}
