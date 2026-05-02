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
using Ione_DAL;
using Syncfusion.Windows.Forms.Tools;

namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class frmESICriteria : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmESICriteria()
        {
            InitializeComponent();
        }

        private void frmESICriteria_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            txtID.Enabled = false;

            //Bind Heads
            var pscrap = (from m in db.HR_Salary_HeadsInfos where m.Company_ID == logIn.company select new { m.id, m.Head_Name }).Distinct().ToList();
            if (pscrap.Count > 0)
            {
                multiSelectionComboBox1.DataSource = pscrap;
                multiSelectionComboBox1.ValueMember = "id";
                multiSelectionComboBox1.DisplayMember = "Head_Name";
                multiSelectionComboBox1.SelectedIndex = -1;
            }
            bindExisitingRecords();
        }
        public void bindExisitingRecords()
        {
            try
            {
                var p = (from s in db.HR_ESI_Criterias
                         where s.Company_ID == logIn.BU_ID


                         select new
                         {
                             ID = s.id,
                             s.Period_From,
                             s.Period_To,
                             ESI_Per = s.ESI_Rate_Employee


                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    sfDataGrid1.DataSource = dt1;
                }
                else
                {
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpContr.Text == string.Empty)
                {
                    MessageBox.Show("Employee Contribution Should Not Be Empty", "ESI Criteria", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   txtEmpContr.Focus();
                    return;
                }
                else if (txtEmprCont.Text == string.Empty)
                {
                    MessageBox.Show("Employer Contribution Should Not Be Empty");
                    txtEmprCont.Focus();
                    return;
                }
                else if (txtSalaryLimit.Text == string.Empty)
                {
                    MessageBox.Show("Enter Salary Limit");
                    txtSalaryLimit.Focus();
                    return;
                }             
               
                
                else if (chkGrossSal.Checked==false)
                {

                    if (multiSelectionComboBox1.VisualItems.Count == 0)
                    {
                        MessageBox.Show("Select Head Names on Which ESI Will be Deducted");
                        multiSelectionComboBox1.Focus();
                        return;
                    }
                    else
                    {
                        save();
                    }
                }                
                else
                {
                    save();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void save()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (txtID.Text != "")
                {
                    var p1 = db.HR_ESI_Criterias.Where(w => w.id == Convert.ToInt32(txtID.Text) && w.Company_ID == logIn.BU_ID).FirstOrDefault();

                    
                    p1.Period_From = dtFrmDate.Value;
                    p1.Period_To = dtToDate.Value;                    
                    p1.ESI_Rate_Employee = Convert.ToDecimal(txtEmpContr.Text);
                    p1.ESI_Rate_Employer = Convert.ToDecimal(txtEmprCont.Text); 
                    p1.Salary_Limit = Convert.ToDecimal(txtSalaryLimit.Text);
                    p1.Deduct_on_Gross_Sal = chkGrossSal.Checked;

                  
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

                    p1.Heads_To_Deduct = RMData;                  

                    p1.Created_By = lblCreatedBy.Text;
                    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p1.Company_ID = logIn.BU_ID;

                    db.SubmitChanges();

                    MessageBox.Show("Record Updated Successfully");
                    this.Close();


                }
                else
                {
                    if (null != db.Connection)
                    {
                        db.Connection.Close();
                    }
                    System.Data.Common.DbTransaction transaction;
                    db.Connection.Open();
                    transaction = db.Connection.BeginTransaction();
                    db.Transaction = transaction;

                    HR_ESI_Criteria p = new HR_ESI_Criteria();


                    p.Period_From = dtFrmDate.Value;
                    p.Period_To = dtToDate.Value;
                    p.ESI_Rate_Employee = Convert.ToDecimal(txtEmpContr.Text);
                    p.ESI_Rate_Employer = Convert.ToDecimal(txtEmprCont.Text);
                    p.Salary_Limit = Convert.ToDecimal(txtSalaryLimit.Text);
                    p.Deduct_on_Gross_Sal = chkGrossSal.Checked;


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

                    p.Heads_To_Deduct = RMData;

                    p.Created_By = lblCreatedBy.Text;
                    p.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p.Company_ID = logIn.BU_ID;                   

                    db.HR_ESI_Criterias.InsertOnSubmit(p);
                    db.SubmitChanges();
                    db.Transaction = transaction;
                    transaction.Commit();

                    MessageBox.Show("Record Saved Successfully");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }

            finally
            {
                if (null != db.Connection)
                {
                    db.Connection.Close();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                //  txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                txtID.Text = currentCellValue.ToString();
                var d = (from po in db.HR_ESI_Criterias
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Period_From,
                             po.Period_To,
                             po.ESI_Rate_Employee,
                             po.ESI_Rate_Employer,
                             po.Created_By,
                             po.Modified_BY,
                             po.Salary_Limit,
                             po.Deduct_on_Gross_Sal,
                             po.Heads_To_Deduct


                         }).ToList();
                if (d.Count > 0)
                {
                    dtFrmDate.Text = d[0].Period_From.ToString();
                    dtToDate.Text = d[0].Period_To.ToString();

                    txtEmpContr.Text = d[0].ESI_Rate_Employee.ToString();
                    txtEmprCont.Text = d[0].ESI_Rate_Employer.ToString();
                    txtSalaryLimit.Text = d[0].Salary_Limit.ToString();
                    chkGrossSal.Checked = false;

                    if (d[0].Deduct_on_Gross_Sal == true)
                    {
                        chkGrossSal.Checked = true;
                    }

                    if (d[0].Heads_To_Deduct != null)
                    {
                        string MP = d[0].Heads_To_Deduct.ToString();
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

                    lblCreatedBy.Text = d[0].Created_By;
                    lblModified.Text = d[0].Modified_BY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
