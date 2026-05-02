using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.Shared;
using Ione_DAL;
using OpenCvSharp;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
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

namespace ioneNet.QulaityManagement.Transactions
{
    public partial class frmTestCertificate: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode;

        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private Database crDatabase;
        private string path, TCNo;
        public frmTestCertificate()
        {
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void frmTestCertificate_Load(object sender, EventArgs e)
        {
            try
            {
                dpTCDate.MinDate = logIn.fy_Start_Date;
                dpTCDate.MaxDate = logIn.fy_End_Date;
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                AutoincrementId();

                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                CmbBuyerName.SelectedIndex = -1;

                var prodBind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (prodBind.Count > 0)
                {
                    cmbProduct.DataSource = prodBind;
                    cmbProduct.ValueMember = "ID";
                    cmbProduct.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                cmbProduct.SelectedIndex = -1;

                //var MgradeBind = (from m in db.QA_Mtrl_Grade_Masters where m.Company_ID == logIn.company select new { m.id, m.Material_Grade }).Distinct().ToList();
                //if (MgradeBind.Count > 0)
                //{
                //    cmbMtrlGrade.DataSource = MgradeBind;
                //    cmbMtrlGrade.ValueMember = "id";
                //    cmbMtrlGrade.DisplayMember = "Material_Grade";

                //}
                ////if (CmbBuyerName.Items.Count > 0)
                //cmbMtrlGrade.SelectedIndex = -1;


                //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
                if (frmListOfTestCertificates.TC_No != null)
                {
                    txtTCNo.Text = frmListOfTestCertificates.TC_No;
                    //txtInvNo.Text = cellVaue.ToString();
                    BindEdit();
                    editMode = true;
                    frmListOfTestCertificates.TC_No = null;

                }
                //groupBox1.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AutoincrementId()
        {
            try
            {
                var result = db.Sp_autoincrement_TestCertificate(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtTCNo.Text = result.FirstOrDefault().Report_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            //SqlCommand cmd2 = new SqlCommand("Get_FGLOt_For_TC", con);
            //cmd2.CommandType = CommandType.StoredProcedure;
            //cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //cmd2.Parameters.AddWithValue("@prodid", Convert.ToInt32(cmbProduct.SelectedValue));
            //cmd2.Parameters.AddWithValue("@mtrlgrade", Convert.ToInt32(cmbMtrlGrade.SelectedValue));

            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            ////DataSet ds2 = new DataSet();
            //DataTable ds2 = new DataTable();
            //// da2.Fill(ds2, "x");
            //da2.Fill(ds2);
            //dgFgLotDetails.DataSource = ds2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd5 = new SqlCommand("delete  from [Temp_FGLot_For_TC]", con);
            if (con.State != ConnectionState.Open)
                con.Open();
            
            cmd5.ExecuteNonQuery();

            SqlCommand cmd1 = con.CreateCommand();
            for (int i = 0; i < dgFgLotDetails.Rows.Count - 1; i++)
            {
                decimal lotqty = (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);
                //if (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value.ToString() != "" || dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value != null)
                //{
                    //decimal lotqty = (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);

                    if (lotqty > 0)
                    {
                        cmd1.CommandText = "INSERT INTO Temp_FGLot_For_TC  (FG_Lot_No) VALUES  (@FGLotNo)";
                        string fglot = dgFgLotDetails.Rows[i].Cells["FG_Batch_No"].Value.ToString();
                        cmd1.Parameters.AddWithValue("@FGLotNo", fglot);
                        
                        cmd1.ExecuteNonQuery();
                        cmd1.Parameters.Clear();
                    }
                //}
            }


            CallChemParamters();
            CallMechParamters();
        }
        
        private void CallChemParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_ChemicalTest_Parameters_For_TC", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //cmd2.Parameters.AddWithValue("@mtrlgrade", "");

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dgChemData.DataSource = ds2;
            //dgChemData.Rows[0].Cells["Chem_FG_Batch_No"].Value = "Spec";
            //dgChemData.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }
        private void CallMechParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_MechanicalTest_Parameters_For_TC", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //cmd2.Parameters.AddWithValue("@mtrlgrade", "");
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dgMechData.DataSource = ds2;
            //dgChemData.Rows[0].Cells["Chem_FG_Batch_No"].Value = "Spec";
            //dgChemData.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                if ((from a in db.QA_Test_Certificate_Masters where a.Company_Id == logIn.company && a.TC_NO == txtTCNo.Text select a).Count() > 0)
                {
                    db.Sp_Delete_TestCertificate(logIn.company, txtTCNo.Text);
                }
                
                MessageBox.Show("Record Deleted Successfully");
                this.Close();

            }

           
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ReadingData = "";
                if ((from a in db.QA_Test_Certificate_Masters where a.Company_Id == logIn.company && a.TC_NO == txtTCNo.Text select a).Count() > 0)
                {
                    db.Sp_Delete_TestCertificate(logIn.company, txtTCNo.Text);
                }

                

                for (int i = 0; i < dgFgLotDetails.Rows.Count - 1; i++)
                {
                    decimal lotqty = (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value ==""|| dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);
                    if (lotqty > 0)
                    //    if (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value != "");
                    //{
                        
                        {
                            QA_Test_Certificate_Master pb = new QA_Test_Certificate_Master();
                        string MtrlGrade = (dgFgLotDetails.Rows[i].Cells["Prod_Grade"].Value == null) ? "" : (dgFgLotDetails.Rows[i].Cells["Prod_Grade"].Value).ToString();

                        var d1 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == MtrlGrade select new { a.id }).ToList();


                        pb.TC_NO = txtTCNo.Text;
                        pb.TC_Date = dpTCDate.Value;
                        pb.Customer_ID = Convert.ToInt32(CmbBuyerName.SelectedValue);
                        pb.Consignee_ID = Convert.ToInt32(cmbProduct.SelectedValue);
                        pb.Inv_No = txtInvNo.Text;
                        pb.Loading_Slip_Ref = txtLoadingSlipNo.Text;
                        //pb.Inv_Qty = Convert.ToDecimal(txtInvQty.Text);
                        pb.Fg_Item_ID = Convert.ToInt32(dgFgLotDetails.Rows[i].Cells["Prod_Id"].Value) ;
                        pb.FG_Grade_ID = d1[0].id;
                        pb.FG_Lot_NO = dgFgLotDetails.Rows[i].Cells["FG_Batch_No"].Value.ToString();
                        pb.Inv_Qty  = (dgFgLotDetails.Rows[i].Cells["TC_Qty"].Value == "" || dgFgLotDetails.Rows[i].Cells["TC_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["TC_Qty"].Value);
                        //pb.Batch_Qty_Balance = (dgFgLotDetails.Rows[i].Cells["Batch_Qty_Balance"].Value == "" || dgFgLotDetails.Rows[i].Cells["Batch_Qty_Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Batch_Qty_Balance"].Value);
                        pb.Remarks = txtComments.Text;
                        pb.FG_Lot_Qty_Issued = lotqty ;
                        pb.RM_Lot_No = dgFgLotDetails.Rows[i].Cells["RM_Lot_No"].Value.ToString();
                        pb.Company_Id = logIn.company;
                        pb.Created_By = lblCreatedBy.Text;
                        pb.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.QA_Test_Certificate_Masters.InsertOnSubmit(pb);
                        }
                    //}
                }
                db.SubmitChanges();



                MessageBox.Show("Record Updated Sucessfully");



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

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string SO_No = txtTCNo.Text;


                path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "TestCertificate.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new ioneNet.QulaityManagement.Transactions.TestCertificate();

                SqlCommand cmd5 = new SqlCommand("delete  from [Temp_FGLot_For_TC]", con);
                if (con.State != ConnectionState.Open)
                    con.Open();

                cmd5.ExecuteNonQuery();

                SqlCommand cmd1 = con.CreateCommand();
                for (int i = 0; i < dgFgLotDetails.Rows.Count - 1; i++)
                {
                 //   if (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value.ToString() != "")
                   // {
                        decimal lotqty = (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);

                        if (lotqty > 0)
                        {
                            cmd1.CommandText = "INSERT INTO Temp_FGLot_For_TC  (FG_Lot_No) VALUES  (@FGLotNo)";
                            string fglot = dgFgLotDetails.Rows[i].Cells["FG_Batch_No"].Value.ToString();
                            cmd1.Parameters.AddWithValue("@FGLotNo", fglot);
                            cmd1.ExecuteNonQuery();
                            cmd1.Parameters.Clear();
                        }
                    //}
                }

                CallChemParamters();
                CallMechParamters();


                



                


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
                rep.RecordSelectionFormula = "{QA_Test_Certificate_Master.TC_NO} = '" + txtTCNo.Text + "'";


                //rep.Subreports[0].SetParameterValue(0, logIn.company);
                //rep.Subreports[0].SetParameterValue(1, cmbMtrlGrade.Text);
                rep.SetParameterValue("@CompName", logIn.company, rep.Subreports[0].Name.ToString());
                rep.SetParameterValue("@mtrlgrade","", rep.Subreports[0].Name.ToString());
                rep.SetParameterValue("@CompName", logIn.company, rep.Subreports[1].Name.ToString());
                rep.SetParameterValue("@mtrlgrade", "", rep.Subreports[1].Name.ToString());
                //rep.Subreports[1].SetParameterValue(0, logIn.company);
                //rep.Subreports[1].SetParameterValue(1, cmbMtrlGrade.Text);

                //rep.SetParameterValue("TC_No", txtTCNo,Text);
                //rep.SetParameterValue("Creation_Company", logIn.company);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    //cmd.Parameters.Clear();
                
                


                Process.Start(path);

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




                var p = (from s in db.QA_Test_Certificate_Masters
                         
                         where s.Company_Id == logIn.company 

                         select new
                         {
                             s.TC_NO,
                             s.TC_Date,
                             s.Inv_No
                         }
                        ).Distinct().ToList();

                if (p.Count >= 0)
                {
                    sfDataGrid1.DataSource = p;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["TC_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["TC_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["TC_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["TC_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Inv_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Inv_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Inv_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Inv_No"].FilterRowCondition = FilterRowCondition.Contains;
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

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
            TCNo = cellVaue.ToString();
            //txtInvNo.Text = cellVaue.ToString();
            BindEdit();
            groupBox1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
            TCNo = cellVaue.ToString();
            //txtInvNo.Text = cellVaue.ToString();
            BindEdit();
            groupBox1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void dgFgLotDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow R1 = dgFgLotDetails.Rows[dgFgLotDetails.CurrentRow.Index];
            //DataGridViewRow R2 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index-1];
            int columnIndex = dgFgLotDetails.CurrentCell.ColumnIndex;
            string columnName = dgFgLotDetails.Columns[columnIndex].Name;

            decimal pqty = Convert.ToDecimal(txtInvQty.Text);
            decimal rmqty = Convert.ToDecimal(R1.Cells["Batch_Qty_Balance"].Value);
            decimal allotqty = Convert.ToDecimal(R1.Cells["Issue_Batch_Qty"].Value);
            if (allotqty > rmqty)
            {
                MessageBox.Show("Allotment Qty Cannot Greater Than Batch Available Qty");
                R1.Cells["Issue_Batch_Qty"].Value = "0.00";
                return;
            }

            decimal y = 0;
            for (int i = 0; i < dgFgLotDetails.Rows.Count - 1; i++)
            {
                y += (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == null || dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);


            }
            //decimal tqty = pqty * 10 / 100;
            if (y > (pqty))
            {
                MessageBox.Show("Total Allotment Qty Cannot Greater Than Invoice Qty");
                txtTotalBatchQty.Text = "0.00";
                return;
            }
            else
            {
                txtTotalBatchQty.Text = (y).ToString();
            }
        }

        private void txtLoadingSlipNo_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (txtLoadingSlipNo.Text != "")
                {

                    var da = (from obj in db.Loading_Childs
                              join l in db.Loading_Masters on obj.Loading_Master_ID equals l.Id
                              join s in db.Sale_Order_Masters on obj.SO_Ref_No equals s.SO_NO
                              where obj.Loading_Slip_No == txtLoadingSlipNo.Text
                              select new {s.BuyerName,s.ConsigneeName, l.VehicleNo}).ToList();

                    if (da.Count > 0)
                    {

                        CmbBuyerName.SelectedValue = da[0].BuyerName;
                        cmbProduct.SelectedValue = da[0].ConsigneeName;
                        txtVehicleNo.Text = da[0].VehicleNo.ToString();


                        //var prodBind = (from m in db.Loading_Childs
                        //                join p in db.Products on m.Prod_Code equals p.prod_ID
                        //                where m.Company_ID == logIn.company && m.Loading_Slip_No == txtLoadingSlipNo.Text

                        //                select new { p.prod_ID, p.Prod_Name }).Distinct().ToList();
                        //if (prodBind.Count > 0)
                        //{
                        //    cmbProduct.DataSource = prodBind;
                        //    cmbProduct.ValueMember = "prod_ID";
                        //    cmbProduct.DisplayMember = "Prod_Name";

                        //}
                        //if (CmbBuyerName.Items.Count > 0)
                        //cmbProduct.SelectedIndex = -1;
                        SqlCommand cmd2 = new SqlCommand("SP_Get_LoadingSLipData_For_TC", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@compname", logIn.company);
                        cmd2.Parameters.AddWithValue("@loadingSlipNo", txtLoadingSlipNo.Text);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        //DataSet ds2 = new DataSet();
                        DataTable ds2 = new DataTable();
                        // da2.Fill(ds2, "x");
                        da2.Fill(ds2);
                        dgFgLotDetails.DataSource = ds2;



                        //var GradeBind = (from m in db.Loading_Childs
                        //                 join s in db.Sale_Order_Childs on new { x = m.SO_Item_No, y = m.SO_Ref_No } equals new { x = s.enq_item_no, y = s.SO_NO }
                        //                join g in db.QA_Mtrl_Grade_Masters on s.Prod_Grade_Id equals g.id
                        //                join p in db.Products on m.Prod_Code equals p.prod_ID
                        //                 where m.Company_ID == logIn.company && m.Loading_Slip_No == txtLoadingSlipNo.Text 
                                        

                        //                select new { p.prod_ID,p.Prod_Name, Prod_Grade = g.Material_Grade, TC_Qty = m.Qty_Loaded });
                        ////if (GradeBind.Count > 0)
                        ////{
                        //    SqlCommand cmd3 = (SqlCommand)db.GetCommand(GradeBind);
                        //    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                        //    DataTable dtr1 = new DataTable();
                        //    da3.Fill(dtr1);
                        //    if (dtr1.Rows.Count >= 0)
                                //dgFgLotDetails.DataSource = dtr1;

                        //}
                        //if (CmbBuyerName.Items.Count > 0)
                        //cmbMtrlGrade.SelectedIndex = -1;

                    }
                    else
                    {
                        MessageBox.Show("Invalid Loading Slip Entered Selected");
                        txtLoadingSlipNo.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgFgLotDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                int i = dgFgLotDetails.CurrentCell.RowIndex;
                string ItemCode = (dgFgLotDetails.Rows[i].Cells["Prod_Id"].Value == null) ? "" : (dgFgLotDetails.Rows[i].Cells["Prod_Id"].Value).ToString();
                string MtrlGrade = (dgFgLotDetails.Rows[i].Cells["Prod_Grade"].Value == null) ? "" : (dgFgLotDetails.Rows[i].Cells["Prod_Grade"].Value).ToString();
                txtTCQty.Text = (dgFgLotDetails.Rows[i].Cells["TC_Qty"].Value == null) ? "" : (dgFgLotDetails.Rows[i].Cells["TC_Qty"].Value).ToString();
                var d1 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == MtrlGrade select new { a.id }).ToList();
              

                SqlCommand cmd2 = new SqlCommand("Get_FGLOt_For_TC", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@prodid", Convert.ToInt32(ItemCode));
                cmd2.Parameters.AddWithValue("@mtrlgrade", Convert.ToInt32(d1[0].id));

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dgGetFGLots.DataSource = ds2;

                groupBox2.Visible = true;
            }
        }
        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void button5_Click(object sender, EventArgs e)
        {

            int j = dgFgLotDetails.CurrentRow.Index;


            for (int i = 0; i < dgGetFGLots.Rows.Count - 1; i++)
            {
                if (dgGetFGLots.Rows[i].Cells["Issue_Batch_Qty"].Value.ToString() != "")
                {
                    decimal lotqty = (dgGetFGLots.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgGetFGLots.Rows[i].Cells["Issue_Batch_Qty"].Value);

                    if (lotqty > 0)
                    {
                        dgFgLotDetails.Rows[j].Cells["Issue_Batch_Qty"].Value = lotqty;
                        dgFgLotDetails.Rows[j].Cells["FG_Batch_No"].Value = dgGetFGLots.Rows[i].Cells["FG_Batch_No"].Value;
                        dgFgLotDetails.Rows[j].Cells["RM_Lot_No"].Value = dgGetFGLots.Rows[i].Cells["RM_Lot_No"].Value;
                        groupBox2.Visible = false;

                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void txtLoadingSlipNo_LocationChanged(object sender, EventArgs e)
        {

        }

        private void BindEdit()
        {


            var sa = (from sq in db.QA_Test_Certificate_Masters
                      join l in db.Loading_Masters on sq.Loading_Slip_Ref equals l.Loading_Slip_No
                      where sq.Company_Id == logIn.company && sq.TC_NO == txtTCNo.Text 
                      orderby sq.id
                      select new
                      {
                          sq.TC_NO,
                          sq.TC_Date,
                          sq.Customer_ID,
                          sq.Fg_Item_ID,
                          sq.FG_Grade_ID,
                          sq.Inv_No,
                          sq.Inv_Qty,
                          sq.Loading_Slip_Ref,
                          sq.Consignee_ID,
                          sq.Created_By,
                          sq.Modified_By,                          
                          l.VehicleNo,
                          sq.Remarks
                          
                     
                          
                      }).ToList();
            if (sa.Count > 0)
            {
                txtInvNo.Text = sa[0].Inv_No;
                dpTCDate.Text = sa[0].TC_Date.Value.ToString();
                txtTCNo.Text = sa[0].TC_NO;
                //cmbMtrlGrade.SelectedValue = sa[0].FG_Grade_ID;
                cmbProduct.SelectedValue = sa[0].Consignee_ID;
                CmbBuyerName.SelectedValue = sa[0].Customer_ID;
                txtLoadingSlipNo.Text = sa[0].Loading_Slip_Ref;
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_By;
                txtVehicleNo.Text = sa[0].VehicleNo;
                txtComments.Text = sa[0].Remarks;
            }

            var dm2 = (from s in db.QA_Test_Certificate_Masters
                       join p in db.Products on s.Fg_Item_ID equals p.prod_ID
                       join m in db.QA_Mtrl_Grade_Masters on s.FG_Grade_ID equals m.id
                       where s.TC_NO == txtTCNo.Text
                       orderby s.FG_Lot_NO


                       select new

                       {
                           Prod_Id = s.Fg_Item_ID,
                           Prod_Name = p.Prod_Name,
                           Prod_Grade = m.Material_Grade,
                           TC_Qty = s.Inv_Qty,
                           FG_Batch_No = s.FG_Lot_NO,
                           Issue_Batch_Qty = s.FG_Lot_Qty_Issued,
                           RM_Lot_No = s.RM_Lot_No
                           //Batch_Qty = s.Batch_Qty,
                           //Batch_Qty_Balance = s.Batch_Qty_Balance,
                          




                       });




            SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dtr1 = new DataTable();
            da3.Fill(dtr1);
            if (dtr1.Rows.Count > 0)
                dgFgLotDetails.DataSource = dtr1;


            SqlCommand cmd5 = new SqlCommand("delete  from [Temp_FGLot_For_TC]", con);
            if (con.State != ConnectionState.Open)
                con.Open();

            cmd5.ExecuteNonQuery();

            SqlCommand cmd1 = con.CreateCommand();
            for (int i = 0; i < dgFgLotDetails.Rows.Count -1 ; i++)
            {
                //if (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value.ToString() != "")
                //{
                    decimal lotqty = (dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgFgLotDetails.Rows[i].Cells["Issue_Batch_Qty"].Value);

                    if (lotqty > 0)
                    {
                        cmd1.CommandText = "INSERT INTO Temp_FGLot_For_TC  (FG_Lot_No) VALUES  (@FGLotNo)";
                        string fglot = dgFgLotDetails.Rows[i].Cells["FG_Batch_No"].Value.ToString();
                        cmd1.Parameters.AddWithValue("@FGLotNo", fglot);
                        cmd1.ExecuteNonQuery();
                    }
                    cmd1.Parameters.Clear();
                //}
            }

            CallChemParamters();
            CallMechParamters();
        }
    }
}
