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
    public partial class frmInspectionReport_Pharma : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string GRN_NO, ItemCode, RFNo, MtrlGrade, reportNo;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var da1 = (from obj in db.Bloom_Roll_Wise_Receipts
                               join s in db.Products on obj.Prod_ID equals s.prod_ID                             
                               join g in db.GoodsReceiptNote_Childs on new { x1 = obj.Grn_ID, x2 = obj.Company_ID, x3 = obj.Prod_ID } equals new { x1 = g.Grn_NO, x2 = g.Company_ID, x3 = g.Prod_Code }
                               where g.GRN_Master_ID == Convert.ToInt32(txtGRNID.Text) && s.prod_ID == Convert.ToInt32(comboBox2.SelectedValue)                  
                           select new { obj.RollNo }).Distinct().ToList();

            if (da1.Count > 0)
            {
                comboBox2.DataSource = da1;
                comboBox2.ValueMember = "RollNo";
                comboBox2.DisplayMember = "RollNo";
            }

        }

        public frmInspectionReport_Pharma()
        {
            InitializeComponent();
        }

        private void frmInspectionReport_Pharma_Load(object sender, EventArgs e)
        {

        }

        public void GetGRNData()
        {
            try
            {
                if (txtGRNNo.Text != "")
                {
                    var da = (from obj in db.GoodsReceiptNote_Masters
                              join s in db.Supplier_informations on obj.SupplierName equals s.ID
                              where obj.Grn_NO == txtGRNNo.Text && obj.BU_ID == logIn.BU_ID
                              select new { obj.Supplier_InvNo, s.Supplier_Name, grnid = obj.Id }).ToList();

                    if (da.Count > 0)
                    {
                        txtSuppName.Text = da[0].Supplier_Name;
                        txtDCNo.Text = da[0].Supplier_InvNo;
                        txtGRNID.Text = da[0].grnid.ToString();

                    }

                    var pStatus = (from m in db.GoodsReceiptNote_Childs                                   
                                   join r in db.Products
                                   on m.Prod_Code equals r.prod_ID
                                   where m.GRN_Master_ID ==  Convert.ToInt32(txtGRNID.Text)
                                   select new { r.prod_ID, r.Prod_Name }).Distinct().ToList();
                    if (pStatus.Count > 0)
                    {
                        comboBox1.DataSource = pStatus;
                        comboBox1.ValueMember = "prod_ID";
                        comboBox1.DisplayMember = "Prod_Name";
                    }
                    


                    //var da1 = (from obj in db.Bloom_Roll_Wise_Receipts
                    //           join s in db.Products on obj.Prod_ID equals s.prod_ID                             
                    //           join g in db.GoodsReceiptNote_Childs on new { x1 = obj.Grn_ID, x2 = obj.Company_ID, x3 = obj.Prod_ID } equals new { x1 = g.Grn_NO, x2 = g.Company_ID, x3 = g.Prod_Code }
                    //           where obj.Grn_ID == txtGRNNo.Text && obj.BU_ID == logIn.BU_ID
                    //           select new
                    //           {
                    //               Prod_Code = obj.Prod_ID,
                    //               Item_Grade = m.Material_Grade,
                    //               TC_No = g.Heat_No,
                    //               RM_Sec = s.Prod_Customer_Code,
                    //               RM_Length = obj.QtyinSqMtrs,
                    //               RF_No = obj.RollNo,
                    //           });
                    //SqlCommand cmd2 = (SqlCommand)db.GetCommand(da1);
                    //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataTable dtr = new DataTable();
                    //da2.Fill(dtr);
                    //if (dtr.Rows.Count >= 0)
                    //    comboBox1.DataSource = dtr;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtGRNNo_Leave(object sender, EventArgs e)
        {
            GetGRNData();
        }
    }
}
