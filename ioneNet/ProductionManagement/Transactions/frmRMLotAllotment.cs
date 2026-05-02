using Ione_DAL;
using Syncfusion.Windows.Forms.Tools;
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
    public partial class frmRMLotAllotment: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static decimal TotQty;
        public frmRMLotAllotment()
        {
            InitializeComponent();
        }

        private void frmRMLotAllotment_Load(object sender, EventArgs e)
        {
            try
            {
                dtProdSchDate.Value = ProductionManagement.Transactions.frmDayProductionPlan.PlanDate;
                txtItemCode.Text = ProductionManagement.Transactions.frmDayProductionPlan.ItemCode;
                decimal tqty = Convert.ToDecimal(ProductionManagement.Transactions.frmDayProductionPlan.RecQty)*10/100;
                txtTotQtyFinished.Text = (Convert.ToDecimal(ProductionManagement.Transactions.frmDayProductionPlan.RecQty)+tqty).ToString();

                txtItemName.Text = ProductionManagement.Transactions.frmDayProductionPlan.ItemName;
                txtGrade.Text = ProductionManagement.Transactions.frmDayProductionPlan.MtrlGrade;
                textBox1.Text = ProductionManagement.Transactions.frmDayProductionPlan.Suppname;
                txtID.Text = ProductionManagement.Transactions.frmDayProductionPlan.plan_master_id.ToString();
                //if (ProductionManagement.Transactions.frmDayProductionPlan.convprod ==true)
                //{
                //    checkBox1.Checked = true;
                    textBox1.Text = ProductionManagement.Transactions.frmDayProductionPlan.Suppname;
                //}
                //else
                //{
                //    checkBox1.Checked = false;
                //    textBox1.Text = "";
                //}
                    var pscrap = (from m in db.Products where m.Prod_Type_Id == 140 && m.Company_ID == logIn.company select new { m.Prod_Name }).Distinct().ToList();
                if (pscrap.Count > 0)
                {
                    multiSelectionComboBox1.DataSource = pscrap;
                    multiSelectionComboBox1.ValueMember = "Prod_Name";
                    multiSelectionComboBox1.DisplayMember = "Prod_Name";
                    multiSelectionComboBox1.SelectedIndex = -1;
                }

                string pd = dtProdSchDate.Value.ToString("yyyy-MM-dd");
                var dm1 = (from s in db.Prod_DayPlan_RM_Allotments
                           
                           join g in db.GoodsReceiptNote_Childs on s.RM_Lot_No equals g.Int_Batch_No
                           join p in db.Products on g.Prod_Code equals p.prod_ID
                           join m in db.QA_Mtrl_Grade_Masters on g.Prod_Grade_ID equals m.id into ps
                           from m in ps.DefaultIfEmpty()
                           where s.prod_date == Convert.ToDateTime(pd) && s.FG_Item_Code == Convert.ToInt32(txtItemCode.Text)
                           && m.Material_Grade == txtGrade.Text


                           select new

                           {
                               RM_Lot_No = s.RM_Lot_No,
                               RM_Size = p.Prod_Name,
                               Prod_ID = p.prod_ID,
                               RM_Grade = m.Material_Grade,
                               Item_Grade = m.Material_Grade,
                               RM_Lot_Qty = s.RM_Lot_Qty_Bal,
                               RM_Qty_Alloted = s.RM_Lot_Qty_Alloted
                               
                           });
                                
                
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dgrmconsumed.DataSource = ds2;



                decimal y = 0;
                decimal q = 0;
                decimal v = 0;
                for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                {
                    y += (dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value);
                    //q += (dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_E_cut"].Value);
                    //v += (dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_G_Loss"].Value);


                }
                txtTotalRMQty.Text = (y).ToString();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var RmLot = (from a in db.SP_Get_RM_Lot_Qty(logIn.company, null, txtGrade.Text)

            //                select new { a.LOT_No, a.RM_Grade,a.Prod_Code,a.prod_name });

            string RMData = "";
            foreach (VisualItem obj in this.multiSelectionComboBox1.VisualItems)
            {

                if (RMData != "")
                {

                    RMData = RMData + "," + obj.Text;
                }
                else

                {

                    RMData = obj.Text ;
                }
            }
            //var dm1 = (from a in db.SP_Get_RM_Lot_Qty_ForAllotment(logIn.company, null, txtGrade.Text)

            //           where a.prod_name.Contains(RMData)
            //           select new
            //           {
            //               RM_Lot_No =  a.LOT_No,
            //               RM_Size = a.prod_name,
            //               Prod_ID = a.Prod_Code,
            //               RM_Grade = a.RM_Grade,
            //               RM_Lot_Qty =a.Lot_Bal_Qty,
            //               RM_Qty_Alloted = 0

            //           }).ToList();
            string pd = dtProdSchDate.Value.ToString("yyyy-MM-dd");
            if (textBox1.Text == "Own")
            {

                SqlCommand cmd2 = new SqlCommand("SP_Get_RM_Lot_ForAllotment", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@RM_Grade", txtGrade.Text);
                cmd2.Parameters.AddWithValue("@prodName", RMData);

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dgrmconsumed.DataSource = ds2;

            }
            else
            {
                SqlCommand cmd3 = new SqlCommand("SP_Get_RM_Lot_ForAllotment_Conv", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@compname", logIn.company);
                cmd3.Parameters.AddWithValue("@RM_Grade", txtGrade.Text);
                cmd3.Parameters.AddWithValue("@prodName", RMData);
                cmd3.Parameters.AddWithValue("@convParty", textBox1.Text);

                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                //DataSet ds2 = new DataSet();
                DataTable ds3 = new DataTable();
                // da2.Fill(ds2, "x");
                da3.Fill(ds3);
                dgrmconsumed.DataSource = ds3;
            }
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
                {
                    string lotno = (dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value.ToString(); ;
                    decimal AQty = Convert.ToDecimal(txtTotQtyFinished.Text) * 10/100 ;
                    if (Convert.ToDecimal(txtTotalRMQty.Text) >= (Convert.ToDecimal(txtTotQtyFinished.Text)))
                    {

                    }
                    else
                    {
                        MessageBox.Show("Alloted Qty is Not Tallied With Planned Qty, cannot be saved");
                        return;
                    }
                    

                }
                string pd = dtProdSchDate.Value.ToString("yyyy-MM-dd");
                int grade_id = 0;
                SqlCommand cmd2 = new SqlCommand("delete  from [Prod_DayPlan_RM_Allotment] where master_id = @masterid and [FG_Item_Code] = @prodid and [FG_Grade] = @Mgrade", con);
                //con.Close();
                con.Open();
                cmd2.Parameters.AddWithValue("@masterid", Convert.ToInt32(txtID.Text));
                cmd2.Parameters.AddWithValue("@prodid", Convert.ToInt32(txtItemCode.Text));
                var S = (from a in db.QA_Mtrl_Grade_Masters
                         where a.Company_ID == logIn.company && a.Material_Grade == txtGrade.Text.ToString()
                         select new { a.id }).ToList();
                grade_id = S[0].id;
                cmd2.Parameters.AddWithValue("@Mgrade",grade_id );
                cmd2.ExecuteNonQuery();
                con.Close();
                //db.Sp_delete_Production_RMData(txtReportNo.Text, Convert.ToInt32(txtItemCode.Text), txtGrade.Text, logIn.company, logIn.BU_ID);
                for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
                {
                    decimal qa =  (dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value);

                    if (qa > 0)
                    {
                        Prod_DayPlan_RM_Allotment SC = new Prod_DayPlan_RM_Allotment();
                        //var d1 = (from a in db.Production_report_rollings where a.PR_No == txtReportNo.Text && a.Company_Id == logIn.company && a.BU_ID == logIn.BU_ID select new { a.ID }).ToList();
                        SC.Master_Id = Convert.ToInt32(txtID.Text);
                        SC.prod_date = Convert.ToDateTime(dtProdSchDate.Value.ToString("yyyy-MM-dd"));
                        SC.FG_Item_Code = Convert.ToInt32(txtItemCode.Text);

                        //SC.rm = Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Prod_ID"].Value);
                        //var S = (from a in db.QA_Mtrl_Grade_Masters
                        //         where a.Company_ID == logIn.company && a.Material_Grade == txtGrade.Text.ToString()
                        //         select new { a.id }).ToList();
                        SC.FG_Grade = grade_id;


                        //SC.RM_Size = dgrmconsumed.Rows[i].Cells["RM_Grade"].Value.ToString();
                        SC.RM_Lot_No = (dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["RM_Lot_No"].Value.ToString();
                        //SC.RM_Size = dgrmconsumed.Rows[i].Cells["RM_Size"].Value.ToString(); 
                        SC.RM_Lot_Qty_Bal = (dgrmconsumed.Rows[i].Cells["RM_Lot_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Lot_Qty"].Value);
                        SC.RM_Lot_Qty_Alloted = (dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value);

                        db.Prod_DayPlan_RM_Allotments.InsertOnSubmit(SC);
                    }
                }
                db.SubmitChanges();
                TotQty = Convert.ToDecimal(txtTotalRMQty.Text);
                MessageBox.Show("RM Lot Allotment Done Sucessfully");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TotQty = Convert.ToDecimal(txtTotalRMQty.Text);
            this.Close();
        }

        private void dgrmconsumed_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];
            //DataGridViewRow R2 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index-1];
            int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
            string columnName = dgrmconsumed.Columns[columnIndex].Name;

            decimal pqty = Convert.ToDecimal(txtTotQtyFinished.Text);
            decimal rmqty = Convert.ToDecimal(R1.Cells["RM_Lot_Qty"].Value);
            decimal allotqty = Convert.ToDecimal(R1.Cells["RM_Qty_Alloted"].Value);
            if(allotqty> rmqty)
            {
                MessageBox.Show("Allotment Qty Cannot Greater Than Lot Available Qty");
                R1.Cells["RM_Qty_Alloted"].Value = "0.00";
                return;
            }

            decimal y=0;
            for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
            {
                y += (dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value);
               

            }
            decimal tqty = pqty;
            if (y > (pqty))
            {
                MessageBox.Show("Total Allotment Qty Cannot Greater Than Planned Qty");
                txtTotalRMQty.Text = "0.00";
                return;
            }
            else
            {
                txtTotalRMQty.Text = (y).ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            decimal pqty = Convert.ToDecimal(txtTotQtyFinished.Text);
            //decimal tqty = (pqty * 10) / 100;
            decimal aQty = pqty;
            for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
            {
                if (aQty > 0)
                {
                    decimal rmqty = Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Lot_Qty"].Value);
                    if ((aQty) > rmqty)
                    {
                        dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value = rmqty;
                        aQty = aQty - rmqty;
                    }
                    else
                    {
                        dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value = pqty;
                        aQty = 0;
                    }
                }
            }



            //}
            //else
            //{
            //    //dgrmconsumed.DataSource = null;
            //}
            decimal y = 0;
            for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
            {
                y += (dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_Qty_Alloted"].Value);


            }
            txtTotalRMQty.Text = (y).ToString();
        }
    }
}
