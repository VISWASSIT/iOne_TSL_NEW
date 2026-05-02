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
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.Qulaity_Management.Masters
{
    public partial class frmQA_ColourCode : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmQA_ColourCode()
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
                if (txtParameterName.Text == string.Empty)
                {
                    MessageBox.Show("Colour Name should Not be Empty", "Color Codes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtParameterName.Focus();
                    return;
                }

                else
                 if (txtshortCode.Text == string.Empty)
                {
                    MessageBox.Show("Colour Short Code should Not be Empty", "Color Codes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtshortCode.Focus();
                    return;
                }

                else
                {
                    Save();
                    txtParameterName.Text = "";
                    txtshortCode.Text = "";
                    txtID.Text = "";
                    bindGroups();
                }
                
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
                    SqlCommand cmd2 = new SqlCommand("delete  from [QA_Color_Codes] where id =@ProdID", con);
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

                


                var p = (from s in db.QA_Color_Codes
                         


                         select new
                         {
                             s.id,
                             s.Color_Description,
                             s.Colour_Code

                         }
                        ).ToList();
               
                if (p.Count >= 0)
                {
                    sfDataGrid1.DataSource = p;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Colour_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Colour_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Colour_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Colour_Code"].FilterRowCondition = FilterRowCondition.Contains;
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
                var d = (from po in db.QA_Color_Codes where
                         po.id == Convert.ToInt32(txtID.Text) 
                         select new
                         {
                             po.Color_Description,
                             po.Colour_Code
                           
                         }).ToList();
                if (d.Count > 0)
                {

                    txtParameterName.Text = d[0].Color_Description.ToString();
                    txtshortCode.Text = d[0].Colour_Code.ToString();
                   
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Save()
        {
            try
            {
                int storageid = (txtID.Text == null || txtID.Text == "") ? 0 : Convert.ToInt32(txtID.Text);
                if ((from u in db.QA_Color_Codes where u.id == storageid  select u).Count() > 0)
                {
                   
                    var c = db.QA_Color_Codes.Where(w => w.id == (Convert.ToInt32(txtID.Text.ToString()))).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Colour_Code  = (txtshortCode.Text == "") ? "" : (txtshortCode.Text);
                        c.Color_Description = (txtParameterName.Text == "") ? "" : (txtParameterName.Text);
                        //c.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                        
                          db.SubmitChanges();
                        MessageBox.Show("Record Upadated Successfully");
                    }
                    
                }
                else
                {
             
                    QA_Color_Code ci = new QA_Color_Code();
                    ci.Color_Description = (txtParameterName.Text == "") ? "" : (txtParameterName.Text);
                    ci.Colour_Code = (txtshortCode.Text == "") ? "" : (txtshortCode.Text);
                    db.QA_Color_Codes.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    
                    

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
    }
}
