using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Configuration;
using Syncfusion.Windows.Forms.Tools;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;

namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class EmployeeGroups : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;

        public EmployeeGroups()
        {
            InitializeComponent();
        }

        private void dgcity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // // // // City Names Grid Binding
        public void bindGroups()
        {
            try
            {
                var p = (from s in db.HR_Employee_Groups where s.Status == 1 && s.Company_ID == logIn.company


                         select new
                         {
                             ID=s.ID,
                             Group_Name=s.Group_Name
                            
                             

                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    sfDataGrid1.DataSource = dt1;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Group_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Group_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Group_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Group_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    

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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGroupName.Text == string.Empty)
                {
                    MessageBox.Show("Group Name should Not be Empty", "Group Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroupName.Focus();
                    return;
                }
                else if (txtGroupShortName.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Group Shortname", "Group Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroupShortName.Focus();
                    return;
                }
              
                else
                {
                    Save();
                    
                    bindGroups();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
               
                    //if ((from u in db.Product_Groups where u.ID == Convert.ToInt32(txtGroupID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    if(txtGroupID.Text !="")
                {

                            //if (frmGate.Modify.Contains(this.Text))
                            //{

                            var c = db.HR_Employee_Groups.Where(w => w.ID == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                            c.Short_Name = (txtGroupShortName.Text == "") ? "" : (txtGroupShortName.Text);
                            c.Prefix = (txtPrefix.Text == "") ? "" : (txtPrefix.Text);
                            string heads = "";
                            foreach (VisualItem obj in this.multiSelectionComboBox1.VisualItems)
                            {

                                if (heads != "")
                                {

                                    heads = heads + "," + obj.Text;
                                }
                                else
                                {

                                    heads = obj.Text;
                                }
                            }

                        c.App_Heads = heads;

                        c.ESIApplicable = chkESI.Checked;
                        c.PFApplicable = chkPF.Checked;
                        c.Status = 1;
                           
                        c.Created_By = linkCreatedBy.Text;
                            c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                        clear1();
                        autogen();                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
                    //}
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                        HR_Employee_Group ci = new HR_Employee_Group();
                    ci.Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                    ci.Short_Name = (txtGroupShortName.Text == "") ? "" : (txtGroupShortName.Text);
                    ci.Prefix = (txtPrefix.Text == "") ? "" : (txtPrefix.Text);
                    string heads = "";
                    foreach (VisualItem obj in this.multiSelectionComboBox1.VisualItems)
                    {

                        if (heads != "")
                        {

                            heads = heads + "," + obj.Text;
                        }
                        else
                        {

                            heads = obj.Text;
                        }
                    }

                    ci.App_Heads = heads;

                    ci.ESIApplicable = chkESI.Checked;
                    ci.PFApplicable = chkPF.Checked;
                    ci.Status = 1;
                    ci.Company_ID = logIn.company;
                    ci.Created_By = linkCreatedBy.Text;
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    db.HR_Employee_Groups.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                    clear1();
                    bindGroups();
                    //autogen();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Save City");
                    //}

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

        private void frmCityMaster_Load(object sender, EventArgs e)
        {
           
            linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //Bind Heads
            var pscrap = (from m in db.HR_Salary_HeadsInfos where m.Company_ID == logIn.company select new { m.id, m.Short_Name }).Distinct().ToList();
            if (pscrap.Count > 0)
            {
                multiSelectionComboBox1.DataSource = pscrap;
                multiSelectionComboBox1.ValueMember = "id";
                multiSelectionComboBox1.DisplayMember = "Short_Name";
                multiSelectionComboBox1.SelectedIndex = -1;
            }

            txtGroupName.Focus();
            bindGroups();
            bindmethod();
            //autogen();      
        }
        public void autogen()
        {
            //var result = db.Sp_autoincrement_ProdGroup_Master(logIn.company);
            //txtGroupID.Text = result.FirstOrDefault().Prod_Group_ID;
        }
        public void bindmethod()
        {
            try
            {

                //SqlCommand cmd = new SqlCommand("select  Distinct Prod_Group_Name,Prod_Group_ID from Product_Groups WHERE [Company_ID] = @CompID", con);
                //cmd.Parameters.AddWithValue("@CompID", logIn.company);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                //da.Fill(ds, "t");
                //DataRow drow = ds.Tables["t"].NewRow();
                //drow["Prod_Group_Name"] = "Primary";
                //ds.Tables["t"].Rows.InsertAt(drow, 0);
                //cmbMainGroup.DataSource = ds.Tables["t"];
                //cmbMainGroup.DisplayMember = "Prod_Group_Name";
                //cmbMainGroup.ValueMember = "Prod_Group_ID";
                //cmbMainGroup.SelectedIndex = 0;


                //var bindMainGroups = (from m in db.HR_Employee_Groups
                //                      where m.Company_ID == logIn.company && m.Status_ID == 1
                //                      select new
                //                      {
                //                          m.Prod_Group_Name,
                //                          m.ID,
                //                      }).ToList();

                //if (bindMainGroups.Count > 0)
                //{
                //    cmbMainGroup.DisplayMember = "Prod_Group_Name";
                //    cmbMainGroup.ValueMember = "ID";
                //    cmbMainGroup.DataSource = bindMainGroups;

                //}

                //var bindProdTypes = (from m in db.Attributes_Prod_Types
                //                      where m.Company_ID == logIn.company 
                //                      select new
                //                      {
                //                          m.Prod_Type,
                //                          m.Prod_Type_Id,
                //                      }).ToList();

                //if (bindProdTypes.Count > 0)
                //{
                //    comboBox1.DisplayMember = "Prod_Type";
                //    comboBox1.ValueMember = "Prod_Type_Id";
                //    comboBox1.DataSource = bindProdTypes;

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvcity_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void clear1()
        {
            foreach (Control x in this.Controls)
            {
                foreach (Control d in panel1.Controls)
                {
                    if (d is TextBox)
                        (d as TextBox).Clear();
                    if (d is ComboBox)
                        (d as ComboBox).SelectedIndex = -1;
                    if (d is CheckBox)
                        (d as CheckBox).Checked = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           try
           {
                if (txtGroupID.Text != "")
                {

                        DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            int ProdID;
                            ProdID = Convert.ToInt32(txtGroupID.Text);
                            SqlCommand cmd1 = new SqlCommand("delete  from [HR_Employee_Groups] where id =@ProdID and Company_ID = @compID", con);
                            cmd1.Parameters.AddWithValue("@ProdID", ProdID);
                            cmd1.Parameters.AddWithValue("@compID", logIn.company);

                        if (con.State != ConnectionState.Open)
                                con.Open();
                            //con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Employee Group Deleted Successfully");
                        }

                }
                else
                {
                    MessageBox.Show("Please Select Product Group to Delete");
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    MessageBox.Show("The Master Record Already in Use, Cannot Be Deleted");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }                
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
            bindGroups();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
              //  txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                txtGroupID.Text = currentCellValue.ToString();
                var d = (from po in db.HR_Employee_Groups
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Group_Name,
                             po.Short_Name,                            
                             po.Prefix,
                             po.Created_By,
                             po.Modified_BY,   
                             po.App_Heads,
                             po.PFApplicable,
                             po.ESIApplicable
                                                

                         }).ToList();
                if (d.Count > 0)
                {
                  
                    txtGroupName.Text = d[0].Group_Name;
                    txtPrefix.Text = d[0].Prefix;
                    txtGroupShortName.Text = d[0].Short_Name;                    
                    chkESI.Checked = false;
                    chkPF.Checked = false;
                    if (d[0].PFApplicable == true)
                    {
                        chkPF.Checked = true;
                    }
                    if (d[0].ESIApplicable == true)
                    {
                        chkESI.Checked = true;
                    }
                    if (d[0].App_Heads != null)
                    {
                        string MP = d[0].App_Heads.ToString();
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

                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_BY;                   
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtGroupname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtGroupName.Text != "")
                {
                    if ((from u in db.Product_Groups where u.Prod_Group_Name == txtGroupName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("Group Name Cannot Be Duplicate");
                        txtGroupName.Text = "";
                        txtGroupName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCityMaster_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.Control && e.KeyCode == Keys.R)
                    btnReset_Click(sender, e);


                if (e.Control && e.KeyCode == Keys.S)
                    btnSubmit_Click(sender, e);

                if (e.Control && e.KeyCode == Keys.D)
                    btnDelete_Click(sender, e);

                if (e.Alt && e.KeyCode == Keys.F4)
                    btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
           
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                //  txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                txtGroupID.Text = currentCellValue.ToString();
                var d = (from po in db.HR_Employee_Groups
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Group_Name,
                             po.Short_Name ,
                             po.Prefix,
                             po.App_Heads,
                             po.Created_By,
                             po.Modified_BY,
                             po.AppDedHeads,
                             po.PFApplicable,
                             po.ESIApplicable


                         }).ToList();
                if (d.Count > 0)
                {                  

                    txtGroupName.Text = d[0].Group_Name.ToString();
                    txtGroupShortName.Text = d[0].Short_Name.ToString();
                    txtPrefix.Text = d[0].Prefix.ToString();
                    if (d[0].App_Heads != null)
                    {
                        string MP = d[0].App_Heads.ToString();
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

                    if (d[0].AppDedHeads != null)
                    {
                        string MP = d[0].AppDedHeads.ToString();
                        //multiSelectionComboBox1.Text = MP;
                        string[] values = MP.Split(',');



                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            multiSelectionComboBox2.AddVisualItem(m);

                            //   multiSelectionComboBox1.
                            //multiSelectionComboBox1.Text = m;

                        }
                    }


                    if (d[0].PFApplicable == true)
                    {
                        chkPF.Checked = true;
                    }
                    if (d[0].ESIApplicable == true)
                    {
                        chkESI.Checked = true;
                    }


                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_BY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
