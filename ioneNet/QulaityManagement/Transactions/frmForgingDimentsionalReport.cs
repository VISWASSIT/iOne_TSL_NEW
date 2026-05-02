using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
using System.Configuration;
using System.Data.SqlClient;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.Qulaity_Management.Transactions
{
    public partial class frmForgingDimentsionalReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmForgingDimentsionalReport()
        {
            InitializeComponent();
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime t = dtProdDate.Value;                
                
                string t1 = t.ToString("dd/MMM/yyyy");

                var d = (from data in db.Get_ForgingData_For_Inspection(logIn.company,Convert.ToDateTime(t1)) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dgProducts.DataSource = d;
                    
                }

                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    //txtSearch.Text = "";
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmForgingDimentsionalReport_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            AutoincrementId();
        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Forging_DimensionalReport (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtInvNo.Text = result.FirstOrDefault().Report_No;

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((from a in db.Forging_DimensionalReports where a.Company_ID == logIn.company && a.Report_no == txtInvNo.Text select a).Count() > 0)
                {
                    db.Sp_Delete_DimensionalReport (logIn.company, txtInvNo.Text);
                }

                for (int i = 0; i < dgProducts.Rows.Count; i++)
                {
                    if(dgProducts.Rows[i].Cells["MO_Sno"].Value !=null)
                    {


                    Forging_DimensionalReport pb = new Forging_DimensionalReport();
                    pb.Report_no = txtInvNo.Text;
                    pb.Report_Date = dpInvDate.Value;
                    pb.Forging_Date = dtProdDate.Value;
                    pb.MO_Sno = Convert.ToInt32(dgProducts.Rows[i].Cells["MO_Sno"].Value.ToString());
                    pb.MO_No = (dgProducts.Rows[i].Cells["MO_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["MO_No"].Value).ToString();
                    pb.Insp_Qty = Convert.ToDecimal(dgProducts.Rows[i].Cells["BalQty"].Value.ToString()); 
                    pb.Forging_Dimensions = (dgProducts.Rows[i].Cells["Forging_Dimensions"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Dimensions"].Value).ToString();
                    pb.Visual_Insp = (dgProducts.Rows[i].Cells["Visual_Insp"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Visual_Insp"].Value).ToString();
                    pb.QA_Result = (dgProducts.Rows[i].Cells["QA_Result"].Value == null) ? "" : (dgProducts.Rows[i].Cells["QA_Result"].Value).ToString();
                    pb.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    pb.Inspected_By = textBox2.Text;
                    pb.Company_ID = logIn.company;
                    pb.Created_By = lblCreatedBy.Text;
                    pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                    pb.Report_Comments = textBox1.Text;
                    pb.Forging_Report_ID = Convert.ToInt32(dgProducts.Rows[i].Cells["Forging_Report_ID"].Value.ToString());
                    db.Forging_DimensionalReports.InsertOnSubmit(pb);
                    
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
            try
            {
                var p = (from s in db.Forging_DimensionalReports

                         where s.Company_ID == logIn.company


                         select new
                         {
                             s.Report_no,
                             s.Report_Date,
                             s.Forging_Date


                         }
                        ).Distinct();
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    sfDataGrid2.DataSource = dt1;
                    this.sfDataGrid2.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid2.Columns["Report_no"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid2.Columns["Report_no"].ShowFilterRowOptions = false;
                    this.sfDataGrid2.Columns["Report_no"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid2.Columns["Report_no"].FilterRowCondition = FilterRowCondition.Contains;

                    //this.sfDataGrid2.Columns["GRN_NO"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid2.Columns["GRN_NO"].ShowFilterRowOptions = false;
                    //this.sfDataGrid2.Columns["GRN_NO"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid2.Columns["GRN_NO"].FilterRowCondition = FilterRowCondition.Contains;




                }
                else
                {
                }
                groupBox2.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid2.CurrentCell.RowIndex;
                var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                var mappingName = sfDataGrid2.Columns[0].MappingName;
                // var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //  txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                txtInvNo.Text = currentCellValue.ToString();
                var d = (from po in db.Forging_DimensionalReports                           where
                         po.Report_no == txtInvNo.Text && po.Company_ID == logIn.company
                         select new
                         {
                             po.Report_Date,
                             po.Forging_Date,                           
                             po.Created_By,
                             po.Modified_BY,
                             po.Inspected_By,
                             po.Report_Comments
                          



                         }).ToList();


                if (d.Count > 0)
                {

                    dpInvDate.Text = d[0].Report_Date.ToString();
                    //txtSuppName.Text = d[0].Supplier_Name;
                    //txtGRNNo.Text = d[0].GRN_NO;
                    dtProdDate.Text = d[0].Forging_Date.ToString();
                    textBox2.Text = d[0].Inspected_By;
                    textBox1.Text = d[0].Report_Comments;
                    lblCreatedBy.Text = d[0].Created_By;
                    lblModified.Text = d[0].Modified_BY;
                }

                var da1 = (from po in db.Forging_DimensionalReports
                           
                           
                          
                           where po.Report_no == txtInvNo.Text && po.Company_ID == logIn.company

                           select new
                           {

                               MO_No = po.MO_No,                              
                               MO_Sno = po.MO_Sno,
                               RF_No ="",                               
                               BalQty = po.Insp_Qty,                             
                               Forging_Dimensions = po.Forging_Dimensions,
                               Visual_Insp = po.Visual_Insp,
                               QA_Result = po.QA_Result,
                               Remarks = po.Remarks,
                               Forging_Report_ID = po.Forging_Report_ID
                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(da1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;

                groupBox2.Visible = false;
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
                if (dgProducts.Rows.Count > 0)
                {

                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);                    
                    
                    //}
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
