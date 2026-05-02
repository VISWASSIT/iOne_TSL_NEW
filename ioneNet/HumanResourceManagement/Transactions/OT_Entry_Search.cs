using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ione_DAL;
namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class OT_Entry_Search : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string ModuleNo, MoNO, Proj_Code, Group_Name;
        public static string frmName;
        public OT_Entry_Search()
        {
            InitializeComponent();
        }

        public static string FInname, receipcode;

        private void BomSearch_Load(object sender, EventArgs e)
        {
            try
            {
                    var sa = (from a in db.HR_OT_Entries
                              where a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID
                             
                              select new
                              {
                                  a.Vch_no,
                                  a.e_Date,
                                  a.Emp_Dept,
                                  a.ISCoff                                 
                              }).Distinct().ToList();
                    if (sa.Count > 0)
                    {
                        dgvBOMData.DataSource = sa;
                    }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBOMData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                
                Proj_Code = dgvBOMData.Rows[e.RowIndex].Cells["Vch_no"].Value.ToString();
                

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }
    }
}
