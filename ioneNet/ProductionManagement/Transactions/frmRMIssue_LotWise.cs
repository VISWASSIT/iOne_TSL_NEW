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

namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmRMIssue_LotWise: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmRMIssue_LotWise()
        {
            InitializeComponent();
        }

        private void frmRMIssue_LotWise_Load(object sender, EventArgs e)
        {
            try
            {

            txtReportNo.Text = ProductionManagement.Transactions.frm_Production_Report.ReportNo;
            txtItemCode.Text = ProductionManagement.Transactions.frm_Production_Report.ItemCode;
            txtTotQtyFinished.Text = ProductionManagement.Transactions.frm_Production_Report.RecQty;
            txtItemName.Text = ProductionManagement.Transactions.frm_Production_Report.ItemName;
            txtGrade.Text  = ProductionManagement.Transactions.frm_Production_Report.MtrlGrade;
            dpPDate.Value = ProductionManagement.Transactions.frm_Production_Report.pdate;
                txtPartyName.Text = ProductionManagement.Transactions.frm_Production_Report.conv_party.ToString();
                var dm1 = (from s in db.Production_report_rolling_Childs
                       join p in db.Products on s.Prod_ID equals p.prod_ID
                       join g in db.QA_Mtrl_Grade_Masters on s.FG_Grade_ID equals g.id
                       //join i in db.Incoming_Chemical_Reports on  s.RM_Lot_No equals i.New_Batch_No
                       where s.PR_No == txtReportNo.Text && s.Company_Id == logIn.company && s.FG_Prod_ID == Convert.ToInt32(txtItemCode.Text)
                       && g.Material_Grade == txtGrade.Text
                       orderby s.ID

                       select new

                       {
                           s.RM_Lot_No,
                           RM_Size = p.Prod_Name,
                           Prod_ID = p.prod_ID,                          
                           RM_Grade = g.Material_Grade,                          
                           s.RM_Lot_Qty,
                           RM_Issued = s.RM_iss_Qty,
                           RM_E_cut = s.Rm_Cut_Qty,
                           RM_G_Loss = s.GC_Loss,

                       });

            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
                if (dtr.Rows.Count > 0)
                {
                    dgrmconsumed.DataSource = dtr;
                    decimal y = 0;
                    decimal q = 0;
                    decimal v = 0;
                    for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                    {
                        y += (dgrmconsumed.Rows[i].Cells["RM_Issued"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Issued"].Value);
                        q += (dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value);
                        v += (dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value);


                    }
                    txtTotalRMQty.Text = (y - q - v).ToString();
                }
                else
                {
                    string pd = dpPDate.Value.ToString("yyyy-MM-dd");

                   
                    SqlCommand cmd3 = new SqlCommand("SP_Get_RM_Alloted_Qty", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@compname", logIn.company);
                    cmd3.Parameters.AddWithValue("@prod_Date", Convert.ToDateTime(pd));
                    cmd3.Parameters.AddWithValue("@prod_id", Convert.ToInt32(txtItemCode.Text));
                    cmd3.Parameters.AddWithValue("@prod_Grade", txtGrade.Text);
                    cmd3.Parameters.AddWithValue("@partyname", Convert.ToInt32(txtPartyName.Text));

                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    //DataSet ds2 = new DataSet();
                    DataTable ds3 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da3.Fill(ds3);
                    dgrmconsumed.DataSource = ds3;
                 
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgrmconsumed_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];
                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
                if (columnName == "RM_Lot_No")
                {
                    if (frm_Production_Report.convprod == false)
                    {
                        var S = (from a in db.SP_Get_RM_Alloted_Qty(logIn.company, dpPDate.Value)
                                 where a.RM_Lot_No == R1.Cells["RM_Lot_No"].Value.ToString()
                                 select new { a.RM_Size, a.Lot_Bal_Qty, a.RM_Grade, a.Prod_ID }).ToList();
                        if (S.Count > 0)
                        {

                            R1.Cells["Prod_ID"].Value = S[0].Prod_ID.ToString();
                            R1.Cells["RM_Size"].Value = S[0].RM_Size.ToString();
                            R1.Cells["RM_Grade"].Value = S[0].RM_Grade.ToString();
                            R1.Cells["RM_Lot_Qty"].Value = S[0].Lot_Bal_Qty.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Lot No Entered or Lot Not Cleared by QA inspection");
                            R1.Cells[columnIndex].Value = "";
                            return;
                        }
                    }
                    else
                    {
                        var S = (from a in db.SP_Get_RM_Lot_Qty_Conversion(logIn.company, R1.Cells["RM_Lot_No"].Value.ToString(), txtGrade.Text)

                                 select new { a.Prod_Name, a.Lot_Bal_Qty, a.RM_Grade, a.Prod_Code }).ToList();
                        if (S.Count > 0)
                        {

                            R1.Cells["Prod_ID"].Value = S[0].Prod_Code.ToString();
                            R1.Cells["RM_Size"].Value = S[0].Prod_Name.ToString();
                            R1.Cells["RM_Grade"].Value = txtGrade.Text;
                            R1.Cells["RM_Lot_Qty"].Value = S[0].Lot_Bal_Qty.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Lot No Entered or Lot Not Cleared by QA inspection");
                            R1.Cells[columnIndex].Value = "";
                            return;
                        }
                    }
                }
                if (columnName == "RM_Issued" || columnName == "RM_E_Cut" || columnName == "RM_G_Loss")
                {
                    decimal RM_Lot_Qty = Convert.ToDecimal(R1.Cells["RM_Lot_Qty"].Value);
                    decimal RM_Issued = Convert.ToDecimal(R1.Cells["RM_Issued"].Value);
                    decimal RM_E_cut = Convert.ToDecimal(R1.Cells["RM_E_cut"].Value);
                    decimal RM_G_Loss = Convert.ToDecimal(R1.Cells["RM_G_Loss"].Value);
                    if ((RM_Issued) > RM_Lot_Qty)
                    {
                        MessageBox.Show("Total RM Used Qty Cannot Be Greater Than RM Lot Available Qty");
                        R1.Cells[columnIndex].Value = "";
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                        {
                            y += (dgrmconsumed.Rows[i].Cells["RM_Issued"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Issued"].Value);
                            q += (dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value);
                            v += (dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value);


                        }
                        txtTotalRMQty.Text = (y - q - v).ToString();
                    }
                }
                if (columnName == "RM_Grade")
                {

                    var S = (from a in db.QA_Mtrl_Grade_Masters
                             where a.Company_ID == logIn.company && a.Material_Grade == R1.Cells["RM_Grade"].Value.ToString()
                             select a).ToList();
                    if (S.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Material Grade Entered is invalid");
                        R1.Cells["RM_Grade"].Value = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        private void dgrmconsumed_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "RM_Lot_No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "Mtrl_Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                //{
                //    y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                //    q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                //    v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                //}
                //TxtrolledQty.Text = (y - q - v).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addSections(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];

                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
                if (columnName == "RM_Lot_No")
                {
                    var Prodname = (from a in db.SP_Get_RM_Alloted_Qty(logIn.company, dpPDate.Value)
                                    where a.Fg_item_Code == Convert.ToInt32(txtItemCode.Text) && a.RM_Grade == txtGrade.Text
                                    && a.Lot_Bal_Qty>0
                                    select new { a.RM_Lot_No, a.RM_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("LOT_No");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.RM_Lot_No);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                if (columnName == "Mtrl_Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mtrl_Grade");
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            { 
                for (int i = 0; i < dgrmconsumed.RowCount-1; i++)
                {
                    string lotno = (dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value.ToString(); ;
                    if(Convert.ToDecimal(txtTotalRMQty.Text) == Convert.ToDecimal(txtTotQtyFinished.Text))
                    {

                    }
                    else
                    {
                        MessageBox.Show("Rolled Qty is Not Tallied, cannot be saved");
                        return;
                    }
                    if(lotno != "")
                    {
                        Decimal LotQTy  =  (dgrmconsumed.Rows[i].Cells["RM_Issued"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Issued"].Value);
                        if (LotQTy == 0)
                        {
                            MessageBox.Show("Lot Issue Qty Cannot Be Zero");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Blank Records Cannot Be Saved");
                        return;
                    }

                }
                db.Sp_delete_Production_RMData(txtReportNo.Text, Convert.ToInt32(txtItemCode.Text), txtGrade.Text, logIn.company, logIn.BU_ID) ;
                for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
                    {

                 
                    Production_report_rolling_Child SC = new Production_report_rolling_Child();
                    //var d1 = (from a in db.Production_report_rollings where a.PR_No == txtReportNo.Text && a.Company_Id == logIn.company && a.BU_ID == logIn.BU_ID select new { a.ID }).ToList();
                    //SC.PR_Master_ID = d1[0].ID;
                    SC.PR_No = txtReportNo.Text;
                    SC.FG_Prod_ID = Convert.ToInt32(txtItemCode.Text);
                    SC.Prod_ID = Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Prod_ID"].Value);
                    var S = (from a in db.QA_Mtrl_Grade_Masters
                             where a.Company_ID == logIn.company && a.Material_Grade == txtGrade.Text.ToString()
                             select new { a.id }).ToList();
                    SC.FG_Grade_ID = S[0].id;

                    var S1 = (from a in db.QA_Mtrl_Grade_Masters
                             where a.Company_ID == logIn.company && a.Material_Grade == dgrmconsumed.Rows[i].Cells["RM_Grade"].Value.ToString()
                              select new { a.id }).ToList();
                    SC.RM_Grade = S1[0].id;

                    //SC.RM_Size = dgrmconsumed.Rows[i].Cells["RM_Grade"].Value.ToString();
                    SC.RM_Lot_No = (dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value.ToString();
                    SC.RM_iss_Qty = (dgrmconsumed.Rows[i].Cells["RM_Issued"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Issued"].Value);
                    SC.Rm_Cut_Qty = (dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value);
                    //SC.RM_Size = dgrmconsumed.Rows[i].Cells["RM_Size"].Value.ToString(); 
                    SC.RM_Lot_Qty = (dgrmconsumed.Rows[i].Cells["RM_Lot_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Lot_Qty"].Value);
                    SC.GC_Loss = (dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value);
                    SC.Company_Id = logIn.company;
                    //SC.Created_By = lblCreatedBy.Text;
                    //SC.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Production_report_rolling_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                MessageBox.Show("RM Lot Wise Details Updated Sucessfully");
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
    }
}
