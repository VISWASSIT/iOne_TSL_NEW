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
using System.Web.Services.Description;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using OpenCvSharp;
using Syncfusion.Windows.Forms.Tools.Win32API;
using Syncfusion.WinForms.DataGrid;
using ZXing;

namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class FrmPrintQRCode_Lables : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public FrmPrintQRCode_Lables()
        {
            InitializeComponent();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            try
            {
                var Buyerblind = (from m in db.Products where m.Company_ID == logIn.company && m.Prod_Type_Id == 140 select new { m.prod_ID, m.Prod_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbProdName.DataSource = Buyerblind;
                    cmbProdName.ValueMember = "prod_ID";
                    cmbProdName.DisplayMember = "Prod_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //CmbBuyerName.SelectedIndex = -1;

            

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmPrintQRCode_Lables_Load(object sender, EventArgs e)
        {
            try
            {
               
               


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //string QRCODE = "";

            ////if (comboBox1.Text == "RAW MATERIAL")
            ////{
            ////    QRCODE = "RM Lot No :" + txtRMLotNoEntry.Text + " Receipt Date : " + dtpReceipt.Text + " Product :" + comboBox2.Text + " Prod Grade:" + textBox1.Text;
            ////}
            ////else
            ////{
            //    QRCODE = "FG Lot No :" + txtFGLotNo.Text + " Prod Date : " + dtpProdDate.Text + " Product :" + comboBox2.Text + " Prod Grade:" + textBox1.Text + " RM Lot No:" + txtRMLotNo_FGlink.Text;

            ////}

            //IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
            //var result = writer.Write(QRCODE);
            //var barcodeBitmap = new Bitmap(result);
            //pictureBox1.Image = barcodeBitmap;
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtNoofLables.Text == string.Empty)
            {
                MessageBox.Show("Enter No of Lables to Print");
                txtNoofLables.Focus();
                return;
            }


            string Report_No = txtFGLotNo.Text;


            //var sa = (from sq in db.QA_Test_Certificate_Masters
            //          join g in db.Products on sq.Fg_Item_ID equals g.prod_ID

            //          join su in db.QA_Mtrl_Grade_Masters on sq.FG_Grade_ID equals su.id

            //          where sq.Company_Id == logIn.company && sq.FG_Lot_NO == Report_No
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


            //    cmbProdName.Text = sa[0].Prod_Name.ToString();
            //    txtGrade.Text = sa[0].Material_Grade;
            //    txtRMLotNo.Text = sa[0].RM_Lot_No;
               

            //}


            SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde]", con);
            if (con.State != ConnectionState.Open)
                con.Open();
            //cmd5.Parameters.AddWithValue("@comp", logIn.company);
            cmd5.ExecuteNonQuery();



            int lc = Convert.ToInt32(txtNoofLables.Text);
            for (int j = 0; j < lc-1; j++)
            {


                string QRCODE = "";

                QRCODE = "Batch No :" + txtFGLotNo.Text + " Product :" + cmbProdName.Text + " Prod Grade:" + txtGrade.Text;

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
                cmd1.Parameters.AddWithValue("@Inv_No", txtFGLotNo.Text);
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

            string FG_Name = "Product :" + cmbProdName.Text;
            string FG_Grade = "Grade :" + txtGrade.Text;
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

        private void txtFGLotNo_Leave(object sender, EventArgs e)
        {
            try
            {
                string Report_No = txtFGLotNo.Text;


                var sa = (from sq in db.QA_Test_Certificate_Masters
                          join g in db.Products on sq.Fg_Item_ID equals g.prod_ID

                          join su in db.QA_Mtrl_Grade_Masters on sq.FG_Grade_ID equals su.id

                          where sq.Company_Id == logIn.company && sq.FG_Lot_NO == Report_No
                          orderby sq.id
                          select new
                          {
                              sq.FG_Lot_NO,
                              g.Prod_Name,
                              su.Material_Grade,
                              sq.RM_Lot_No,

                          }).ToList();
                if (sa.Count > 0)
                {


                    cmbProdName.Text = sa[0].Prod_Name.ToString();
                    txtGrade.Text = sa[0].Material_Grade;
                    txtRMLotNo.Text = sa[0].RM_Lot_No;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
