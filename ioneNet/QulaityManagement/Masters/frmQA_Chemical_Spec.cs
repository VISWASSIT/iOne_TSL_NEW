using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Ione_DAL;

namespace ioneNet.Qulaity_Management.Masters
{
    public partial class frmQA_Chemical_Spec : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmQA_Chemical_Spec()
        {
            InitializeComponent();
        }

        private void frmQA_Chemical_Spec_Load(object sender, EventArgs e)
        {

            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //Status
            var pStatus = (from m in db.QA_Mtrl_Grade_Masters select new { m.id, m.Material_Grade }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbMtrlGrade.DataSource = pStatus;
                cmbMtrlGrade.ValueMember = "id";
                cmbMtrlGrade.DisplayMember = "Material_Grade";
                cmbMtrlGrade.SelectedIndex = -1;
            }

            


        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(cmbMtrlGrade.SelectedIndex == -1)
            {
                MessageBox.Show("Select Material Grade");
                cmbMtrlGrade.Focus();
                return;
            }
            if (txtRevNo.Text == null)
            {
                MessageBox.Show("Enter Revision No");
                txtRevNo.Focus();
                return;
            }

            if ((from a in db.QA_Mtrl_Grade_Test_Specs where a.Company_id == logIn.company && a.Mtrl_Grade_Id == Convert.ToInt32(cmbMtrlGrade.SelectedValue) && a.Rev_No == Convert.ToInt32(txtRevNo.Text) select a).Count() > 0)
            {
                db.sp_Delete_Mtrl_test_Specs(Convert.ToInt32(cmbMtrlGrade.SelectedValue), logIn.company,"Chemical",Convert.ToInt32(txtRevNo.Text));
            }

            for (int i = 0; i < dgSpecs.RowCount - 1; i++)
            {
                QA_Mtrl_Grade_Test_Spec SC = new QA_Mtrl_Grade_Test_Spec();
                //var d1 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == cmbMtrlGrade.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                SC.Rev_No = Convert.ToInt32(txtRevNo.Text);
                SC.Rev_Date = dtRevDate.Value;

                SC.Mtrl_Grade_Id = Convert.ToInt32(cmbMtrlGrade.SelectedValue);

                SC.Parameter_ID = Convert.ToInt32(dgSpecs.Rows[i].Cells["parameter_id"].Value);
                SC.Spec_Min = (dgSpecs.Rows[i].Cells["Min_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSpecs.Rows[i].Cells["Min_Value"].Value);
                SC.Spec_Max = (dgSpecs.Rows[i].Cells["Max_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSpecs.Rows[i].Cells["Max_Value"].Value);
                SC.Company_id = logIn.company;
                
                db.QA_Mtrl_Grade_Test_Specs.InsertOnSubmit(SC);
                db.SubmitChanges();
                
            }
            MessageBox.Show("Record Saved / Updated Successfully");
            this.Close();
        }

        private void dgSpecs_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgSpecs.CurrentCell.ColumnIndex;
                string columnName = dgSpecs.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
               
                if (tb3 != null && columnName == "Parameter")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgSpecs.Rows[dgSpecs.CurrentRow.Index];

                int columnIndex = dgSpecs.CurrentCell.ColumnIndex;
                string columnName = dgSpecs.Columns[columnIndex].HeaderText;

                if (columnName == "Parameter")
                {
                    var Prodname = (from d in db.QA_Test_Parameters where d.Test_Group == "Chemical" select new { d.Test_Parameter }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Parameter_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Test_Parameter);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgSpecs_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string comnpstatecode, suppStateCode;
                decimal taxRate = 0;
                DataGridViewRow R1 = dgSpecs.Rows[dgSpecs.CurrentRow.Index];
                int columnIndex = dgSpecs.CurrentCell.ColumnIndex;
                string columnName = dgSpecs.Columns[columnIndex].Name;


                if (columnName == "Parameter_Name")
                {
                    if (R1.Cells["Parameter_Name"].Value != null)
                    {
                        var getProductName = (from s in db.QA_Test_Parameters
                                              where s.Test_Parameter == R1.Cells["Parameter_Name"].Value.ToString()
                                              select new { s.id, s.Parameter_Uom }).ToList();

                        if (getProductName.Count > 0)
                        {
                            R1.Cells["Parameter_UOM"].Value = getProductName[0].Parameter_Uom;
                            R1.Cells["parameter_id"].Value = getProductName[0].id;

                        }
                        else
                        {
                            MessageBox.Show("Invalid Parameter Selected Selected");
                            R1.Cells["Parameter_Name"].Value = "";
                            //cmbAltUom.Focus();
                            return;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbMtrlGrade_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbMtrlGrade.Text != "")
                {
                    int grn_id = 0;
                    var da = (from obj in db.sp_Get_QA_Spec_LatestRevData(Convert.ToInt32(cmbMtrlGrade.SelectedValue), "Chemical")
                             select new { obj.rev_no, obj.Rev_Date }).ToList();


                              
         
                    if (da.Count > 0)
                    {
                        dtRevDate.Text = da[0].Rev_Date.ToString();
                        txtRevNo.Text = da[0].rev_no.ToString();

                    }

                    var ds = (from obj in db.QA_Mtrl_Grade_Test_Specs
                              join a in db.QA_Test_Parameters on obj.Parameter_ID equals a.id
                              where obj.Mtrl_Grade_Id == Convert.ToInt32(cmbMtrlGrade.SelectedValue) 
                              && obj.Rev_No == Convert.ToInt32(txtRevNo.Text) && a.Test_Group == "Chemical"
                              select new { parameter_id = obj.Parameter_ID, Parameter_Name  = a.Test_Parameter, 
                                  Parameter_UOM =a.Parameter_Uom,
                                  Min_Value = obj.Spec_Min,
                                  Max_Value = obj.Spec_Max });

                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(ds);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgSpecs.DataSource = dtr;






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

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                db.sp_Delete_Mtrl_test_Specs(Convert.ToInt32(cmbMtrlGrade.SelectedValue), logIn.company, "Chemical", Convert.ToInt32(txtRevNo.Text));
            }
        }
    }
}
