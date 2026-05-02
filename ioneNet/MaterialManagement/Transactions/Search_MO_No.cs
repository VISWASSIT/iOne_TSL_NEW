using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class Search_MO_No : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string mo_no = "",project_code="",project_name="";
        public static string frmName;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();

        public Search_MO_No()
        {
            InitializeComponent();
        }

        private void search_MO_No_Load(object sender, EventArgs e)
        {
            var d = (from data in db.SP_getMO(logIn.company, logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["MO_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["MO_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["MO_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["MO_No"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Project_Code"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Project_Code"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Project_Code"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Project_Code"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Project_Description"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Project_Description"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Project_Description"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Project_Description"].FilterRowCondition = FilterRowCondition.Contains;

            }
            //binddata();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                mo_no = (sfDataGrid1.GetRecordAtRowIndex(0).GetType().GetProperty(sfDataGrid1.Columns["MO_No"].MappingName).GetValue(sfDataGrid1.GetRecordAtRowIndex(0), null).ToString()).ToString();
                project_code= (sfDataGrid1.GetRecordAtRowIndex(0).GetType().GetProperty(sfDataGrid1.Columns["Project_Code"].MappingName).GetValue(sfDataGrid1.GetRecordAtRowIndex(0), null).ToString()).ToString();
                project_name= (sfDataGrid1.GetRecordAtRowIndex(0).GetType().GetProperty(sfDataGrid1.Columns["Project_Description"].MappingName).GetValue(sfDataGrid1.GetRecordAtRowIndex(0), null).ToString()).ToString();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvcity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfDataGrid1_CellClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
           
        }

        public void binddata()
        {
            try
            {
                var p = (from s in db.Engg_Mfg_Orders
                         join m in db.Project_code_Masters on s.Project_ID equals m.id
                         where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             s.id,
                             s.MO_No,
                             s.Project_Code,
                             m.Project_Description

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
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
