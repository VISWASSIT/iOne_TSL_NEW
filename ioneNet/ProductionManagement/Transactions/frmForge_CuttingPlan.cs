using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Ione_DAL;
using Syncfusion.Windows.Forms.Chart.SvgBase;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Diagnostics;

namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_CuttingPlan : Form
    {
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string FPress, MGroup;
        public static string MtrlGrade, RMWt, itemno, mono, rmsec,CQty,Prefno,mo_stage;

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public frmForge_CuttingPlan()
        {
            InitializeComponent();
        }

        private void frmForge_ProductionPlanning_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                      
            bindDroupDown_Lookup();
            AutoincrementId();

            if (CuttingPlansList.editMode == true)
            {
                txtSlipNo.Text = CuttingPlansList.SO_No;
                bindedit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("sp_Get_CuttingPlan", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);               
                cmd2.Parameters.AddWithValue("@pRef", txtForgePlanRefNo.Text);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                if (ds2.Rows.Count > 0)
                {
                    dgProducts.DataSource = ds2;
                }
                else
                {
                    MessageBox.Show("Either Invalid Ref No Entered  or Already Cuttng Plan Generated");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var d = (from data in db.SP_Forge_Get_CuttingPlanList (logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date ) select data).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["PlanningRef"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["PlanningRef"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["PlanningRef"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["PlanningRef"].FilterRowCondition = FilterRowCondition.Contains;

                  groupBox1.Visible = true;
                
            }
}

        private void sfButton1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PlanningRef"].MappingName;
                 var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {

                    txtSlipNo.Text = cellVaue.ToString();
                    bindedit();
                    groupBox1.Visible = false;
                    //SO_No = cellVaue;
                    //var = "0";
                    //editMode = true;
                    //MaterialManagement.Transactions.GoodsReceiptNote frm = new MaterialManagement.Transactions.GoodsReceiptNote();
                    ////OrderManagement.Transactions.
                    //frm.MdiParent = this.MdiParent;
                    //frm.Show();
                    ////FrmInv.ShowDialog();
                    ////i1 = 0;
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if ((from u in db.Forging_CuttingReport_Masters where u.Process == txtSlipNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    MessageBox.Show("Report Already Done, Cannot Be Deleted");
                    return;
                }
                else
                {

                    db.sp_Forge_Cutting_Plan_Delete(txtSlipNo.Text, logIn.company);
                    MessageBox.Show("Record Deleted Sucessfully");
                    clear();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbShift.Text == string.Empty)
                {
                    MessageBox.Show("Shift Should Not Be Empty", "Planning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbShift.Focus();
                    return;
                }
                else if (cmbMtrlGroup.Text == string.Empty)
                {
                    MessageBox.Show("Material Group  Should Not Be Empty");
                    cmbMtrlGroup.Focus();
                    return;
                }               
                if (dgProducts.Rows[0].Cells["MO_No"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Planning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Save();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "CuttingPlan.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();

                rep = new ProductionManagement.Transactions.rptCuttingPlan();




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


                rep.SetParameterValue("PlanningRef", txtSlipNo.Text);
                //rep.SetParameterValue("BU_ID", logIn.BU_ID);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.F2)
            {
                ioneNet.ProductionManagement.Transactions.frmRF_BloomAllotment form = new ioneNet.ProductionManagement.Transactions.frmRF_BloomAllotment();
               
                int i = dgProducts.CurrentCell.RowIndex;
                mono = dgProducts.Rows[i].Cells["MO_No"].Value.ToString();
                itemno = dgProducts.Rows[i].Cells["MO_Sno"].Value.ToString();
                rmsec = dgProducts.Rows[i].Cells["RM_Sec"].Value.ToString();
                MtrlGrade = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                RMWt = dgProducts.Rows[i].Cells["RM_WT"].Value.ToString();
                CQty = dgProducts.Rows[i].Cells["BalQty"].Value.ToString();
                mo_stage = dgProducts.Rows[i].Cells["stage"].Value.ToString();
                    Prefno = txtSlipNo.Text;

                form.ShowDialog();
                string RFSel = ioneNet.ProductionManagement.Transactions.frmRF_BloomAllotment.rfnos;
                    if (RFSel != null)
                    {
                        dgProducts.Rows[i].Cells["RF_No"].Value = RFSel;//  ioneNet.ProductionManagement.Transactions.frmRF_BloomAllotment.rfnos.ToString();
                    }
            }
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

                var result = db.Sp_autoincrement_Forging_Cutting_Plan(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtSlipNo.Text = result.FirstOrDefault().Report_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindDroupDown_Lookup()
        {
            try
            {



                //Purchase Basis
                var PurBasis = (from m in db.Attributes_Datas where m.Head_Name == "MtrlType" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PurBasis.Count > 0)
                {
                    cmbMtrlGroup.DataSource = PurBasis;
                    cmbMtrlGroup.ValueMember = "ID";
                    cmbMtrlGroup.DisplayMember = "Descr";
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

        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSlipNo.Text;
                if (txtSlipNo.Text != "")
                //if ((from u in db.GoodsReceiptNote_Masters where u.Grn_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Forge_Cutting_Plan_Delete(myString, logIn.company);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSlipNo.Text;

                }

                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Forge_Cutting_Plan SC = new Forge_Cutting_Plan();
                    SC.PlanningRef = myString;
                    SC.PlanningRefDate = dpSODate.Value;
                    SC.Prod_Shift = cmbShift.Text;                  
                    SC.Material_Type = Convert.ToInt32(cmbMtrlGroup.SelectedValue);
                    SC.MO_Sno = (dgProducts.Rows[i].Cells["MO_Sno"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["MO_Sno"].Value);
                    SC.Mo_No = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                    SC.Forging_Plan_No = txtForgePlanRefNo.Text;
                    var d1 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == dgProducts.Rows[i].Cells["Mo_No"].Value.ToString() && a.Company_ID == logIn.company select new { a.id }).ToList();
                    SC.MO_Master_ID = d1[0].id;


                    //SC.Mo_Date = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_No"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_No"].Value);

                    //SC.Item_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.MaterialGrade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Qty = (dgProducts.Rows[i].Cells["BalQty"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["BalQty"].Value);
                    SC.RMSec = (dgProducts.Rows[i].Cells["RM_Sec"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["RM_Sec"].Value);
                    SC.RM_Length = (dgProducts.Rows[i].Cells["RM_Length"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["RM_Length"].Value);
                    SC.RM_WT = (dgProducts.Rows[i].Cells["RM_WT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["RM_WT"].Value);
                    SC.Stage = (dgProducts.Rows[i].Cells["stage"].Value == null) ? "" : (dgProducts.Rows[i].Cells["stage"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Company_ID = logIn.company;
                    SC.Created_By = lblCreatedBy.Text;
                    SC.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Forge_Cutting_Plans.InsertOnSubmit(SC);
                }
                db.SubmitChanges();

                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
                //clear();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindedit()
        {
            try
            {

                int QuoteMasterID = 0;
                var da = (from obj in db.Forge_Cutting_Plans
                          where obj.PlanningRef == txtSlipNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    QuoteMasterID = da[0].id;
                    dpSODate.Text = da[0].PlanningRefDate.ToString();

                    cmbShift.Text = da[0].Prod_Shift;
                    //cmbForgingPress.SelectedValue = da[0].Prod_Machine;
                    cmbMtrlGroup.SelectedValue = da[0].Material_Type;
                    txtForgePlanRefNo.Text = da[0].Forging_Plan_No;
                    // cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                }


                var dm1 = (from s in db.Forge_Cutting_Plans
                           join m in db.Forge_MFG_Order_Masters on s.Mo_No equals m.MO_No
                           join c in db.Supplier_informations on m.Customer_Name equals c.ID
                           join f in db.Forge_Mfg_Order_Childs on new {s.MO_Master_ID, s.MO_Sno} equals new {f.MO_Master_ID, MO_Sno = f.Prod_Code}
                           join b in db.RF_Cutting_RM_Allotments on new { s.Mo_No, s.MO_Sno, s.PlanningRef }
                           equals new { Mo_No= b.MO_No, MO_Sno = b.Item_No, PlanningRef = b.Plan_Ref_No } into rfnos from rfno in rfnos.DefaultIfEmpty()
                          
                           where s.PlanningRef == txtSlipNo.Text && s.Company_ID == logIn.company
                            

                           //var QSOuterJoin = from emp in Employee.GetAllEmployees()
                           //                  join add in Address.GetAddress()
                           //                  on emp.AddressId equals add.ID
                           //                  into EmployeeAddressGroup
                           //                  from address in EmployeeAddressGroup.DefaultIfEmpty()
                           //                  select new { emp, address };

                select new

                           {
                               s.Mo_No,                             
                               Customer_Name =  c.Supplier_Name,
                               s.MO_Sno,
                               Item_No = s.Prod_Code,
                               Item_Description = f.Product_Description +'-'+f.Prod_Grade,
                               Item_Grade = s.MaterialGrade,
                               RM_Sec = s.RMSec,
                               s.RM_Length,
                               RM_WT = s.RM_WT,
                               BalQty =rfno.RM_Cut_Qty,
                               RF_No = rfno.RF_No,
                               stage = s.Stage,
                               s.Remarks
                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AutoincrementId();
                cmbShift.Text = "";
                cmbMtrlGroup.Text = "";
                txtForgePlanRefNo.Text = "";               
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


                if (dgProducts.Rows.Count > 0)
                {
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        dgProducts.Rows.RemoveAt(i);
                        i--;
                        while (dgProducts.Rows.Count == 0)
                            continue;
                    }
                }
                //if (dgSelectedocument.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dgSelectedocument.Rows.Count - 1; i++)
                //    {
                //        dgSelectedocument.Rows.RemoveAt(i);
                //        i--;
                //        while (dgSelectedocument.Rows.Count == 0)
                //            continue;
                //    }
                //}
                //txtTotalAmt.Text = "";
                //txtTotalReceivedAmount.Text = "";

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message + " While  clear Getting Error ", "PaymentVoucher", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
