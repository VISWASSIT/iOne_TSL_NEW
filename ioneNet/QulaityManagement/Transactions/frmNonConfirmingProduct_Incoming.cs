using Ione_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class frmNonConfirmingProduct_Incoming: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string GRN_NO, ItemCode, RFNo, MtrlGrade, reportNo;

        private void txtIntLotNo_Leave(object sender, EventArgs e)
        {
            if(txtIntLotNo.Text !="")
            {
                
                BindEdit();
            }
        }

        public frmNonConfirmingProduct_Incoming()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ReadingData = "";
                if ((from a in db.QA_Non_Conformance_Report_Incomings where a.Company_Id == logIn.company && a.NCP_Report_No == txtInvNo.Text select a).Count() > 0)
                {
                    db.Sp_Delete_NCP_Incoming_Report(logIn.company, txtInvNo.Text);
                }

                for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    string s = dataGridView1.Rows[i].Cells["Sample_ID"].Value.ToString();
                    if (s.Length > 0)
                    {


                        QA_Non_Conformance_Report_Incoming pb = new QA_Non_Conformance_Report_Incoming();
                        pb.NCP_Report_No = txtInvNo.Text;
                        pb.NCP_Date = dpInvDate.Value;
                        pb.Int_Lot_No = txtIntLotNo.Text;
                        pb.Disposal_Action_Authorized_By = txtApprovedBy.Text;
                        pb.NCP_Stage = "Incoming";
                        pb.Sample_ID = dataGridView1.Rows[i].Cells["Sample_ID"].Value.ToString();
                        //for (int j = 1; j < dataGridView1.Columns.Count - 2; j++)
                        //{
                        //    if (ReadingData != "")
                        //    {
                        //        ReadingData = ReadingData + ',' + Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        //    }
                        //    else
                        //    {
                        //        ReadingData = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        //    }
                        //}
                        ////pb.Chemical_Readings = ReadingData;

                        //ReadingData = "";
                        pb.Accepted_Grade = dataGridView1.Rows[i].Cells["Accepted_Grade"].Value.ToString();
                        pb.New_Batch_No = dataGridView1.Rows[i].Cells["Batch_No"].Value.ToString(); ;

                        pb.Company_Id = logIn.company;
                        pb.Created_By = lblCreatedBy.Text;
                        pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.QA_Non_Conformance_Report_Incomings.InsertOnSubmit(pb);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {


                db.Sp_Delete_NCP_Incoming_Report(logIn.company, txtInvNo.Text);
                MessageBox.Show("Record Deleted Successfully");
                this.Close();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Get Standard Spec
            var S = (from a in db.QA_Mtrl_Grade_Test_Specs
                     join m in db.QA_Mtrl_Grade_Masters on a.Mtrl_Grade_Id equals m.id
                     join p in db.QA_Test_Parameters on a.Parameter_ID equals p.id

                     where a.Company_id == logIn.company && m.Material_Grade == txtMtrlGrade.Text && p.id == 1
                     select new { a.Spec_Min, a.Spec_Max }).ToList();
            for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
            {
                for (int j = 1; j < dataGridView1.Columns.Count - 2; j++)
                {
                    decimal aread = Convert.ToDecimal(dataGridView1.Rows[i].Cells[j].Value);
                    decimal CERead = Convert.ToDecimal(dataGridView1.Rows[i].Cells["CE"].Value);
                    decimal aspec = Convert.ToDecimal(dataGridView1.Rows[1].Cells[j].Value);
                    decimal CESPec = Convert.ToDecimal(dataGridView1.Rows[0].Cells["CE"].Value);

                    string CarbonSpec = "0.26";
                    if (j == 1)
                    {
                        if (aread > Convert.ToDecimal(CarbonSpec))
                        {
                            dataGridView1.Rows[i].Cells["Accepted_Grade"].Value = "Comm Gade";
                            dataGridView1.Rows[i].Cells["Batch_No"].Value = txtIntLotNo.Text + "/1";
                        }
                        else
                        {
                            if(CERead> CESPec)
                            {
                                dataGridView1.Rows[i].Cells["Accepted_Grade"].Value = "Comm Gade";
                                dataGridView1.Rows[i].Cells["Batch_No"].Value = txtIntLotNo.Text + "/1";
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                                dataGridView1.Rows[i].Cells["Batch_No"].Value = txtIntLotNo.Text;

                            }
                        }
                    }
                }
            }
        }

        private void frmNonConfirmingProduct_Incoming_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtInspectedBy.Text = logIn.username;
            txtApprovedBy.Text = logIn.username;

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
            if (frmQA_InomingInspReportList.Report_No !="")
            {
                txtIntLotNo.Text = frmQA_InomingInspReportList.Report_No;
                txtIntLotNo_Leave(sender, e);
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


            var sa = (from sq in db.Incoming_Chemical_Reports
                      join g in db.GoodsReceiptNote_Masters on sq.GRN_Master_Id equals g.Id
                      join gc in db.GoodsReceiptNote_Childs on g.Id equals gc.GRN_Master_ID
                      join su in db.Supplier_informations on g.Purchase_Account equals su.ID
                      join p in db.Products on gc.Prod_Code equals p.prod_ID
                      where sq.Company_Id == logIn.company && sq.Int_Lot_No == txtIntLotNo.Text
                      && sq.Accepted_Grade !=  gc.Prod_Grade
                      orderby sq.id
                      select new
                      {
                          sq.Supplier_Heat_No,
                          sq.Supplier_TC_No,
                          sq.No_Of_Pcs,
                          sq.Inspected_By,
                          sq.Approved_By,
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

                txtGRNNo.Text = sa[0].Grn_NO;
                txtProdName.Text = sa[0].Prod_Name.ToString();
                txtSuppName.Text = sa[0].Supplier_Name;
                txtMtrlGrade.Text = sa[0].Prod_Grade;
                txtIntLotNo.Text = sa[0].Int_Batch_No;
                txtSupplierHeat.Text = sa[0].Supplier_Heat_No;
                txtSupplierTCno.Text = sa[0].Supplier_TC_No;
                txtNoOfPcs.Text = sa.Count.ToString();
                txtInspectedBy.Text = sa[0].Inspected_By;
                //txtApprovedBy.Text = sa[0].Approved_By;
                txtComments.Text = sa[0].Comments_Remarks;
                cmbStatus.SelectedValue = sa[0].Result;
                

                txtDocPath.Text = sa[0].Doc_Link;
                txtSuppInvNo.Text = sa[0].Supplier_InvNo;
                
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
                int ColIndex = 0;
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

                        dr[i + 1] = p;
                        ColIndex = i + 1;

                    }
                    dr[gridcolcount - 2] = sa[j].Accepted_Grade;
                    dr[gridcolcount - 1] = sa[j].New_Batch_No;

                    dt.Rows.Add(dr);
                }
                dataGridView1.DataSource = dt;

                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.DarkGray;

                decimal ActReading = 0;
                decimal StdSpec = 0;
                for (int i = 1; i < dataGridView1.ColumnCount - 3; i++)
                {
                    for (int r = 0; r < dataGridView1.RowCount - 1; r++)
                    {
                        //dataGridView1.Rows[r].Cells[i].Value.ToString();
                        ActReading = (dataGridView1.Rows[r].Cells[i].Value == DBNull.Value || dataGridView1.Rows[r].Cells[i].Value == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[r].Cells[i].Value);
                        StdSpec = (dataGridView1.Rows[0].Cells[i].Value == DBNull.Value || dataGridView1.Rows[0].Cells[i].Value == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells[i].Value);

                        if (ActReading > StdSpec)
                        {
                            dataGridView1.Rows[r].Cells[i].Style.ForeColor = Color.Red;
                            //R1.Cells["Accepted_Grade"].Value = "";
                        }
                        else
                        {
                            dataGridView1.Rows[r].Cells[i].Style.ForeColor = Color.Black;
                            //R1.Cells["Accepted_Grade"].Value = txtMtrlGrade.Text;
                        }
                    }
                }

            }
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_NCP_IncomingInspReport(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtInvNo.Text = result.FirstOrDefault().Report_No;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

}
