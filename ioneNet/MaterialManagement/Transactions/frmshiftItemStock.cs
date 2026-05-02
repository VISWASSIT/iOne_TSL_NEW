using Ione_DAL;
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

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmshiftItemStock : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmshiftItemStock()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmshiftItemStock_Load(object sender, EventArgs e)
        {
            try
            {
                var Buyerblind = (from m in db.Products
                                 
                                  where m.Company_ID == logIn.company                                  
                                  select new { m.prod_ID, m.Prod_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbMasterItem.DataSource = Buyerblind;
                    cmbMasterItem.ValueMember = "prod_ID";
                    cmbMasterItem.DisplayMember = "Prod_Name";

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                cmbMasterItem.SelectedIndex = -1;

                var prodbind = (from m in db.Products
                                 
                                  where m.Company_ID == logIn.company                                  
                                  select new { m.prod_ID, m.Prod_Name }).Distinct().ToList();
                if (prodbind.Count > 0)
                {
                    cmbTransferItem.DataSource = prodbind;
                    cmbTransferItem.ValueMember = "prod_ID";
                    cmbTransferItem.DisplayMember = "Prod_Name";

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                cmbTransferItem.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmdIssue = new SqlCommand("UpdateProdToProd", con);
            cmdIssue.CommandType = CommandType.StoredProcedure;
            cmdIssue.Parameters.AddWithValue("@compname", logIn.company);
            cmdIssue.Parameters.AddWithValue("@prodid_Master", cmbMasterItem.SelectedValue);
            cmdIssue.Parameters.AddWithValue("@prodid_ToTranser", cmbTransferItem.SelectedValue);
            con.Open();
            cmdIssue.ExecuteNonQuery();
            con.Close();
            //SqlDataAdapter daIssue = new SqlDataAdapter(cmdIssue);
            //DataTable dsIssue = new DataTable();
            //daIssue.Fill(dsIssue);
        }
    }
}
