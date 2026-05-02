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
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class MRP_Search : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public MRP_Search()
        {
            InitializeComponent();
        }
        
        public static string FInname, receipcode;

        private void BomSearch_Load(object sender, EventArgs e)
        {
            try
            {
                var sa = (from a in db.MRPLists where a.company_id == logIn.company                      
                          select new {
                             a.MRPNo,
                             a.MRP_Date,
                             a.MO_No,
                             a.Project_Code,
                             a.Prod_Group_Name,
                             a.Project_qty }).Distinct().ToList();
                if(sa.Count>0)
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

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FInname = dgvBOMData.Rows[e.RowIndex].Cells["MRPNo"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }
    }
}
