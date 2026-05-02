using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;

namespace ioneNet.ProductionManagement
{
    public partial class frmForging_Production_MachiningList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static string Voucherno;
        public frmForging_Production_MachiningList()
        {
            InitializeComponent();
        }

        private void Productionvouchersearch_Load(object sender, EventArgs e)
        {
            try
            {
                var d = (from data in db.SP_ShowProdReportMachiningView(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dataGridView1.DataSource = d;                }



               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    Voucherno = dataGridView1.Rows[e.RowIndex].Cells["Voucher_No"].Value.ToString();


                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                var d = (from data in db.SP_ShowProdReportMachiningView(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, txtSearch.Text) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dataGridView1.DataSource = d;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
