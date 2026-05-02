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
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using Ione_DAL;

namespace ioneNet.OrderManagement.Masters
{
    public partial class frmPriceSetup : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmPriceSetup()
        {
            InitializeComponent();
        }

        private void frmPriceSetup_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            var Buyerblind = (from m in db.Product_Groups
                              join p in db.Products on m.ID equals p.Prod_Group_Id
                              where m.Company_ID == logIn.company && p.Prod_IsBOM_Item == true
                              select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
            if (Buyerblind.Count > 0)
            {
                cmbProdGroup.DataSource = Buyerblind;
                cmbProdGroup.ValueMember = "ID";
                cmbProdGroup.DisplayMember = "Prod_Group_Name";
            }

            var bStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
            if (bStatus.Count > 0)
            {
                cmbStatus.DataSource = bStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (cmbStorageLocation.Text == "")
                //{
                //    MessageBox.Show("Ware House / Storage Location Should not be empty");
                //    cmbStorageLocation.Focus();
                //    return;
                //}
                Save();
            }
            catch (Exception)
            {


            }
        }

        private void Save()
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                Price_Setup s = new Price_Setup();
                s.Prod_Group = Convert.ToInt32(cmbProdGroup.SelectedValue.ToString());
                s.Effective_Date = dateTimePicker1.Value;
                s.Grade = (dataGridView1.Rows[i].Cells["Grade"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Grade"].Value).ToString();
                s.Basic_price = (dataGridView1.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Basic_Price"].Value);
                s.Disc_max = (txtDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDiscount.Text);
                s.Spl_Charges = (txtSplCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSplCharges.Text);
                s.Loading_Charges = (txtLoadingCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtLoadingCharges.Text);
                s.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                s.Remarks = (txtRemarks.Text == "") ? "" : txtRemarks.Text;
                s.Company_ID = logIn.company;
                s.BU_ID = logIn.BU_ID;
                s.Created_By = lblCreatedBy.Text;
                s.Modified_BY = logIn.username + "-" + DateTime.Now;

                db.Price_Setups.InsertOnSubmit(s);

            }
            db.SubmitChanges();
            //transaction.Commit();               
            MessageBox.Show("Record Saved / Updated Successfully " );
            this.Close();
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("Get_GradeData_PriceSetup", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);

                cmd2.Parameters.AddWithValue("@ProdGroup", Convert.ToInt32(cmbProdGroup.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                dataGridView1.DataSource = ds2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {

        }
    }
}
