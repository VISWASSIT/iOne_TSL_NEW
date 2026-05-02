using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using ioneNet.ProductionManagement.Transactions;
using ioneNet.Qulaity_Management.Transactions;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
using Syncfusion.Windows.Forms.Tools.Win32API;
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
using ZXing;

namespace ioneNet.QulaityManagement.Transactions
{
    public partial class frmListOfTestCertificates : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path,  Mgrade;
        public static string TC_No;
        public frmListOfTestCertificates()
        {
            InitializeComponent();
        }

        private void frmListOfTestCertificates_Load(object sender, EventArgs e)
        {
            BindTClist();
        }

        public void BindTClist()
        {
            try
            {
                var d = (from data in db.ShowTCList(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date,logIn.BU_ID) select (data)).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    sfDataGrid1.Columns["Customer"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Customer"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Customer"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Customer"].FilterRowCondition = FilterRowCondition.Contains;
                    sfDataGrid1.Columns["TC_NO"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["TC_NO"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["TC_NO"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["TC_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;

                    sfDataGrid1.Columns["Inv_No"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Inv_No"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Inv_No"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Inv_No"].FilterRowCondition = FilterRowCondition.Contains;

                    sfDataGrid1.Columns["Batch_No"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Batch_No"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Batch_No"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Batch_No"].FilterRowCondition = FilterRowCondition.Contains;

                }


                // sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;



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
            ioneNet.QulaityManagement.Transactions.frmTestCertificate frm = new frmTestCertificate();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

            BindTClist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindTClist();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
                //var prodname = sfDataGrid1.Columns["TC_NO"].MappingName;
                var MtrlGrade = sfDataGrid1.Columns["Material_Grade"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellVaue1 = (rowData.GetType().GetProperty(MtrlGrade).GetValue(rowData, null).ToString());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                TC_No = cellVaue.ToString();
                Mgrade = cellVaue1.ToString();

                path = Path.Combine(Directory.GetCurrentDirectory(), "Test_Certificate.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new QulaityManagement.Transactions.TestCertificate();


                SqlCommand cmd5 = new SqlCommand("delete  from [Temp_FGLot_For_TC]", con);
                if (con.State != ConnectionState.Open)
                    con.Open();

                cmd5.ExecuteNonQuery();

                //SqlCommand cmd1 = con.CreateCommand();
                
                    
                cmd1.CommandText = "INSERT INTO Temp_FGLot_For_TC   select FG_Lot_NO from QA_Test_Certificate_Master where TC_No = @tcno";
                
                cmd1.Parameters.AddWithValue("@tcno", TC_No);
                
                cmd1.ExecuteNonQuery();
                    

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
                rep.RecordSelectionFormula = "{QA_Test_Certificate_Master.TC_NO} = '" + TC_No + "'";


                //rep.Subreports[0].SetParameterValue(0, logIn.company);
                //rep.Subreports[0].SetParameterValue(1, cmbMtrlGrade.Text);
                rep.SetParameterValue("@CompName", logIn.company, rep.Subreports[0].Name.ToString());
               // rep.SetParameterValue("@mtrlgrade", Mgrade, rep.Subreports[0].Name.ToString());
                rep.SetParameterValue("@CompName", logIn.company, rep.Subreports[1].Name.ToString());
                //rep.SetParameterValue("@mtrlgrade", Mgrade, rep.Subreports[1].Name.ToString());
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

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
            //var prodname = sfDataGrid1.Columns["TC_NO"].MappingName;
            var MtrlGrade = sfDataGrid1.Columns["Material_Grade"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(MtrlGrade).GetValue(rowData, null).ToString());
            TC_No = cellVaue;
            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Test Certificate" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
            if (uRole.Count > 0)
            {
                if (uRole[0].Modify_Role == true)
                {
                   
                    editMode = true;
                    sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                    frmTestCertificate frm = new frmTestCertificate();
                    //OrderManagement.Transactions.
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;


                }
                else
                {
                    MessageBox.Show("You Have No Permissions to Modify The Test Certificate");
                    return;
                }

            }

        }

        private void CallChemParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_ChemicalTest_Parameters_For_TC", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //cmd2.Parameters.AddWithValue("@mtrlgrade", Mgrade);

            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            //dgChemData.DataSource = ds2;
            //dgChemData.Rows[0].Cells["Chem_FG_Batch_No"].Value = "Spec";
            //dgChemData.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
            //var prodname = sfDataGrid1.Columns["TC_NO"].MappingName;
            var MtrlGrade = sfDataGrid1.Columns["Material_Grade"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(MtrlGrade).GetValue(rowData, null).ToString());
            TC_No = cellVaue;
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                if ((from a in db.QA_Test_Certificate_Masters where a.Company_Id == logIn.company && a.TC_NO == TC_No select a).Count() > 0)
                {
                    db.Sp_Delete_TestCertificate(logIn.company, TC_No);
                }

                MessageBox.Show("Record Deleted Successfully");
                BindTClist();

            }
        }

        private void printLablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            //if (i >= 0)
            //{
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            
            var mappingName3 = sfDataGrid1.Columns["Batch_No"].MappingName;
            var cellVaue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            label1.Text = cellVaue3;
            groupBox1.Visible = true;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if(txtNoofLables.Text == string.Empty)
            {
                MessageBox.Show("Enter No of Lables to Print");
                txtNoofLables.Focus();
                return;
            }
            int i = sfDataGrid1.CurrentCell.RowIndex;
            //if (i >= 0)
            //{
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid1.Columns["TC_NO"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var mappingName1 = sfDataGrid1.Columns["Prod_Name"].MappingName;
            var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            var mappingName2 = sfDataGrid1.Columns["Material_Grade"].MappingName;
            var cellVaue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            var mappingName3 = sfDataGrid1.Columns["Batch_No"].MappingName;
            var cellVaue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            string Report_No = cellVaue;


            //var sa = (from sq in db.QA_Test_Certificate_Masters
            //          join g in db.Products on sq.Fg_Item_ID equals g.prod_ID

            //          join su in db.QA_Mtrl_Grade_Masters on sq.FG_Grade_ID equals su.id

            //          where sq.Company_Id == logIn.company && sq.TC_NO == Report_No
            //          orderby sq.id
            //          select new
            //          {
            //              sq.FG_Lot_NO,
            //              g.Prod_Name,
            //              su.Material_Grade,
            //              sq.RM_Lot_No,

            //          }).ToList();
            //if (sa.Count > 0)
            //{

            //    //dpInvDate.Text = sa[0].Insp_Date.Value.ToString();
            //    //txtProdName.Text = sa[0].Prod_Name.ToString();
            //    //txtMtrlGrade.Text = sa[0].Accepted_Grade;
            //    //txtIntLotNo.Text = sa[0].New_Batch_No;
            //    //txtSupplierHeat.Text = sa[0].Supplier_Heat_No;
            //    //txtSupplierTCno.Text = sa[0].Supplier_TC_No;

            //}


            SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde]", con);
            if (con.State != ConnectionState.Open)
                con.Open();
            //cmd5.Parameters.AddWithValue("@comp", logIn.company);
            cmd5.ExecuteNonQuery();



            int lc = Convert.ToInt32(txtNoofLables.Text);
            for (int j = 0; j < lc - 1; j++)
            {


                string QRCODE = "";

                QRCODE = "Batch No :" + cellVaue3 + " Product :" + cellVaue1 + " Prod Grade:" + cellVaue2;

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
                cmd1.Parameters.AddWithValue("@Inv_No", cellVaue3);
                cmd1.ExecuteNonQuery();

                //pval = "";


            }

            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            //rep = new ioneNet.Qulaity_Management.Transactions.QRCodePrint_RM();
            rep = new ioneNet.Reports.rptPrintBarCode_FG();
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), "FG_QR_Label.pdf");
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



            string FG_Name = "Product :" + cellVaue1;
            string FG_Grade = "Grade :" + cellVaue2;
            rep.SetParameterValue("FG_Name", FG_Name);
            rep.SetParameterValue("FG_Grade", FG_Grade);


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

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void CallMechParamters()
        {
            SqlCommand cmd2 = new SqlCommand("Sp_QA_Get_MechanicalTest_Parameters_For_TC", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //cmd2.Parameters.AddWithValue("@mtrlgrade", Mgrade);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            //dgMechData.DataSource = ds2;
            //dgChemData.Rows[0].Cells["Chem_FG_Batch_No"].Value = "Spec";
            //dgChemData.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;


        }
    }
}
