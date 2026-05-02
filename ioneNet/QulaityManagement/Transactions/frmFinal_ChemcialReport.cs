using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Ione_DAL;
using System.Windows.Media.Animation;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using iTextSharp.text;
using ioneNet.QulaityManagement.Reports;

namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class frmFinal_ChemcialReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string GRN_NO, ItemCode, RFNo, MtrlGrade, reportNo;

        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private Database crDatabase;
        private string path;
        public frmFinal_ChemcialReport()
        {
            InitializeComponent();
        }

        private void frmForging_IncomingInspectionReport_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtInspectedBy.Text = logIn.username;
            txtApprovedBy.Text = logIn.username;
            btnSave.Enabled = true;
            cmdDelete.Enabled = true;
            button1.Enabled = true;
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Insp Result" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }

            AutoincrementId();

            CallChemParamters();
            //if (frmQA_InomingInspReportList.editMode == true)
            //{
            //    txtInvNo.Text = frmQA_InomingInspReportList.Report_No;
            //    BindEdit();
            //}
            if (FG_Lot_Register.ReportRef != string.Empty)
            {
                txtInvNo.Text = FG_Lot_Register.ReportRef;
                BindEdit();
                btnSave.Enabled = false;
                cmdDelete.Enabled = false;
                button1.Enabled = false;
                FG_Lot_Register.ReportRef = string.Empty;
            }

        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Final_Chemical_Report(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtInvNo.Text = result.FirstOrDefault().Report_No;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var d = (from data in db.Get_GRN_For_Inspection(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Grn_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Grn_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Grn_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Grn_NO"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Supplier_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Supplier_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Supplier_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Supplier_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    groupBox1.Visible = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void txtGRNNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtGRNNo.Text != "")
                {
                    if ((from a in db.QA_Chmical_Report_Finals
                         
                         where a.Company_Id == logIn.company && a.FG_Batch_No == txtGRNNo.Text select a).Count() > 0)
                    {
                        MessageBox.Show("Inspection Already Completed For The Selected Batch No");
                        txtGRNNo.Focus();
                    }
                    else
                    {
                        GetGRNData();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                var mappingName1 = sfDataGrid1.Columns[2].MappingName;
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                // var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
                txtInvNo.Text = cellVaue.ToString();
                txtGRNNo.Text = cellVaue1.ToString();
                BindEdit();
                
                groupBox1.Visible = false;
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ReadingData = "";
                string ParaSeq = "";
                if ((from a in db.QA_Chmical_Report_Finals where a.Company_Id == logIn.company && a.Report_Ref_No == txtInvNo.Text select a).Count() > 0)
                {
                    db.Sp_Delete_Final_Chemcial_Report(logIn.company, txtInvNo.Text);
                }

                for (int j = 1; j < dataGridView1.Columns.Count - 2; j++)
                {
                    var d1 = (from a in db.QA_Test_Parameters where a.Parameter_ShortCode == dataGridView1.Columns[j].Name select new { a.id }).ToList();
                //    SC.PR_Master_ID = d1[0].ID;
                    if (ParaSeq != "")
                    {
                        ParaSeq = ParaSeq + ',' + d1[0].id.ToString();
                    }
                    else
                    {
                        ParaSeq = d1[0].id.ToString();
                    }
                }
                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string s = dataGridView1.Rows[i].Cells["Sample_ID"].Value.ToString();
                    if (s.Length>0)
                    {

                        QA_Chmical_Report_Final pb = new QA_Chmical_Report_Final();
                        pb.Report_Ref_No = txtInvNo.Text;
                        pb.Insp_Date = dpInvDate.Value;
                        pb.RM_Lot_No = txtIntLotNo.Text;
                        
                        pb.FG_Batch_No = txtGRNNo.Text;
                        pb.FG_Batch_Qty = Convert.ToDecimal(txtBatchQty.Text);
                        pb.Inspected_By = txtInspectedBy.Text;
                        pb.Approved_By = txtApprovedBy.Text;

                        pb.Sample_ID = dataGridView1.Rows[i].Cells["Sample_ID"].Value.ToString();
                        for (int j = 1; j < dataGridView1.Columns.Count - 2; j++)
                        {
                            if (ReadingData != "")
                            {
                                ReadingData = ReadingData + ',' + Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                            }
                            else
                            {
                                ReadingData = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                            }


                            
                        }
                        pb.Chemical_Readings = ReadingData;
                        ReadingData = "";
                        pb.Chem_Param_Seq = ParaSeq;
                        pb.Accepted_Grade = dataGridView1.Rows[i].Cells["Accepted_Grade"].Value.ToString().Trim();
                        
                        if (dataGridView1.Rows[i].Cells["Accepted_Grade"].Value.ToString().Trim() != txtMtrlGrade.Text.Trim())
                        {
                            pb.Result = 1159;
                            //pb.New_Batch_No = txtIntLotNo.Text + "/1";
                        }
                        else
                        {
                            pb.Result = Convert.ToInt32(cmbStatus.SelectedValue);
                            //pb.New_Batch_No = dataGridView1.Rows[i].Cells["Batch_No"].Value.ToString(); 
                        }
                        pb.Doc_Link = (txtDocPath.Text == null) ? "" : txtDocPath.Text;

                        
                        pb.Visual_Inspection = checkBox1.Checked;
                        pb.Comments_Remarks = txtComments.Text;
                        pb.Company_Id = logIn.company;
                        pb.Created_By = lblCreatedBy.Text;
                        pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.QA_Chmical_Report_Finals.InsertOnSubmit(pb);
                    }
                }

                db.SubmitChanges();
                MessageBox.Show("Record Updated Sucessfully");



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {




                var p = (from s in db.QA_Chmical_Report_Finals
                         where s.Company_Id == logIn.company 

                         select new
                         {
                             s.Report_Ref_No,
                             s.Insp_Date,
                             s.FG_Batch_No
                         }
                        ).Distinct().ToList();

                if (p.Count >= 0)
                {
                    sfDataGrid1.DataSource = p;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Report_Ref_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Report_Ref_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowCondition = FilterRowCondition.Contains;

                   
                    this.sfDataGrid1.Columns["FG_Batch_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["FG_Batch_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["FG_Batch_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["FG_Batch_No"].FilterRowCondition = FilterRowCondition.Contains;
                }
                else
                {

                }
                groupBox1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    string file = open.FileName;
                    txtDocPath.Text = file;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtDocPath.Text;
                Process prc = new Process();
                prc.StartInfo.FileName = fileName;
                prc.Start();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                //daDiagnosis.Dispose();
                //daDiagnosis = null;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
            string columnName = dataGridView1.Columns[columnIndex].HeaderText;
            TextBox tb3 = e.Control as TextBox;
            tb3.AutoCompleteCustomSource = null;
            if (R1.Cells[0].Value == "Spec")
            {
                dataGridView1.CurrentCell.ReadOnly = true;
            }
            else
            {
                dataGridView1.CurrentCell.ReadOnly = false;

            }
            if (tb3 != null && columnName == "Accepted_Grade")
            {
                tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                addSections(DataColl);
                tb3.AutoCompleteCustomSource = DataColl;
            }
        }
        public void addSections(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;
                
                if (columnName == "Accepted_Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Accepted_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
            }
            catch
            {
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            //groupBox2.Visible = false;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
            string columnName = dataGridView1.Columns[columnIndex].Name;
            decimal StdSpec = 0;
            decimal ActReading = 0;
            if (columnName == "Accepted_Grade")
            {
                string prodgrade = R1.Cells["Accepted_Grade"].Value.ToString();
                var S = (from a in db.QA_Mtrl_Grade_Masters
                         where a.Company_ID == logIn.company && a.Material_Grade == prodgrade
                         select a).ToList();
                if (S.Count > 0)
                {

                }
                else
                {
                    MessageBox.Show("Material Grade Entered is invalid");
                    R1.Cells["Accepted_Grade"].Value = "";
                    return;
                }
            }
                if (columnIndex > 0 && columnIndex < dataGridView1.Columns.Count-2)
            {
                DataGridViewRow R2 = dataGridView1.Rows[0];
               // StdSpec = Convert.ToDecimal(dataGridView1.Rows[0].Cells[columnIndex].Value);
                ActReading = (R1.Cells[columnIndex].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells[columnIndex].Value);
                StdSpec = (R2.Cells[columnIndex].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R2.Cells[columnIndex].Value);

                if (ActReading > StdSpec)
                {
                    R1.Cells[columnIndex].Style.ForeColor = Color.Red;
                    R1.Cells["Accepted_Grade"].Value = "Off-Grade";
                }
                else
                {
                    R1.Cells[columnIndex].Style.ForeColor = Color.Black;
                    R1.Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                }

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    
                    if(R1.Cells[i].Style.ForeColor == Color.Red)
                    {
                        R1.Cells["Accepted_Grade"].Value = "Off-Grade";
                        R1.Cells["Batch_No"].Value = "";
                        return;
                    }
                   else
                    {
                        R1.Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                        R1.Cells["Batch_No"].Value = txtIntLotNo.Text;
                    }

                }
            }

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
        private void copyAlltoClipboard()
        {
            dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridView1.MultiSelect = true;
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string filename = "";
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = @"c:\";
            //  fdlg.FileName = txtChooseFile.Text;
            fdlg.Filter = "Excel Sheet(*.xlsx)|*.xls|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                filename = fdlg.FileName;
                //Application.DoEvents();
            }


            Cursor.Current = Cursors.WaitCursor;
            System.Data.OleDb.OleDbConnection MyConnection;
            System.Data.DataTable DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;

            string SheetName = "Sheet1";
            // string ExcellSheet = ;

            string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
            MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'");

            MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + SheetName + "$] ", MyConnection);
            //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
            MyCommand.TableMappings.Add("Table", filename);
            DtSet = new System.Data.DataTable();

            MyCommand.Fill(DtSet);
            dataGridView1.DataSource = DtSet;


            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string prodgrade = (dataGridView1.Rows[i].Cells["Accepted_Grade"].Value.ToString());
                var S = (from a in db.QA_Mtrl_Grade_Masters
                         where a.Company_ID == logIn.company && a.Material_Grade == prodgrade
                         select a).ToList();
                if (S.Count > 0)
                {

                }
                else
                {
                    dataGridView1.Rows[i].Cells["Accepted_Grade"].Style.ForeColor = Color.Red;     
                    


                }

                //
                //ActReading = (R1.Cells[columnIndex].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells[columnIndex].Value);
                //StdSpec = (R2.Cells[columnIndex].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R2.Cells[columnIndex].Value);

                //if (ActReading > StdSpec)
                //{
                //    R1.Cells[columnIndex].Style.ForeColor = Color.Red;
                //    //R1.Cells["Accepted_Grade"].Value = "";
                //}
                //else
                //{
                //    R1.Cells[columnIndex].Style.ForeColor = Color.Black;
                //    //R1.Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                //}


            }

        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {


                db.Sp_Delete_Final_Chemcial_Report(logIn.company, txtInvNo.Text);
                MessageBox.Show("Record Deleted Successfully");
                this.Close();

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string SO_No = txtInvNo.Text;


                path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "LoadingSlip.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new OrderManagement.Transactions.LoadingSlip();

                SqlCommand cmd = new SqlCommand("sp_Rpt_LoadingSlip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slip_No", SO_No);
                cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);



                if (Dt.Rows.Count > 0)
                {


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
                    rep.SetDataSource(Dt);


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    //cmd.Parameters.Clear();
                }
                con.Close();


                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                string fileExcel = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                fdlg.Filter = "Excel Sheet(*.xlsx)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    fileExcel = fdlg.FileName;
                    //Application.DoEvents();
                }
                //fileExcel = "C:\\Users\\v4vis\\Downloads\\Wef Feb 2024.csv";
                //fileExcel = "D:\\SPECTROREDINGS\\Wef Feb 2024.csv";
                Cursor.Current = Cursors.WaitCursor;
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataTable DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;

                string SheetName = "Sheet1";
                // string ExcellSheet = ;

                //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + fileExcel + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                //MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileExcel + ";Extended Properties='Excel 8.0;HDR=Yes'");


                //dataGridView1.DataSource = DtSet;

                //string fileExcel = @"C:\test.xlsx";
                var application = new Excel.Application();
                var workbook = application.Workbooks.Open(fileExcel);
                var worksheet = workbook.ActiveSheet;
                var range = (Excel.Range)worksheet.Columns["D:D"];
                Excel.Range usedRange = worksheet.UsedRange;


                //Get Standard Spec
                var S = (from a in db.QA_Mtrl_Grade_Test_Specs
                         join m in db.QA_Mtrl_Grade_Masters on a.Mtrl_Grade_Id equals m.id
                         join p in db.QA_Test_Parameters on a.Parameter_ID equals p.id

                         where a.Company_id == logIn.company && m.Material_Grade == txtMtrlGrade.Text && p.id ==1 
                         select new { a.Spec_Min,a.Spec_Max }).ToList();

                if ((from d in db.QA_Chmical_Report_Finals where d.Company_Id == logIn.company && d.Report_Ref_No == txtInvNo.Text select d).Count() > 0)
                {
                    db.Sp_Delete_Final_Chemcial_Report(logIn.company, txtInvNo.Text);
                }
                int rval = 0;
                string searchstring = string.Empty;
                if (txtOldBatcNo.Text == string.Empty)
                {
                    searchstring = txtIntLotNo.Text;
                }
                else
                {
                    searchstring = txtOldBatcNo.Text;
                }
                    //Iterate the rows in the used range
                    foreach (Excel.Range row in usedRange.Rows)
                    {
                        var foundRange = row.Find(searchstring,
                        Type.Missing,
                        Excel.XlFindLookIn.xlValues,
                        Excel.XlLookAt.xlPart,
                        Excel.XlSearchOrder.xlByRows,
                        Excel.XlSearchDirection.xlNext,
                        false,
                        Type.Missing,
                        Type.Missing);

                        if (foundRange != null)
                        {
                            var col = foundRange.Column;
                            if (col == 4)
                            {
                                //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + SheetName + "$] ", MyConnection);
                                ////MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
                                //MyCommand.TableMappings.Add("Table", fileExcel);
                                //DtSet = new System.Data.DataTable();

                                //MyCommand.Fill(DtSet);
                                var numberRange = application.get_Range($"A{foundRange.Row}");
                                string fval = foundRange.Value;
                                string trim = Regex.Replace(fval, @" ", "");
                                var SampleID = Mid(trim.Trim(), 8, 2);
                                var sid = Regex.Replace(SampleID, @",", "");
                                //var aGrade = Mid(foundRange.Value, 23, 5);
                                var aGrade1 = Mid(trim, trim.Length - 5, 6);
                                var a = application.get_Range($"AC{foundRange.Row}");
                                rval = rval + 1;

                                //string ReadingData = application.get_Range($"AC{foundRange.Row}").ToString() + ',' + application.get_Range($"AE{foundRange.Row}").ToString();
                                //Console.WriteLine(foundRange.Value);
                                //Console.WriteLine(numberRange.Value);
                                string ReadingData = "";
                                string ChemParam = "";
                                string AccpGrade = "";
                                for (int j = 29; j <= 67; j++)
                                {
                                    string r = usedRange.Cells[foundRange.Row, j].Value.ToString();
                                    string c = usedRange.Cells[1, j].Value.ToString();

                                    //check carbon is with in the specified Limits
                                    if (c == "C")
                                    {
                                        if (r.Length > 1)
                                        {
                                            decimal Cactual = Convert.ToDecimal(r);
                                            decimal CSpecMin = Convert.ToDecimal(S[0].Spec_Min);
                                            decimal CSpecMax = Convert.ToDecimal(S[0].Spec_Max);
                                            if (Cactual >= CSpecMin && Cactual <= CSpecMax)
                                            {
                                                AccpGrade = txtMtrlGrade.Text;
                                            }
                                            else
                                            {
                                                AccpGrade = "Comm Grade";
                                            }
                                        }
                                    }

                                    if (ReadingData != "")
                                    {
                                        //MessageBox.Show(numberRange.Cells[foundRange.Row, j].Value2.ToString());


                                        if (r.Length > 1)
                                        {
                                            ReadingData = ReadingData + ',' + r;
                                            ChemParam = ChemParam + ',' + c;
                                        }
                                    }
                                    else
                                    {
                                        //string r = usedRange.Cells[foundRange.Row, j].Value.ToString();
                                        //string c = usedRange.Cells[1, j].Value.ToString();
                                        ReadingData = r;
                                        ChemParam = c;
                                    }
                                }


                                QA_Chmical_Report_Final pb = new QA_Chmical_Report_Final();
                                pb.Report_Ref_No = txtInvNo.Text;
                                pb.Insp_Date = dpInvDate.Value;
                                pb.FG_Batch_No = txtGRNNo.Text;
                                //pb.Supplier_Heat_No = txtSupplierHeat.Text;
                                //pb.Supplier_TC_No = txtSupplierTCno.Text;
                                pb.RM_Lot_No = txtIntLotNo.Text;
                                pb.FG_Batch_Qty = Convert.ToDecimal(txtBatchQty.Text);
                                pb.Inspected_By = txtInspectedBy.Text;
                                pb.Approved_By = txtApprovedBy.Text;

                                pb.Sample_ID = rval.ToString();

                                pb.Chemical_Readings = ReadingData;
                                pb.Chem_Param_Seq = "";

                                pb.Accepted_Grade = AccpGrade;
                                //pb.New_Batch_No = txtIntLotNo.Text; ;
                                if (AccpGrade != txtMtrlGrade.Text)
                                {
                                    pb.Result = 1159;
                                    //pb.New_Batch_No = txtIntLotNo.Text + "/1";
                                }
                                else
                                {
                                    pb.Result = Convert.ToInt32(cmbStatus.SelectedValue);
                                    //pb.New_Batch_No = txtIntLotNo.Text; ;
                                }
                                //pb.Doc_Link = (txtDocPath.Text == null) ? "" : txtDocPath.Text;
                                //pb.Result = 1156;
                                //pb.Visual_Inspection = checkBox1.Checked;
                                //pb.Comments_Remarks = txtComments.Text;
                                pb.Company_Id = logIn.company;
                                pb.Created_By = lblCreatedBy.Text;
                                pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                                db.QA_Chmical_Report_Finals.InsertOnSubmit(pb);


                                db.SubmitChanges();


                                //pb.Chemical_Readings = ReadingData;

                                //ReadingData = "";

                            }

                        }

                    }
                BindEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                var mappingName1 = sfDataGrid1.Columns[2].MappingName;
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                // var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
                txtInvNo.Text = cellVaue.ToString();
                txtGRNNo.Text = cellVaue1.ToString();
                BindEdit();

                groupBox1.Visible = false;


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
        public void GetGRNData()
        {
            try
            {
                if (txtGRNNo.Text != "")
                {
                    int grn_id = 0;
                    var da = (from obj in db.Production_Report_Rolling_Sections
                              join s in db.Products on obj.Prod_Id equals s.prod_ID
                              join m in db.QA_Mtrl_Grade_Masters on obj.Prod_Grade equals m.id
                              where obj.FG_Lot_No == txtGRNNo.Text && obj.Company_Id == logIn.company
                              select new { s.Prod_Name, m.Material_Grade , obj.Qty_Finished, m.id,s.prod_ID}).ToList();

                    if (da.Count > 0)
                    {
                        txtProdName.Text = da[0].Prod_Name;
                        txtMtrlGrade.Text = da[0].Material_Grade;
                        txtBatchQty.Text = da[0].Qty_Finished.ToString();
                    }

                    //var da1 = (from obj in db.GoodsReceiptNote_Childs
                    //           join s in db.Products on obj.Prod_Code equals s.prod_ID
                    //           where obj.Grn_NO == txtGRNNo.Text && obj.Company_ID == logIn.company
                    //           select new
                    //           {
                    //               Prod_Code = obj.Prod_Code,
                    //               RM_Sec = s.Prod_Name,
                    //               Item_Grade = obj.Prod_Grade,
                    //               TC_No = obj.TCNo,
                    //               obj.Heat_No,
                    //               obj.Int_Batch_No
                    //           }).ToList();

                    //txtProdName.Text = da1[0].RM_Sec;
                    //txtMtrlGrade.Text = da1[0].Item_Grade;
                    //txtSupplierTCno.Text = da1[0].TC_No;
                    //txtSupplierHeat.Text = da1[0].Heat_No;
                    //txtIntLotNo.Text = da1[0].Int_Batch_No;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CallChemParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_ChemicalTest_Parameters", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@mtrlgrade", txtMtrlGrade.Text);

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dataGridView1.DataSource = ds2;
            dataGridView1.Rows[0].Cells["Sample_ID"].Value = "Spec";
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;

            
        }
        private void BindEdit()
        {
           
            
            var sa = (from sq in db.QA_Chmical_Report_Finals
                      join g in db.Production_Report_Rolling_Sections on sq.FG_Batch_No equals g.FG_Lot_No
                      //join gc in db.GoodsReceiptNote_Childs on g.Id equals gc.GRN_Master_ID
                      join su in db.QA_Mtrl_Grade_Masters on g.Prod_Grade equals su.id
                      join p in db.Products on g.Prod_Id equals p.prod_ID
                      where sq.Company_Id == logIn.company && sq.Report_Ref_No == txtInvNo.Text orderby sq.id
                      select new
                      {
                          sq.Insp_Date,
                          sq.RM_Lot_No,
                          sq.FG_Batch_No,
                          sq.FG_Batch_Qty,
                          sq.Inspected_By,
                          sq.Approved_By,
                          sq.Created_By,
                          sq.Modified_BY,
                          sq.Visual_Inspection,
                          sq.Result,
                          sq.Comments_Remarks,
                          sq.Chemical_Readings,                          
                          p.Prod_Name,
                          su.Material_Grade ,                                         
                          sq.Doc_Link,                                              
                          sq.Accepted_Grade,                         
                          sq.Sample_ID
                      }).ToList();
            if (sa.Count > 0)
            {

                dpInvDate.Text = sa[0].Insp_Date.Value.ToString();
                txtGRNNo.Text = sa[0].FG_Batch_No;
                //txtOldBatcNo.Text = sa[0].grnID.ToString();
                txtProdName.Text = sa[0].Prod_Name.ToString();
                //txtSuppName.Text = sa[0].Supplier_Name;
                txtMtrlGrade.Text = sa[0].Material_Grade;
                txtIntLotNo.Text = sa[0].RM_Lot_No;
                //txtSupplierHeat.Text = sa[0].Supplier_Heat_No;
                //txtSupplierTCno.Text = sa[0].Supplier_TC_No;
                txtBatchQty.Text = sa[0].FG_Batch_Qty.ToString();
                txtInspectedBy.Text = sa[0].Inspected_By;
                txtApprovedBy.Text = sa[0].Approved_By;
                txtComments.Text = sa[0].Comments_Remarks;
                cmbStatus.SelectedValue = sa[0].Result;
                if (sa[0].Visual_Inspection == true)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
                
                txtDocPath.Text = sa[0].Doc_Link;
                //txtSuppInvNo.Text = sa[0].Supplier_InvNo;
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_BY;

                string p;
                DataTable dt = new DataTable();
                CallChemParamters();
                System.Data.DataRow dr;
                dr = dt.NewRow();
                int k = dataGridView1.Rows.Count;
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dt.Columns.Add(dataGridView1.Columns[i].Name);

                    dr[dataGridView1.Columns[i].Name] = dataGridView1.Rows[0].Cells[i].Value.ToString();

                }

                
                //for (int i = 0; i < dataGridView1.ColumnCount; i++)
                //{
                //    dr[dataGridView1.Columns[i].Name] = dataGridView1.Rows[0].Cells[i].Value.ToString();
                //}
                dt.Rows.Add(dr);
                int ColIndex=0;
                int gridcolcount = dt.Columns.Count;
                for (int j = 0; j < sa.Count; j++)
                {
                    
                    dr = dt.NewRow();
                    string s = sa[j].Chemical_Readings;
                    string[] values = s.Split(',');

                    dr[0] = sa[j].Sample_ID;
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        p = values[i].Trim();

                        //DataRow dr;

                        dr[i+1] = p;
                        ColIndex = i+1;

                    }
                    dr[gridcolcount - 2 ] = sa[j].Accepted_Grade;
                    //dr[gridcolcount - 1 ] = sa[j].New_Batch_No;

                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;

                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;
               
                decimal ActReading = 0;
                decimal StdSpec = 0;
                //for (int i = 1; i < dataGridView1.ColumnCount-3; i++)
                //{
                //    for (int r = 0; r < dataGridView1.RowCount - 1; r++)
                //    {
                //        //dataGridView1.Rows[r].Cells[i].Value.ToString();
                //        ActReading = (dataGridView1.Rows[r].Cells[i].Value == DBNull.Value || dataGridView1.Rows[r].Cells[i].Value == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[r].Cells[i].Value);
                //        StdSpec = (dataGridView1.Rows[0].Cells[i].Value == DBNull.Value || dataGridView1.Rows[0].Cells[i].Value =="") ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells[i].Value);

                //        if (ActReading > StdSpec)
                //        {
                //            dataGridView1.Rows[r].Cells[i].Style.ForeColor = Color.Red;
                //            //R1.Cells["Accepted_Grade"].Value = "";
                //        }
                //        else
                //        {
                //            dataGridView1.Rows[r].Cells[i].Style.ForeColor = Color.Black;
                //            //R1.Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                //        }
                //    }
                //}

            }
        }
    }
}
