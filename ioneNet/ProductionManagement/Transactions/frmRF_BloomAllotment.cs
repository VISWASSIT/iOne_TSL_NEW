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
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using Syncfusion.Windows.Forms.Tools;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmRF_BloomAllotment : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string rfnos;
        public frmRF_BloomAllotment()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRF_BloomAllotment_Load(object sender, EventArgs e)
        {
            try
            {
                txtMONo.Text = frmForge_CuttingPlan.mono;
                txtQty.Text = frmForge_CuttingPlan.CQty;
                txtItemCode.Text = frmForge_CuttingPlan.itemno;
                string MtrlGrade = frmForge_CuttingPlan.MtrlGrade;
               // txtMOStage.Text = frmForge_CuttingPlan.mo_stage;
                var pscrap = (from m in db.sp_Get_RM_Sections (MtrlGrade) select new { m.Prod_Name }).Distinct().ToList();
                if (pscrap.Count > 0)
                {
                    multiSelectionComboBox1.DataSource = pscrap;
                    multiSelectionComboBox1.ValueMember = "Prod_Name";
                    multiSelectionComboBox1.DisplayMember = "Prod_Name";
                    multiSelectionComboBox1.SelectedIndex = -1;
                }
                var RMSec = (from m in db.Forge_Mfg_Order_Childs
                             join p in db.Products on m.RM_Sec_ID equals p.prod_ID
                             join mo in db.Forge_MFG_Order_Masters on m.MO_Master_ID equals mo.id
                             where mo.MO_No == txtMONo.Text && m.Company_ID == logIn.company select new { p.Prod_Name }).Distinct().ToList();
                if (RMSec.Count > 0)
                {
                    if (RMSec[0].Prod_Name != null)
                    {
                        string MP = RMSec[0].Prod_Name.ToString();
                        //multiSelectionComboBox1.Text = MP;
                        string[] values = MP.Split(',');



                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            multiSelectionComboBox1.AddVisualItem(m);

                            //   multiSelectionComboBox1.
                            //multiSelectionComboBox1.Text = m;

                        }
                    }
                }

                //Retrive if data available
                var dm1 = (from s in db.RF_Cutting_RM_Allotments
                           where s.Plan_Ref_No == frmForge_CuttingPlan.Prefno && s.MO_No == txtMONo.Text && 
                           s.Item_No == Convert.ToInt32(txtItemCode.Text) &&   s.company_ID == logIn.company && s.Remarks == frmForge_CuttingPlan.mo_stage

                           select new

                           {
                               s.RM_Sec,
                               RollNo= s.RF_No,
                               Bal_Length= s.RM_Stock_Length,
                               CutLength = s.RM_Cut_Length,
                               Cut_Qty = s.RM_Cut_Qty,
                               s.Remarks
                              
                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    grdBloomData.DataSource = dtr;
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
                string RMData = "";
            foreach (VisualItem obj in this.multiSelectionComboBox1.VisualItems)
            {

                if (RMData != "")
                {

                    RMData = RMData + "," + obj.Text;
                }
                else
                {

                    RMData = obj.Text;
                }
            }
            SqlCommand cmd2 = new SqlCommand("sp_Get_BloomFor_Allotment_New", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@mono", txtMONo.Text);
            cmd2.Parameters.AddWithValue("@itemno",Convert.ToInt32(txtItemCode.Text));
            cmd2.Parameters.AddWithValue("@rm_Sec", RMData);
            cmd2.Parameters.AddWithValue("@MtrlGrade", frmForge_CuttingPlan.MtrlGrade);
            cmd2.Parameters.AddWithValue("@rmwt", frmForge_CuttingPlan.RMWt);
            if (checkBox1.Checked)
            {
                cmd2.Parameters.AddWithValue("@rmshape", "Round");
            }
            else
            {
                cmd2.Parameters.AddWithValue("@rmshape", "Square");
            }
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            grdBloomData.DataSource = ds2;
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                db.Sp_delete_RF_Bloom_Allotement(logIn.company,frmForge_CuttingPlan.Prefno,txtMONo.Text, Convert.ToInt32(txtItemCode.Text));
                rfnos = "";
                for (int i = 0; i < grdBloomData.RowCount - 1; i++)
                {
                    if (Convert.ToDecimal(grdBloomData.Rows[i].Cells["Cut_Qty"].Value)>0)
                    {
                        RF_Cutting_RM_Allotment SC = new RF_Cutting_RM_Allotment();
                        SC.Plan_Ref_No = frmForge_CuttingPlan.Prefno;
                        SC.MO_No = txtMONo.Text;
                        SC.Item_No = Convert.ToInt32(txtItemCode.Text);
                        SC.RM_Sec = (grdBloomData.Rows[i].Cells["RM_Sec"].Value == null) ? "" : (grdBloomData.Rows[i].Cells["RM_Sec"].Value).ToString();
                        SC.RF_No = (grdBloomData.Rows[i].Cells["RollNo"].Value == null) ? "" : (grdBloomData.Rows[i].Cells["RollNo"].Value).ToString();
                        if (rfnos == "")
                        {
                            rfnos = grdBloomData.Rows[i].Cells["RollNo"].Value.ToString();
                        }
                        else
                        {
                            rfnos = rfnos + "," + grdBloomData.Rows[i].Cells["RollNo"].Value.ToString();
                        }
                        SC.RM_Stock_Length = (grdBloomData.Rows[i].Cells["Bal_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdBloomData.Rows[i].Cells["Bal_Length"].Value);
                        SC.RM_Cut_Length = (grdBloomData.Rows[i].Cells["CutLength"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdBloomData.Rows[i].Cells["CutLength"].Value);
                        SC.RM_Cut_Qty = (grdBloomData.Rows[i].Cells["Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdBloomData.Rows[i].Cells["Cut_Qty"].Value);
                        SC.Remarks = frmForge_CuttingPlan.mo_stage; // (grdBloomData.Rows[i].Cells["Remarks"].Value == null) ? "" : (grdBloomData.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.company_ID = logIn.company;
                        db.RF_Cutting_RM_Allotments.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdBloomData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = grdBloomData.Rows[grdBloomData.CurrentRow.Index];
                int columnIndex = grdBloomData.CurrentCell.ColumnIndex;
                string columnName = grdBloomData.Columns[columnIndex].Name;

                decimal Qty_Available = (R1.Cells["Bal_Length"].Value == "" || R1.Cells["Bal_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Bal_Length"].Value);
                decimal Cut_Length = (R1.Cells["CutLength"].Value == "" || R1.Cells["CutLength"].Value == null || R1.Cells["CutLength"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["CutLength"].Value);
                decimal Cut_Qty = (R1.Cells["Cut_Qty"].Value == "" || R1.Cells["Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Cut_Qty"].Value);
                decimal tot_Length = Cut_Length * Cut_Qty;
                if(tot_Length>Qty_Available)
                {
                    MessageBox.Show("Sufficiant Stock Not Available to Allot the Entered Qty");
                    R1.Cells["Cut_Qty"].Value = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
            
    }
}
