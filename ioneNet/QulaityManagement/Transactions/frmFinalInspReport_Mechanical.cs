using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using ioneNet.QulaityManagement.Reports;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class frmFinalInspReport_Mechanical: Form
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

        public frmFinalInspReport_Mechanical()
        {
            InitializeComponent();
        }

        private void txtMtrlGrade_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddGrades(DataColl);
                txtMtrlGrade.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmFinalInspReport_Mechanical_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtInspectedBy.Text = logIn.username;
            txtApprovedBy.Text = logIn.username;
            //Status
            btnSave.Enabled = true;
            cmdDelete.Enabled = true;
            button1.Enabled = true;
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Insp Result" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }
            AutoincrementId();
            if(FG_Lot_Register.ReportRef != null)
            {
                txtInvNo.Text = FG_Lot_Register.ReportRef;
                BindEdit();
                btnSave.Enabled = false;
                cmdDelete.Enabled = false;
                button1.Enabled = false;
                FG_Lot_Register.ReportRef = null;
            }

        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
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
                Application.DoEvents();
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ReadingData = "";
                if ((from a in db.Inprocess_Dimensional_Reports where a.Company_Id == logIn.company && a.Report_Ref_No == txtInvNo.Text && a.Test_Type == "Mechanical" select a).Count() > 0)
                {
                    db.Sp_Delete_DimensionalReport(logIn.company, txtInvNo.Text, "Mechanical");
                }
                string Reading = "0";
                string ParaSeq = "";
                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    //if(dgrmconsumed.Rows[i].Cells["Item_ID"].Value.ToString() !=null)
                    //{


                    Inprocess_Dimensional_Report pb = new Inprocess_Dimensional_Report();
                    pb.Report_Ref_No = txtInvNo.Text;
                    pb.Insp_Date = dpInvDate.Value;
                    pb.Test_Type = "Mechanical";
                    pb.Fg_Lot_No = txtIntLotNo.Text;
                    pb.Prod_date = dateTimePicker1.Value;
                    pb.Prod_id = Convert.ToInt32(txtProd_Id.Text);
                    pb.Material_grade = Convert.ToInt32(txtGradeId.Text); ;
                    pb.Inspected_By = txtInspectedBy.Text;
                    pb.Approved_By = txtApprovedBy.Text;

                    pb.Sample_Time = dataGridView1.Rows[i].Cells["Sample_No"].Value.ToString();
                    for (int j = 1; j < dataGridView1.Columns.Count - 1; j++)
                    {
                        if(Convert.ToString(dataGridView1.Rows[i].Cells[j].Value).Length>0)
                        {
                            Reading = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value).ToString();
                        }
                        else
                        {
                            Reading = "0";
                        }
                        if (ReadingData != "")
                        {
                            ReadingData = ReadingData + ',' + Convert.ToString(Reading);
                        }
                        else
                        {
                            ReadingData = Convert.ToString(Reading);
                        }
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
                    pb.Dimensional_Result = ReadingData;
                    ReadingData = "";
                    pb.Test_Para_Seq = ParaSeq;
                    ParaSeq = "";
                    pb.Remarks = dataGridView1.Rows[i].Cells["Remarks"].Value.ToString();
                    pb.Doc_Link = (txtDocPath.Text == null) ? "" : txtDocPath.Text;
                    pb.Result = Convert.ToInt32(cmbStatus.SelectedValue);
                    pb.Visual_Inspection = checkBox1.Checked;
                    pb.Comments_Remarks = txtComments.Text;
                    pb.Company_Id = logIn.company;
                    pb.Created_By = lblCreatedBy.Text;
                    pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Inprocess_Dimensional_Reports.InsertOnSubmit(pb);
                }
                //}

                db.SubmitChanges();
                MessageBox.Show("Record Updated Sucessfully");
                this.Close();


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

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            CallDimParamters();
        }

        private void txtProdName_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddProd(DataColl);
                txtProdName.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtMtrlGrade_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                int MgradeId;
                if (txtMtrlGrade.Text != "")
                {

                    var da = (from obj in db.QA_Mtrl_Grade_Masters
                              where obj.Material_Grade == txtMtrlGrade.Text
                              select obj).ToList();
                    if (da.Count > 0)
                    {

                        MgradeId = da[0].id;
                        txtGradeId.Text = da[0].id.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Grade Selected");
                        txtMtrlGrade.Focus();
                        return;
                    }

                    //get BatchNo
                    string d1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    var Prodname = (from d in db.Production_Report_Rolling_Sections
                                    join k in db.Production_report_rollings on d.PR_Master_ID equals k.ID
                                    join p in db.QA_Mtrl_Grade_Masters on d.Prod_Grade equals p.id

                                    where d.Company_Id == logIn.company && k.PR_date == Convert.ToDateTime(d1)
                                    && d.Prod_Id == Convert.ToInt32(txtProd_Id.Text) && d.Prod_Grade == MgradeId
                                    && k.Conversion_Production == false
                                    select new { d.FG_Lot_No, d.Qty_Finished }).ToList();

                   

                    if (Prodname.Count > 0)
                    {

                        txtIntLotNo.Text = Prodname[0].FG_Lot_No.ToString();
                        txtBatchQty.Text = Prodname[0].Qty_Finished.ToString();
                    }
                    else
                    {
                        //MessageBox.Show("Invalid Product Selected");
                        //txtProdName.Focus();
                        //return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProdName_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (txtProdName.Text != "")
                {

                    var da = (from obj in db.Products
                              where obj.Prod_Name == txtProdName.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtProd_Id.Text = da[0].prod_ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Product Selected");
                        txtProdName.Focus();
                        return;
                    }
                }
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




                var p = (from s in db.Inprocess_Dimensional_Reports
                         where s.Company_Id == logIn.company && s.Test_Type == "Mechanical"

                         select new
                         {                            
                             s.Report_Ref_No,
                             s.Insp_Date,
                             s.Prod_date,
                             s.Fg_Lot_No
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
        public int RId;
        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["Report_Ref_No"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
            reportNo = cellVaue.ToString();
            txtInvNo.Text = reportNo;
            BindEdit();
            groupBox1.Visible = false;
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_DimensionalReport(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, "Mechanical");
                txtInvNo.Text = result.FirstOrDefault().Report_No;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {


                db.Sp_Delete_DimensionalReport(logIn.company, txtInvNo.Text, "Mechanical");
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

        private void button3_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["Report_Ref_No"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
            reportNo = cellVaue.ToString();
            txtInvNo.Text = reportNo;
            BindEdit();
            groupBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void txtIntLotNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtIntLotNo.Text != "")
                {
                    if ((from a in db.Inprocess_Dimensional_Reports

                         where a.Company_Id == logIn.company && a.Fg_Lot_No == txtIntLotNo.Text
                         select a).Count() > 0)
                    {
                        MessageBox.Show("Inspection Already Completed For The Selected Batch No");
                        txtIntLotNo.Focus();
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
        public void GetGRNData()
        {
            try
            {
                if (txtIntLotNo.Text != "")
                {
                    int grn_id = 0;
                    var da = (from obj in db.Production_Report_Rolling_Sections
                              join s in db.Products on obj.Prod_Id equals s.prod_ID
                              join m in db.QA_Mtrl_Grade_Masters on obj.Prod_Grade equals m.id
                              where obj.FG_Lot_No == txtIntLotNo.Text && obj.Company_Id == logIn.company
                              select new { s.Prod_Name, m.Material_Grade, obj.Qty_Finished, m.id, s.prod_ID }).ToList();

                    if (da.Count > 0)
                    {
                        txtProdName.Text = da[0].Prod_Name;
                        txtMtrlGrade.Text = da[0].Material_Grade;
                        txtBatchQty.Text = da[0].Qty_Finished.ToString();
                        txtProd_Id.Text = da[0].prod_ID.ToString();
                        txtGradeId.Text = da[0].id.ToString();

                    }

                    

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddProd(AutoCompleteStringCollection coll)
        {
            try
            {
                string d1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                var Prodname = (from d in db.Production_Report_Rolling_Sections
                                join k in db.Production_report_rollings on d.PR_Master_ID equals k.ID
                                join p in db.Products on d.Prod_Id equals p.prod_ID

                                where d.Company_Id == logIn.company && k.PR_date == Convert.ToDateTime(d1)


                                select new { p.Prod_Name }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Prod_Name");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Prod_Name);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void AddGrades(AutoCompleteStringCollection coll)
        {
            try
            {
                string d1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                var Prodname = (from d in db.Production_Report_Rolling_Sections
                                join k in db.Production_report_rollings on d.PR_Master_ID equals k.ID
                                join p in db.QA_Mtrl_Grade_Masters on  d.Prod_Grade equals p.id

                                where d.Company_Id == logIn.company && k.PR_date == Convert.ToDateTime(d1)

                                select new { p.Material_Grade }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Material_Grade");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Material_Grade);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void CallDimParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_MechanicalTest_Parameters", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@GradeID", Convert.ToInt32(txtGradeId.Text));

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dataGridView1.DataSource = ds2;
            dataGridView1.Rows[0].Cells["Sample_No"].Value = "Spec";
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }
        private void BindEdit()
        {


            var sa = (from sq in db.Inprocess_Dimensional_Reports
                      join su in db.QA_Mtrl_Grade_Masters on sq.Material_grade equals su.id
                      join p in db.Products on sq.Prod_id equals p.prod_ID
                      where sq.Company_Id == logIn.company && sq.Report_Ref_No == txtInvNo.Text && sq.Test_Type== "Mechanical"
                      orderby sq.id
                      select new
                      {
                          sq.Insp_Date,
                          sq.Fg_Lot_No,
                          sq.Prod_date,
                          sq.Prod_id,
                          grade_id = sq.Material_grade,
                          p.Prod_Name,
                          su.Material_Grade,
                          sq.Inspected_By,
                          sq.Approved_By,
                          sq.Created_By,
                          sq.Modified_BY,
                          sq.Visual_Inspection,
                          sq.Result,
                          sq.Comments_Remarks,
                          sq.Dimensional_Result,
                          sq.Doc_Link,
                          sq.Remarks,
                          sq.Sample_Time,

                          sq.Report_Ref_No
                      }).ToList();
            if (sa.Count > 0)
            {
                txtInvNo.Text = sa[0].Report_Ref_No;
                dpInvDate.Text = sa[0].Insp_Date.Value.ToString();
                dateTimePicker1.Text = sa[0].Prod_date.Value.ToString();

                txtGradeId.Text = sa[0].grade_id.ToString();
                txtProdName.Text = sa[0].Prod_Name.ToString();
                txtProd_Id.Text = sa[0].Prod_id.ToString();
                txtMtrlGrade.Text = sa[0].Material_Grade.ToString();
                txtIntLotNo.Text = sa[0].Fg_Lot_No;
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
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_BY;

                string p;
                DataTable dt = new DataTable();
                CallDimParamters();
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
                int ColIndex = 0;
                int gridcolcount = dt.Columns.Count;
                for (int j = 0; j < sa.Count; j++)
                {

                    dr = dt.NewRow();
                    string s = sa[j].Dimensional_Result;
                    string[] values = s.Split(',');

                    dr[0] = sa[j].Sample_Time;
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        p = values[i].Trim();

                        //DataRow dr;

                        dr[i + 1] = p;
                        ColIndex = i + 1;

                    }

                    dr[gridcolcount - 1] = sa[j].Remarks;

                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;

                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;

            }
        }
    }
}
