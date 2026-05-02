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
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;

namespace ioneNet.Qulaity_Management.Masters
{
    public partial class frmQA_MaterialGrades : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmQA_MaterialGrades()
        {
            InitializeComponent();
        }

        private void MaterialGrades_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            bindMasters();
            if(frmQA_MaterialGradesList.editMode == true)
            {
                BindEdit();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
              

               if (txtGradeName.Text == string.Empty)
                {
                    MessageBox.Show("Material grade Should Not Be Empty", "Material grades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGradeName.Focus();
                    return;
                }
                else
                if (cmbColorCode.Text == string.Empty)
                {
                    MessageBox.Show("Select Color Code", "Material grades", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbColorCode.Focus();
                    return;
                }
                else
                {
                    Save();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtGradeName.Text;
                if (txtGradeID.Text != "")
                {
                    if ((from u in db.QA_Mtrl_Grade_Masters where u.id == Convert.ToInt32(txtGradeID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        var p1 = db.QA_Mtrl_Grade_Masters.Where(w => w.id == Convert.ToInt32(txtGradeID.Text) && w.Company_ID == logIn.company).FirstOrDefault();
                        p1.Material_Grade = txtGradeName.Text;
                        p1.ColorCode = Convert.ToInt32(cmbColorCode.SelectedValue);
                        p1.Density = Convert.ToDecimal(txtDensity.Text);
                        p1.HardNess = Convert.ToDecimal(txtHardNess.Text); ;
                        p1.Material_Grade_ShortCode = txtGradeShortCode.Text;
                        string Standards_To_Refer = "";
                        for (int i = 0; i < lstEqStnds.Items.Count; i++)
                        {
                            if (lstEqStnds.GetItemChecked(i))
                            {
                                if (Standards_To_Refer != "")
                                {
                                    Standards_To_Refer = Standards_To_Refer + "," + lstEqStnds.Items[i].ToString();
                                }
                                else
                                {
                                    Standards_To_Refer = lstEqStnds.Items[i].ToString();
                                }
                            }
                        }
                        p1.Ref_Standard = Standards_To_Refer;


                        string EqGrades = "";
                        for (int i = 0; i < lstEqGrades.Items.Count; i++)
                        {
                            if (lstEqGrades.GetItemChecked(i))
                            {
                                if (EqGrades != "")
                                {
                                    EqGrades = EqGrades + "," + lstEqGrades.Items[i].ToString();
                                }
                                else
                                {
                                    EqGrades = lstEqGrades.Items[i].ToString();
                                }
                            }
                        }
                        p1.Eq_Grades = EqGrades;

                        p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.SubmitChanges();
                    }
                }
                else
                {


                    //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                    QA_Mtrl_Grade_Master S = new QA_Mtrl_Grade_Master();
                    {
                        S.Material_Grade = myString;
                        S.ColorCode = Convert.ToInt32(cmbColorCode.SelectedValue);
                        S.Density = Convert.ToDecimal(txtDensity.Text);
                        S.HardNess = Convert.ToDecimal(txtHardNess.Text); ;
                        S.Material_Grade_ShortCode = txtGradeShortCode.Text;
                        string Standards_To_Refer = "";
                        for (int i = 0; i < lstEqStnds.Items.Count; i++)
                        {
                            if (lstEqStnds.GetItemChecked(i))
                            {
                                if (Standards_To_Refer != "")
                                {
                                    Standards_To_Refer = Standards_To_Refer + "," + lstEqStnds.Items[i].ToString();
                                }
                                else
                                {
                                    Standards_To_Refer = lstEqStnds.Items[i].ToString();
                                }
                            }
                        }
                        S.Ref_Standard = Standards_To_Refer;


                        string EqGrades = "";
                        for (int i = 0; i < lstEqGrades.Items.Count; i++)
                        {
                            if (lstEqGrades.GetItemChecked(i))
                            {
                                if (EqGrades != "")
                                {
                                    EqGrades = EqGrades + "," + lstEqGrades.Items[i].ToString();
                                }
                                else
                                {
                                    EqGrades = lstEqGrades.Items[i].ToString();
                                }
                            }
                        }
                        S.Eq_Grades = EqGrades;

                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.QA_Mtrl_Grade_Masters.InsertOnSubmit(S);
                        db.SubmitChanges();
                    }
                }
                MessageBox.Show("Record Saved / Updated Successfully");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public void bindMasters()
        {
            try
            {
                //Status
                var pStatus = (from m in db.QA_Color_Codes  select new { m.id, m.Colour_Code }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbColorCode.DataSource = pStatus;
                    cmbColorCode.ValueMember = "id";
                    cmbColorCode.DisplayMember = "Colour_Code";
                }

                


                using (SqlCommand cmd = new SqlCommand("SELECT distinct StandardCode,id FROM [Mtrl_Standards] where company_id = @CompName  order by id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstEqStnds.Items.Add(dt.Rows[i]["StandardCode"].ToString());
                            }

                        }
                    }
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

        private void cmbMaterial_Leave(object sender, EventArgs e)
        {
            try
            {
               
                using (SqlCommand cmd = new SqlCommand("SELECT distinct Material_Grade,id FROM MaterialGrades where company_id = @CompName  order by id", con))
                {
                    cmd.CommandType = CommandType.Text;                   
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstEqGrades.Items.Add(dt.Rows[i]["Material_Grade"].ToString());
                            }

                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                ioneNet.Qulaity_Management.frmQA_MaterialGradesList obj = new ioneNet.Qulaity_Management.frmQA_MaterialGradesList();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    //txtGradeName.Text = ioneNet.Qulaity_Management.MaterialGradesList.Voucherno;
                    var sa = (from sq in db.QA_Mtrl_Grade_Masters
                              where sq.Company_ID == logIn.company && sq.Material_Grade == txtGradeName.Text
                              select new
                              {
                                  sq.Material_Grade_ShortCode,
                                  sq.ColorCode,
                                  sq.Density,
                                  sq.HardNess,
                                  sq.Ref_Standard,
                                  sq.Eq_Grades ,
                                  sq.id
                              }).ToList();
                    if (sa.Count > 0)
                    {
                        int M_id = sa[0].id;
                        
                        cmbColorCode.SelectedValue = sa[0].ColorCode;
                        txtDensity.Text = sa[0].Density.ToString();

                        ////Get Test Specs of selected Grade (Chemical)

                        //var dm1 = (from s in db.QA_Test_Parameters
                        //           join p in db.QA_Mtrl_Grade_Test_Specs on  
                        //           s.id equals p.Parameter_ID into ps
                        //           from p in ps.DefaultIfEmpty()
                        //           where s.Test_Group == "Chemical" && p.Mtrl_Grade_Id == M_id
                        //           select new
                        //           { s.id, Parameter = s.Parameter_ShortCode, UOM = s.Parameter_Uom, p.Spec_Min, p.Spec_Max });

                        //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        //DataTable dtr = new DataTable();
                        //da2.Fill(dtr);
                        //if (dtr.Rows.Count > 0)
                        //    dgChemicalProd.DataSource = dtr;


                        //var dm2 = (from s in db.QA_Test_Parameters
                        //           join p in db.QA_Mtrl_Grade_Test_Specs on s.id equals p.Parameter_ID into ps
                        //           from p in ps.DefaultIfEmpty()
                        //           where s.Test_Group == "Mechanical" && p.Mtrl_Grade_Id == M_id
                        //           select new
                        //           { Mech_ID = s.id, Mech_Parameter = s.Parameter_ShortCode, Mech_UOM = s.Parameter_Uom, Mech_Spec_Min = p.Spec_Min, Mech_Spec_Max = p.Spec_Max });

                        //SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                        //SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                        //DataTable dtr1 = new DataTable();
                        //da3.Fill(dtr1);
                        //if (dtr1.Rows.Count > 0)
                        //    dgMechanical.DataSource = dtr1;


                        //var da1 = (from qtp in db.SP_TSL_Get_Mtrl_TestSpecs(logIn.company, M_id, "Chemical") select qtp).ToList();
                        //    dgChemicalProd.DataSource = da1;

                        //Get Test Specs of selected Grade
                        //var da2 = (from qtp1 in db.SP_TSL_Get_Mtrl_TestSpecs(logIn.company, M_id, "Mechanical") 

                        //           select new { Mech_ID = qtp1.id, Mech_Parameter = qtp1.Parameter, Mech_UOM = qtp1.UOM, Mech_Spec_Min = qtp1.Spec_Min, Mech_Spec_Max = qtp1.Spec_Max } ).ToList();

                        //dgMechanical.DataSource = da2;



                        //Add Grades based on group
                        using (SqlCommand cmd = new SqlCommand("SELECT distinct Material_Grade,id FROM QA_Mtrl_Grade_Master where company_id = @CompName  order by id", con))
                        {
                            cmd.CommandType = CommandType.Text;
                            
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        lstEqGrades.Items.Add(dt.Rows[i]["Material_Grade"].ToString());
                                    }

                                }
                            }
                        }

                        

                        if (sa[0].Ref_Standard != null)
                        {
                            string MP = sa[0].Ref_Standard.ToString();
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                for (int i = 0; i < lstEqStnds.Items.Count; i++)
                                {
                                    if (lstEqStnds.Items[i].ToString() == m)
                                    {
                                        lstEqStnds.SetItemChecked(i, true);
                                    }
                                }
                            }
                        }

                        if (sa[0].Eq_Grades != null)
                        {
                            string MP = sa[0].Eq_Grades.ToString();
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                for (int i = 0; i < lstEqGrades.Items.Count; i++)
                                {
                                    if (lstEqGrades.Items[i].ToString() == m)
                                    {
                                        lstEqGrades.SetItemChecked(i, true);
                                    }
                                }
                            }
                        }

                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BindEdit()
        {
            int M_id = Convert.ToInt32(ioneNet.Qulaity_Management.frmQA_MaterialGradesList.Grade_ID);
            txtGradeID.Text = M_id.ToString();
            var sa = (from sq in db.QA_Mtrl_Grade_Masters
                      where sq.Company_ID == logIn.company && sq.id == M_id
                      select new
                      {
                          sq.Material_Grade_ShortCode,
                          sq.ColorCode,
                          sq.Density,
                          sq.HardNess,
                          sq.Ref_Standard,
                          sq.Eq_Grades,
                          sq.Material_Grade
                      }).ToList();
            if (sa.Count > 0)
            {

                txtGradeName.Text = sa[0].Material_Grade;
                cmbColorCode.SelectedValue = sa[0].ColorCode;
                txtDensity.Text = sa[0].Density.ToString();
                txtHardNess.Text = sa[0].HardNess.ToString();
                txtGradeShortCode.Text = sa[0].Material_Grade_ShortCode;
                //Add Grades based on group
                using (SqlCommand cmd = new SqlCommand("SELECT distinct Material_Grade,id FROM QA_Mtrl_Grade_Master where company_id = @CompName  order by id", con))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstEqGrades.Items.Add(dt.Rows[i]["Material_Grade"].ToString());
                            }

                        }
                    }
                }



                if (sa[0].Ref_Standard != null)
                {
                    string MP = sa[0].Ref_Standard.ToString();
                    string[] values = MP.Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                        string m = values[j].ToString();
                        for (int i = 0; i < lstEqStnds.Items.Count; i++)
                        {
                            if (lstEqStnds.Items[i].ToString() == m)
                            {
                                lstEqStnds.SetItemChecked(i, true);
                            }
                        }
                    }
                }

                if (sa[0].Eq_Grades != null)
                {
                    string MP = sa[0].Eq_Grades.ToString();
                    string[] values = MP.Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                        string m = values[j].ToString();
                        for (int i = 0; i < lstEqGrades.Items.Count; i++)
                        {
                            if (lstEqGrades.Items[i].ToString() == m)
                            {
                                lstEqGrades.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
        }
    }
}
