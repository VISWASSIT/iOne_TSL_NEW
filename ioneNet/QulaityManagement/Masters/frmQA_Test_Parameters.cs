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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.Qulaity_Management.Masters
{
    public partial class frmQA_Test_Parameters : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmQA_Test_Parameters()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd1 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                if (txtID.Text != "")
                {

                    int ProdID = Convert.ToInt32(txtID.Text);
                    //SqlCommand cmd2 = new SqlCommand("delete  from [QA_Test_Parameters] where id =@ProdID", con);
                    //cmd2.Parameters.AddWithValue("@ProdID", ProdID);
                    //cmd2.ExecuteNonQuery();
                    var upara = db.QA_Test_Parameters.Single(course => course.id == ProdID);
                    upara.Test_Parameter = txtParameterName.Text;
                    upara.Parameter_ShortCode = txtshortCode.Text;
                    upara.Parameter_Uom = txtUOM.Text;
                    upara.Test_Group = cmbTestGroup.Text;
                    upara.Show_In_TC = checkBox1.Checked;
                    upara.Company_id = logIn.company;                    
                    upara.Modified_By = logIn.username;
                    upara.Display_Order = Convert.ToInt32(txtDisplayOrder.Text);
                    db.SubmitChanges();

                }
                else
                {
                    cmd1.CommandText = "INSERT INTO QA_Test_Parameters  (Test_Parameter, Parameter_ShortCode,Parameter_Uom,Test_Group,Company_ID,Created_By,Modified_By,Display_Order,Show_In_TC) VALUES  (@Parameter_Name, @Parameter_ShortCode,@Parameter_UOM,@Test_Group,@Company_ID,@Created_By,@Modified_By,@Display_Order,@Show_TC)";
                    cmd1.Parameters.AddWithValue("@Parameter_Name", txtParameterName.Text);
                    cmd1.Parameters.AddWithValue("@Parameter_ShortCode", txtshortCode.Text);
                    cmd1.Parameters.AddWithValue("@Parameter_UOM", txtUOM.Text);
                    cmd1.Parameters.AddWithValue("@Test_Group", cmbTestGroup.Text);                   
                    cmd1.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd1.Parameters.AddWithValue("@Created_By", logIn.username);
                    cmd1.Parameters.AddWithValue("@Modified_By", logIn.username);
                    cmd1.Parameters.AddWithValue("@Display_Order", Convert.ToInt32(txtDisplayOrder.Text));
                    cmd1.Parameters.AddWithValue("@Show_TC", checkBox1.Checked);
                    cmd1.ExecuteNonQuery();
                }

                
                con.Close();

                txtParameterName.Text = "";
                txtUOM.Text = "";
                txtshortCode.Text = "";
                cmbTestGroup.Text = "";
                txtID.Text = "";
                txtDisplayOrder.Text = "";
                bindGroups();
                
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
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    int ProdID = Convert.ToInt32(txtID.Text.ToString());
                    SqlCommand cmd2 = new SqlCommand("delete  from QA_Test_Parameters where id =@ProdID", con);
                    cmd2.Parameters.AddWithValue("@ProdID", ProdID);
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Parameter Deleted Successfully");

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

        private void frmForge_Test_Parameters_Load(object sender, EventArgs e)
        {
            bindGroups();
        }
        public void bindGroups()
        {
            try
            {

                


                var p = (from s in db.QA_Test_Parameters
                         where  s.Company_id == logIn.company orderby s.Test_Group,s.Display_Order
                         select new
                         {
                             s.id,
                             s.Display_Order,
                             s.Test_Parameter,
                             s.Parameter_ShortCode,
                             s.Parameter_Uom,
                             s.Test_Group



                         }
                        ).ToList();
               
                if (p.Count >= 0)
                {
                    sfDataGrid1.DataSource = p;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Test_Parameter"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Test_Parameter"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Test_Parameter"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Test_Parameter"].FilterRowCondition = FilterRowCondition.Contains;
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

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
                txtID.Text = cellVaue.ToString();
                var d = (from po in db.QA_Test_Parameters where
                         po.id == Convert.ToInt32(txtID.Text) 
                         select new
                         {
                             po.Test_Parameter,
                             po.Parameter_ShortCode,
                             po.Parameter_Uom,
                             po.Test_Group   ,
                             po.Display_Order

                         }).ToList();
                if (d.Count > 0)
                {

                    txtParameterName.Text = d[0].Test_Parameter.ToString();
                    txtshortCode.Text = d[0].Parameter_ShortCode.ToString();
                    txtUOM.Text = d[0].Parameter_Uom.ToString();
                    cmbTestGroup.Text = d[0].Test_Group.ToString();
                    txtDisplayOrder.Text = d[0].Display_Order.ToString();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtParameterName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtParameterName.Text != "")
                {
                    
                    if (txtID.Text == "")
                    {

                        if ((from u in db.QA_Test_Parameters where u.Test_Parameter == txtParameterName.Text select u).Count() > 0)
                        {
                            MessageBox.Show("Partameter Name Cannot Be Duplicate", "Test Parameters", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtParameterName.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parameter Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
