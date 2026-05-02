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
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmEngg_Production_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname, var, RCode;
        public static int iTem_Code;

        public frmEngg_Production_Report()
        {
            InitializeComponent();
        }

        

        private void frmEngg_Production_Report_Load(object sender, EventArgs e)
        {
            try
            {

                //Reindexing voucher numbers
                //DateTime dtt = dtpToDate.Value;
                //string dt2 = dtt.ToString("yyyy/MM/dd");

                //var Prodname = (from d in db.Production_Reports
                //                where d.BU_ID == logIn.BU_ID && d.Voucher_Date >= logIn.fy_Start_Date && d.Voucher_Date <= logIn.fy_End_Date
                //                orderby d.Voucher_Date
                //                select new { d.BOM_Receipe_Code, d.Voucher_No }).ToList();
                //// DataTable dt = new DataTable();
                ////dt.Columns.Add("Raw_Material");
                //string dt4 = "";
                //foreach (var item in Prodname)
                //{
                //    var sa = db.Sp_autoincrement_ProductionVoucher(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date).FirstOrDefault().So_no;
                    

                //    var deleteproduct = db.Production_Reports.Single(course => course.Voucher_No == item.Voucher_No);
                    
                //    deleteproduct.BOM_Receipe_Code = sa;
                    
                //    db.SubmitChanges();
                //}



                    dpdate.MinDate = logIn.fy_Start_Date;
                dpdate.MaxDate = logIn.fy_End_Date;
                
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                var s = (from a in db.Products
                         where a.Company_ID == logIn.company && a.Prod_Status_ID ==1
                         select new { a.Prod_Name, a.prod_ID }).ToList();

                if (s.Count > 0)
                {
                    cmbpname.DataSource = s;
                    cmbpname.ValueMember = "prod_ID";
                    cmbpname.DisplayMember = "Prod_Name";
                    if (cmbpname.Items.Count > 0)
                    {
                        cmbpname.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbpname.SelectedIndex = -1;
                    }
                }
                txtMONo.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtMONo.AutoCompleteSource = AutoCompleteSource.CustomSource;
                if (frmEngg_Prod_Report_List.editMode == true)
                {
                    bindedit();
                    FrmForge_JobCardList.editMode = false;

                }
                else
                {

                    autoincrement();
                }
                // var sa=db.au
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void cmbpname_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void txtMONo_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddMO(DataColl);
                txtMONo.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtMONo_Leave(object sender, EventArgs e)
        {
            try
            {
                if(txtMONo.Text!="" && txtMONo.Text !="NA")
                {
                    var ProcID = (from s in db.Engg_Mfg_Orders
                                  where s.MO_No == txtMONo.Text
                                  select new { s.MO_Qty,s.Project_Code,s.Project_ID }).ToList();

                    txtMOQty.Text = ProcID[0].MO_Qty.ToString();
                    txtProjectCode.Text = ProcID[0].Project_Code.ToString();
                    textBox5.Text = ProcID[0].Project_ID.ToString();
                }
                else
                {
                    txtMOQty.Text = "0";
                    txtProjectCode.Text = "NA";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbpname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbpname.Text != "")
                {
                    var sa = (from a in db.Products
                              join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                              where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company
                              && a.Prod_Status_ID ==1
                              select new { a.Prod_Name, u.Uom_Descr, a.Prod_Code }).ToList();
                    if (sa.Count > 0)
                    {

                        txtuom.Text = sa[0].Uom_Descr;
                        txtProdCode.Text = sa[0].Prod_Code.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((from a in db.Production_Reports where a.Company_ID == logIn.company && a.Voucher_No == txtvchno.Text && a.BU_ID == logIn.BU_ID  select a).Count() > 0)
                {
                    db.Sp_delete_Production(logIn.company, txtvchno.Text,logIn.BU_ID);

                    Production_Report pb = new Production_Report();
                    pb.Voucher_No = txtvchno.Text;
                    pb.Voucher_Date = dpdate.Value;
                    pb.FG_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                    pb.Machine_ID = Convert.ToInt32(textBox5.Text);
                    pb.Item_Name = txtProdCode.Text;
                    pb.FG_Cateogry = (txtProjectCode.Text == null) ? "" : txtProjectCode.Text;
                    pb.FG_UOM = (txtuom.Text == null) ? "" : txtuom.Text;
                    pb.Batch_No = (txtMONo.Text == null) ? "" : txtMONo.Text;
                    pb.Prod_Qty = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                    pb.Qty_Accepted = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                    pb.QtyReq = (txtMOQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtMOQty.Text);
                    pb.Avg_Price = (txtAvgPrice.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtAvgPrice.Text);
                    pb.Company_ID = logIn.company;
                    pb.BU_ID = logIn.BU_ID;
                    //pb.Created_By = lnkus1;
                    pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Production_Reports.InsertOnSubmit(pb);
                    db.SubmitChanges();

                    MessageBox.Show("Record Updated Sucessfully");
                    clear();
                    return;
                }
                else
                {
                    autoincrement();
                    Production_Report pb = new Production_Report();
                    pb.Voucher_No = txtvchno.Text;
                    pb.Voucher_Date = dpdate.Value;
                    pb.FG_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                    pb.Item_Name = txtProdCode.Text;
                    pb.FG_Cateogry = (txtProjectCode.Text == null) ? "" : txtProjectCode.Text;
                    pb.Machine_ID = (textBox5.Text == "") ? Convert.ToInt32(0): Convert.ToInt32(textBox5.Text);
                    pb.FG_UOM = (txtuom.Text == null) ? "" : txtuom.Text;
                    pb.Batch_No = (txtMONo.Text == null) ? "" : txtMONo.Text;
                    pb.Prod_Qty = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                    pb.Qty_Accepted = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                    pb.Avg_Price = (txtAvgPrice.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtAvgPrice.Text);
                    pb.QtyReq = (txtMOQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtMOQty.Text);
                    pb.Company_ID = logIn.company;
                    pb.BU_ID = logIn.BU_ID;
                    pb.Created_By = logIn.username + "-" + DateTime.Now;
                    pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Production_Reports.InsertOnSubmit(pb);
                    db.SubmitChanges();                    
                    //db.SubmitChanges();
                    MessageBox.Show("Record Saved Sucessfully");
                    clear();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindedit()
        {
            try
            {
                // DateTime dtDel;
                txtvchno.Text = ProductionManagement.Transactions.frmEngg_Prod_Report_List.SO_No;
                String myString = "";
                myString = txtvchno.Text;
                var da = (from obj in db.Production_Reports
                          where obj.Voucher_No == txtvchno.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtvchno.Text = da[0].Voucher_No.ToString();
                    dpdate.Text = da[0].Voucher_Date.ToString();
                    //bindCustomer();
                    cmbpname.SelectedValue = da[0].FG_Item_ID;
                    
                    txtProdCode.Text = da[0].Item_Name;
                    textBox5.Text = da[0].Machine_ID.ToString();
                    txtuom.Text = da[0].FG_UOM;
                    txtMONo.Text = da[0].Batch_No.ToString();
                    txtProjectCode.Text = da[0].FG_Cateogry.ToString();
                    txtMOQty.Text = da[0].QtyReq.ToString();
                    txtnoofbatchs.Text = da[0].Prod_Qty.ToString();
                    txtAvgPrice.Text = da[0].Avg_Price.ToString();                    
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                   
                }


             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void autoincrement()
        {
            var sa = db.Sp_autoincrement_ProductionVoucher(logIn.company,logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date).FirstOrDefault().So_no;
            txtvchno.Text = sa;

        }
        public void AddMO(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Engg_Mfg_Orders where d.Company_ID == logIn.company


                                select new { d.MO_No }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("MO_No");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.MO_No);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void clear()
        {
            txtvchno.Text = "";
            dpdate.Value = DateTime.Now;
        }
    }
}
