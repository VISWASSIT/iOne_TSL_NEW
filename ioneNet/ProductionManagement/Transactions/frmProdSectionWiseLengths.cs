using Ione_DAL;
using iTextSharp.text.pdf;
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
    public partial class frmProdSectionWiseLengths: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmProdSectionWiseLengths()
        {
            InitializeComponent();
        }

        private void frmProdSectionWiseLengths_Load(object sender, EventArgs e)
        {
            txtReportNo.Text = ProductionManagement.Transactions.frm_Production_Report.ReportNo;
            txtItemCode.Text = ProductionManagement.Transactions.frm_Production_Report.ItemCode;
            txtTotQtyFinished.Text = ProductionManagement.Transactions.frm_Production_Report.RecQty;
            txtItemName.Text = ProductionManagement.Transactions.frm_Production_Report.ItemName;
            txtgrade.Text = ProductionManagement.Transactions.frm_Production_Report.MtrlGrade;
            string pdate = ProductionManagement.Transactions.frm_Production_Report.proddate;
            var dm = (from s in db.QA_Dimensional_Specs
                      
                       where s.Parameter_ID == 17 && s.Prod_ID == Convert.ToInt32(txtItemCode.Text)
                       select new
                       {
                           s.Spec_Min                      


                       }).ToList();
            txtSecWt.Text = "0";
            if (dm.Count > 0)
            {
                txtSecWt.Text = dm[0].Spec_Min.ToString();
            }
            var dm1 = (from s in db.Production_Section_LengthWises
                       join g in db.QA_Mtrl_Grade_Masters on s.Prod_Grade equals g.id
                       where s.Report_No == txtReportNo.Text && s.Prod_ID == Convert.ToInt32(txtItemCode.Text)
                       && g.Material_Grade == txtgrade.Text
                       select new
                       {
                           s.Planned_Length,
                           s.Planned_Qty_Nos,
                           s.Length_In_Mtrs,
                           s.Prod_Length_To_Claculate,
                           s.Produced_Qty_Nos,
                            s.Produced_Qty_MT
                            
                           

                       });
            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count > 0)
            {
                dgProductsList.DataSource = dtr;
            }
            else
            {
                //string pd = dpPDate.Value.ToString("yyyy-MM-dd");

                var dm2 = (from s in db.Production_DayPlans
                           join g in db.QA_Mtrl_Grade_Masters on s.Item_Grade equals g.id
                           where s.pDate == Convert.ToDateTime(pdate) && s.Item_Code == Convert.ToInt32(txtItemCode.Text)
                           && g.Material_Grade == txtgrade.Text
                           select new
                           {
                               Planned_Length = s.Prod_Length,
                               Planned_Qty_Nos = s.Planned_Qty_Nos,
                               Length_In_Mtrs = "",
                               Prod_Length_To_Claculate = "",
                               Produced_Qty_Nos = "",
                               Produced_Qty_MT = ""


                           });
                SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dtr1 = new DataTable();
                da3.Fill(dtr1);
                if (dtr1.Rows.Count > 0)
                {
                    dgProductsList.DataSource = dtr1;
                }

            }
            decimal x = 0, n = 0;
            for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
            {

                x += (dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == "" || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == null || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value);
                n += 1;

            }
            txtTotalQtyLengthwise.Text = x.ToString(".00");
           
        }

        private void dgProductsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
            int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
            string columnName = dgProductsList.Columns[columnIndex].Name;
            if (columnName == "Length_In_Mtrs")
            {
                double n;
                decimal l = 0;
                if (double.TryParse(R1.Cells["Length_In_Mtrs"].Value.ToString(),out n))
                {
                    l = Convert.ToDecimal(R1.Cells["Length_In_Mtrs"].Value);
                    R1.Cells["Prod_Length_To_Claculate"].Value = l.ToString();
                }
                else
                {
                    l = 0;
                    R1.Cells["Prod_Length_To_Claculate"].Value = "";
                }
                
                
                
                
            }
            if (columnName == "Produced_Qty_Nos")
            {
                decimal q = 0;
                decimal x = Convert.ToDecimal(txtSecWt.Text);
                decimal l = Convert.ToDecimal(R1.Cells["Prod_Length_To_Claculate"].Value);
                int r = Convert.ToInt32(R1.Cells["Produced_Qty_Nos"].Value);
                decimal w = (x * l * r)/1000;
                if (x > 0)
                {
                    R1.Cells["Produced_Qty_MT"].Value = w.ToString("0.000");

                    for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                    {

                        q += (dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == "" || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == null || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value);
                        r = r + 1;

                    }
                    txtTotalQtyLengthwise.Text = q.ToString("0.00");
                }
            }
            if (columnName == "Produced_Qty_MT")
            {
                decimal x = 0;
                decimal r = 0;
                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {

                    x += (dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == "" || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == null || dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value);
                    r = r + 1;

                }
                txtTotalQtyLengthwise.Text = x.ToString("0.00");
                //textBox2.Text = r.ToString();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItemCode.Text);
                myString = txtReportNo.Text;



                if (Convert.ToDecimal(txtTotalQtyLengthwise.Text) != Convert.ToDecimal(txtTotQtyFinished.Text))
                {
                    MessageBox.Show("Legth Wise Qty Should be Equal to Total Finished Qty, Cannot Be Saved");
                    return;
                }
                else
                {
                    if ((from u in db.Production_Section_LengthWises where u.Report_No == myString && u.Prod_ID == Icode select u).Count() > 0)
                {
                    

                    db.sp_Prod_LengthData_Delete(myString, Icode, logIn.company);
                }
                else
                {

                    // myString = txtSoNo.Text;

                }
                decimal y, x;
                y = Convert.ToDecimal(txtTotalQtyLengthwise.Text);



                dgProductsList.Enabled = false;
                Boolean NewRec = true;
                var gstno = (from c in db.Production_report_rollings
                             where c.PR_No == myString
                             select new { c.ID }).ToList();
                if (gstno.Count > 0)
                {
                    NewRec = false;
                }
                //db.Transaction = transaction;
                for (int i = 0; i < dgProductsList.RowCount - 1; i++)
                {
                    Production_Section_LengthWise SC = new Production_Section_LengthWise();

                    SC.Report_No = myString;
                    SC.Prod_ID = Convert.ToInt32(txtItemCode.Text);
                        var S = (from a in db.QA_Mtrl_Grade_Masters
                                 where a.Company_ID == logIn.company && a.Material_Grade == (txtgrade.Text)
                                 select new { a.id }).ToList();

                        SC.Prod_Grade = S[0].id;
                    SC.Length_In_Mtrs = (dgProductsList.Rows[i].Cells["Length_In_Mtrs"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Length_In_Mtrs"].Value).ToString();
                    SC.Planned_Length = (dgProductsList.Rows[i].Cells["Planned_Length"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Planned_Length"].Value).ToString();
                    SC.Planned_Qty_Nos = (dgProductsList.Rows[i].Cells["Planned_Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Planned_Qty_Nos"].Value);
                    SC.Produced_Qty_MT = (dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Produced_Qty_MT"].Value);
                    SC.Produced_Qty_Nos = (dgProductsList.Rows[i].Cells["Produced_Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Produced_Qty_Nos"].Value);
                    SC.Prod_Length_To_Claculate = (dgProductsList.Rows[i].Cells["Prod_Length_To_Claculate"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Prod_Length_To_Claculate"].Value).ToString();

                        //if (NewRec == false)
                        // {
                        //     SC.Status = "Closed";
                        // }
                        // else
                        // {
                        //     SC.Status = "Open";
                        // }



                        db.Production_Section_LengthWises.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Length Wise Details Updated Sucessfully");               
                this.Close();
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

        private void dgProductsList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
            string columnName = dgProductsList.Columns[columnIndex].HeaderText;
            if (columnName == "Produced_Length")
            {
                e.Control.KeyPress += new KeyPressEventHandler(CheckKey);
            }
        }
        private void CheckKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '-'
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}
