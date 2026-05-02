using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using DAL;
//using BAL;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;

namespace ioneNet.ProductionManagement
{
    public partial class ProductionReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname, var,RCode;
        public static int iTem_Code;
        public ProductionReport()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductionVoucherBulk_Load(object sender, EventArgs e)
        {
            try
            {

                dpdate.MinDate = logIn.fy_Start_Date;
                dpdate.MaxDate = logIn.fy_End_Date;
                var sa = (from k in db.Supplier_informations select new { k.Supplier_Name,k.ID }).ToList();
                if (sa.Count > 0)
                {
                    cmbCustomer.DataSource = sa;
                    cmbCustomer.DisplayMember = "Supplier_Name";
                    cmbCustomer.ValueMember = "ID";
                    if (cmbCustomer.Items.Count > 0)
                    {
                        cmbCustomer.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbCustomer.SelectedIndex = -1;
                    }
                }
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                var s = (from a in db.Products
                          where a.Prod_IsBOM_Item == true && a.Company_ID == logIn.company
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
                //Machine ID
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Machine" &&  m.Company_ID==logIn.company select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbMachineID.DataSource = pStatus;
                    cmbMachineID.ValueMember = "ID";
                    cmbMachineID.DisplayMember = "Descr";
                }
                autoincrement();
                // var sa=db.au
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbpname_Leave(object sender, EventArgs e)
        {
            
        }
        public void clear()
        {
            txtvchno.Text = "";
            dpdate.Value = DateTime.Now;
            txttotalqty.Text = "";
            //txtbatchno.Text = "";
            //checkBox1.Checked = false;
            //cmbcustname.Text = "";
            //foreach (Control d in groupBox1.Controls)
            //{
            //    if (d is TextBox)
            //        (d as TextBox).Clear();
            //    if (d is ComboBox)
            //        (d as ComboBox).SelectedIndex = -1;
            //}

            if (dgrmconsumed.Rows.Count >= 1)
            {
                for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                {
                    dgrmconsumed.Rows.RemoveAt(i);
                    i--;
                    while (dgrmconsumed.Rows.Count == 0)
                        continue;
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txtnoofbatchs_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal SUMAmount = 0;
                if (txtnoofbatchs.Text != "" && txtnoofbatchs.Text != null)
                {
                    decimal Consumption;
                    decimal qtyacc = 0;
                    //txtQtyAccepted.Text = txtnoofbatchs.Text;

                    decimal batchsize = Convert.ToInt32(txtnoofbatchs.Text);
                    if (txtQtyAccepted.Text !="")
                    {
                        qtyacc = Convert.ToInt32(txtQtyAccepted.Text);
                    }
                    else
                    {
                        txtQtyAccepted.Text = txtnoofbatchs.Text;
                        qtyacc = Convert.ToInt32(txtQtyAccepted.Text);
                    }

                    for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                    {
                        var sa = (from a in db.BOMs where a.RM_Item_Name == dgrmconsumed.Rows[i].Cells["Item_Name"].Value && a.Bom_Item_ID == Convert.ToUInt32(cmbpname.SelectedValue) && a.Bom_ReceipeCode == cmbReceipe.Text && a.Company_ID == logIn.company select new { a.QtyReq, a.Comp_Per }).ToList();
                        if (sa.Count > 0)
                        {
                            var sa1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value) && a.Company_ID == logIn.company select new { a.Prod_Type_Id }).ToList();
                            decimal qty = Convert.ToDecimal(sa[0].QtyReq);
                            decimal ConPer = Convert.ToDecimal(sa[0].Comp_Per);
                            if (sa1[0].Prod_Type_Id != 1)
                            {
                                Consumption = qtyacc * qty;
                            }
                            else
                            {
                                Consumption = Math.Round((Convert.ToDecimal(txtnoofbatchs.Text) / Convert.ToDecimal(txtbacthsize.Text)) * qty, 3);
                                //  Consumption = Math.Round((batchsize * ConPer * Convert.ToDecimal(textBox3.Text)) / Convert.ToDecimal(textBox4.Text),3);
                            }
                            dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value = Consumption;
                            //Get Stock Report
                            decimal stockqty = Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyinStock"].Value);
                            if (stockqty < Consumption)
                            {
                                dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                            }
                            else
                            {
                                dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                            }
                        }
                        SUMAmount += (dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == "" || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value);

                    }
                }
                txttotalqty.Text = SUMAmount.ToString(".00");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void autoincrement()
        {
            var sa = db.Sp_autoincrement_ProductionVoucher(logIn.company,logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date).FirstOrDefault().So_no;
            txtvchno.Text = sa;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if ((from a in db.Production_Reports where a.Company_ID == logIn.company && a.Voucher_No == txtvchno.Text select a).Count() > 0)
                {
                  db.Sp_delete_Production(logIn.company,txtvchno.Text,logIn.BU_ID);
                    for (int i = 0; i < dgrmconsumed.Rows.Count-1; i++)
                    {
                        //if(dgrmconsumed.Rows[i].Cells["Item_ID"].Value.ToString() !=null)
                        //{

                       
                        Production_Report pb = new Production_Report();
                        pb.Voucher_No = txtvchno.Text;
                        pb.Voucher_Date = dpdate.Value;
                        pb.FG_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                        pb.FG_Cateogry = (txtcategory.Text == null) ? "" : txtcategory.Text;                      

                        pb.FG_UOM = (txtuom.Text == null) ? "" : txtuom.Text;
                        pb.BOM_Receipe_Code = cmbReceipe.Text;
                        pb.LotBatchsize = (txtbacthsize.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtbacthsize.Text);
                        //   pb.RmConsumedKg = (txtrmconsumed.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtrmconsumed.Text);
                        pb.Prod_Qty = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                        pb.Qty_Accepted = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                        pb.Scrap_Prod_ID= Convert.ToInt32(cmbScrapProduct.SelectedValue);
                        pb.Scrap_Qty= (txtScrapQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtScrapQty.Text);
                        if (txtProcessLoss.Text != "")
                        {
                            pb.Process_Loss_Per = (txtProcessLoss.Text == null && txtProcessLoss.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtProcessLoss.Text);
                        }
                        if (txtPowerOB.Text != "")
                        {
                            pb.Power_OB_Units = (txtPowerOB.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerOB.Text);
                        }
                        if (txtPowerCB.Text != "")
                        {
                            pb.Power_CB_Units = (txtPowerCB.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerCB.Text);
                        }
                        pb.JobWork_Production = chkJWProd.Checked;
                        pb.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                        pb.Toatl_Qty = (txttotalqty.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txttotalqty.Text);
                        pb.Item_ID = Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value.ToString());
                        pb.Item_Name = (dgrmconsumed.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["Item_Name"].Value.ToString();
                        pb.UOM = (dgrmconsumed.Rows[i].Cells["UOM"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["UOM"].Value.ToString();
                        pb.Group_Name = (dgrmconsumed.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["Group_Name"].Value.ToString();
                        pb.QtyReq = (dgrmconsumed.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyReq"].Value.ToString());
                        pb.QtyinStock = (dgrmconsumed.Rows[i].Cells["QtyinStock"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyinStock"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyinStock"].Value.ToString());
                        pb.QtyConsumed = (dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value.ToString());
                        pb.Avg_Price = (dgrmconsumed.Rows[i].Cells["Avg_Price"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["Avg_Price"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Avg_Price"].Value.ToString());
                        pb.Company_ID = logIn.company;
                        pb.BU_ID = logIn.BU_ID;
                        //pb.Created_By = lnkus1;
                        pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.Production_Reports.InsertOnSubmit(pb);
                        db.SubmitChanges();
                        }
                    //}

                    db.SubmitChanges();
                    MessageBox.Show("Record Updated Sucessfully");
                    clear();
                    return;
                }
                else
                {
                    autoincrement();
                    for (int i = 0; i < dgrmconsumed.Rows.Count-1; i++)
                    {
                        Production_Report pb = new Production_Report();
                        pb.Voucher_No = txtvchno.Text;
                        pb.Voucher_Date = dpdate.Value;
                        pb.FG_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                        pb.FG_Cateogry = (txtcategory.Text == null) ? "" : txtcategory.Text;

                        pb.FG_UOM = (txtuom.Text == null) ? "" : txtuom.Text;
                        pb.BOM_Receipe_Code = cmbReceipe.Text;
                        pb.LotBatchsize = (txtbacthsize.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtbacthsize.Text);
                        //   pb.RmConsumedKg = (txtrmconsumed.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtrmconsumed.Text);
                        pb.Prod_Qty = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                        pb.Qty_Accepted = (txtnoofbatchs.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtnoofbatchs.Text);
                        pb.Scrap_Prod_ID = Convert.ToInt32(cmbScrapProduct.SelectedValue);
                        pb.Scrap_Qty = (txtScrapQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtScrapQty.Text);
                        if (txtProcessLoss.Text != "")
                        {
                            pb.Process_Loss_Per = (txtProcessLoss.Text == null && txtProcessLoss.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtProcessLoss.Text);
                        }
                        if (txtPowerOB.Text != "")
                        {
                            pb.Power_OB_Units = (txtPowerOB.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerOB.Text);
                        }
                        if (txtPowerCB.Text != "")
                        {
                            pb.Power_CB_Units = (txtPowerCB.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerCB.Text);
                        }
                        //pb.JobWork_Production = chkJWProd.Checked;
                        //pb.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                        pb.Toatl_Qty = (txttotalqty.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txttotalqty.Text);
                        pb.Item_ID = Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value.ToString());
                        pb.Item_Name = (dgrmconsumed.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["Item_Name"].Value.ToString();
                        pb.UOM = (dgrmconsumed.Rows[i].Cells["UOM"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["UOM"].Value.ToString();
                        pb.Group_Name = (dgrmconsumed.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgrmconsumed.Rows[i].Cells["Group_Name"].Value.ToString();
                        pb.QtyReq = (dgrmconsumed.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyReq"].Value.ToString());
                        pb.QtyinStock = (dgrmconsumed.Rows[i].Cells["QtyinStock"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyinStock"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyinStock"].Value.ToString());
                        pb.QtyConsumed = (dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value.ToString());
                        pb.Company_ID = logIn.company;
                        pb.BU_ID = logIn.BU_ID;                        
                        pb.Created_By = logIn.username + "-" + DateTime.Now;
                        pb.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.Production_Reports.InsertOnSubmit(pb);
                        db.SubmitChanges();

                    }

                    db.SubmitChanges();
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

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionManagement.Productionvouchersearch obj = new Productionvouchersearch();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtvchno.Text = ProductionManagement.Productionvouchersearch.Voucherno;
                    var sa = (from sq in db.Production_Reports
                              where sq.Company_ID == logIn.company && sq.Voucher_No == txtvchno.Text
                              select new
                              { sq.Toatl_Qty,sq.Voucher_No, sq.Voucher_Date, sq.FG_Item_ID, sq.FG_Cateogry, sq.FG_UOM, sq.LotBatchsize,
                                  sq.Prod_Qty,sq.Prod_Shift,sq.Machine_ID,sq.BOM_Receipe_Code,sq.JobWork_Production,
                                  sq.Customer_Name,sq.Scrap_Prod_ID,sq.Scrap_Qty,sq.Qty_Accepted,
                              sq.Process_Loss_Per,
                                  sq.Power_OB_Units,sq.Power_CB_Units}).ToList();
                    if(sa.Count>0)
                    {
                        txtvchno.Text = sa[0].Voucher_No;
                        dpdate.Value = Convert.ToDateTime(sa[0].Voucher_Date);
                        cmbpname.SelectedValue = sa[0].FG_Item_ID;
                        txtcategory.Text = sa[0].FG_Cateogry;
                        txtuom.Text = sa[0].FG_UOM;
                        cmbReceipe.Text = sa[0].BOM_Receipe_Code;
                        txtbacthsize.Text = sa[0].LotBatchsize.ToString();
                        txtnoofbatchs.Text = Convert.ToDecimal(sa[0].Prod_Qty).ToString();
                        txttotalqty.Text = Convert.ToDecimal(sa[0].Toatl_Qty).ToString();
                        txtQtyAccepted.Text = Convert.ToDecimal(sa[0].Qty_Accepted).ToString();
                        txtScrapQty.Text = Convert.ToDecimal(sa[0].Scrap_Qty).ToString();
                        cmbScrapProduct.SelectedValue = sa[0].Scrap_Prod_ID;
                        txtProcessLoss.Text = Convert.ToDecimal(sa[0].Process_Loss_Per).ToString();
                        txtPowerOB.Text = Convert.ToDecimal(sa[0].Power_OB_Units).ToString();
                        txtPowerCB.Text = Convert.ToDecimal(sa[0].Power_CB_Units).ToString();
                        if (sa[0].Customer_Name != null)
                        {
                            cmbCustomer.SelectedValue = sa[0].Customer_Name;
                        }
                        if (sa[0].JobWork_Production != null)
                        {
                            chkJWProd.Checked = sa[0].JobWork_Production.Value;
                        }

                    }
                                 var ca = (from sq in db.Production_RM_Datas
                                           where sq.Company_ID == logIn.company && sq.Voucher_No == txtvchno.Text
                              select new
                              {

                                  sq.Item_ID,
                                  sq.Item_Name,
                                  sq.UOM,
                                  sq.Group_Name,
                                  sq.QtyReq,
                                  sq.QtyinStock,
                                  sq.QtyConsumed
                              });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgrmconsumed.DataSource = dt1;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //    if (result == DialogResult.Yes)
            //    {
            //        db.Sp_delete_Production(logIn.company, txtvchno.Text);
            //        MessageBox.Show("Recored Deleted Successfully");
            //        clear();
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }

        private void cmbReceipe_Leave(object sender, EventArgs e)
        {
            if (logIn.company == 16)
            {
                //var s = (from k in db.BOMs
                //         join u in db.UoM_Masters on k.RM_UOM_ID equals u.UOM_ID
                //         join c in db.Product_Groups on k.RM_Group_Name equals c.ID
                //         where k.Bom_ReceipeCode == cmbReceipe.Text && k.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && k.Company_ID == logIn.company
                //         select new
                //         {
                //             Item_ID = k.RM_Item_ID,
                //             Item_Name = k.RM_Item_Name,
                //             UOM = u.Uom_Descr,
                //             Group_Name = c.Prod_Group_Name,
                //             QtyReq = k.QtyReq                             
                //         });
                ////dgrmconsumed.DataSource = s;

                //SqlCommand cmd = (SqlCommand)db.GetCommand(s);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                //DataRow dr = dt.NewRow();
                //dgrmconsumed.DataSource = dt;
                //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                //{

                //    //Get Stock Report
                //    DateTime t = dpdate.Value;
                //    string f1 = t.ToString("dd/MMM/yyyy");

                //    var stock = (from data in db.ShowItemWiseStockReport_Production(logIn.company, Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value), t) select data).ToList();
                //    if (stock.Count > 0)
                //    {
                //        //dgProductsList.DataSource = d;
                //        dgrmconsumed.Rows[i].Cells["QtyinStock"].Value = stock[0].ClosingQty;
                //    }

                //}
            }
            else
            {
                var s = (from k in db.BOMs
                         join u in db.UoM_Masters on k.RM_UOM_ID equals u.UOM_ID
                         join c in db.Product_Groups on k.RM_Group_Name equals c.ID
                         where k.Bom_ReceipeCode == cmbReceipe.Text && k.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && k.Company_ID == logIn.company
                         select new
                         {
                             k.Bom_Item_UnitWt
                         }).ToList();
                textBox3.Text = s[0].Bom_Item_UnitWt.ToString();

            }


        }
                      

          
        

        private void txtuom_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtnoofbatchs_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQtyAccepted_Leave(object sender, EventArgs e)
        {
            try
            {

                                if (logIn.company == 16)
                {
                    decimal SUMAmount = 0;
                    if (txtnoofbatchs.Text != "" && txtnoofbatchs.Text != null)
                    {
                        decimal Consumption;
                        decimal qtyacc = 0;

                        DateTime t = dpdate.Value;
                        SqlCommand cmd = new SqlCommand("GetBomItems_Sticon", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@compname", logIn.company);
                        cmd.Parameters.AddWithValue("@ProdCode", Convert.ToInt32(cmbpname.SelectedValue));
                        cmd.Parameters.AddWithValue("@Receipe", cmbReceipe.Text);
                        cmd.Parameters.AddWithValue("@fromDate", t);
                        cmd.Parameters.AddWithValue("@BatchQty", Convert.ToDecimal(txtnoofbatchs.Text));
                        cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable Dt = new DataTable();

                        da.SelectCommand = cmd;
                        da.Fill(Dt);
                        if (Dt.Rows.Count > 0)
                        {
                            dgrmconsumed.DataSource = Dt;
                        }

                        btnSave.Enabled = true;
                        foreach (DataGridViewRow Myrow in dgrmconsumed.Rows)
                        {            //Here 2 cell is target value and 1 cell is Volume
                            if (Convert.ToDecimal(Myrow.Cells["QtyConsumed"].Value) > 0)
                            {
                                if (Convert.ToDecimal(Myrow.Cells["QtyinStock"].Value) < Convert.ToDecimal(Myrow.Cells["QtyConsumed"].Value))// Or your condition 
                                {
                                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                                    btnSave.Enabled = false;
                                }
                                else
                                {
                                    Myrow.DefaultCellStyle.BackColor = Color.White;
                                }
                            }
                        }
                    }
                        //txtQtyAccepted.Text = txtnoofbatchs.Text;

                        //decimal batchsize = Convert.ToInt32(txtnoofbatchs.Text);
                        //if (txtQtyAccepted.Text != "")
                        //{
                        //    qtyacc = Convert.ToInt32(txtQtyAccepted.Text);
                        //}
                        //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                        //{
                        //    var sa = (from a in db.BOMs where a.RM_Item_Name == dgrmconsumed.Rows[i].Cells["Item_Name"].Value && a.Bom_Item_ID == Convert.ToUInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company && a.Bom_ReceipeCode == cmbReceipe.Text select new { a.QtyReq, a.Comp_Per }).ToList();
                        //    if (sa.Count > 0)
                        //    {
                        //        var sa1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value) && a.Company_ID == logIn.company select new { a.Prod_Type_Id,a.Conv_Formula }).ToList();
                        //        decimal qty = Convert.ToDecimal(sa[0].QtyReq);
                        //        decimal ConPer = Convert.ToDecimal(sa[0].Comp_Per);
                        //        if (sa1[0].Prod_Type_Id != 1)
                        //        {
                        //            qty = qty *Convert.ToDecimal(sa1[0].Conv_Formula);
                        //            Consumption = (batchsize / Convert.ToDecimal(txtbacthsize.Text)) * qty;
                        //        }
                        //        else
                        //        {
                        //            Consumption = Math.Round((Convert.ToDecimal(txtnoofbatchs.Text) / Convert.ToDecimal(txtbacthsize.Text)) * qty, 3);

                        //            //Consumption = (batchsize * ConPer * Convert.ToDecimal(textBox3.Text)) / Convert.ToDecimal(textBox4.Text);
                        //        }

                    //    dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value = Consumption.ToString("0.000");
                    //            //Get Stock Report
                    //            decimal stockqty = Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyinStock"].Value);
                    //            if (stockqty < Consumption)
                    //            {
                    //                dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    //            }
                    //            else
                    //            {
                    //                dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                    //            }

                    //        }
                    //        SUMAmount += (dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == "" || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value);
                    //    }
                    //}
                    //txttotalqty.Text = SUMAmount.ToString(".00");
                }
                else
                {
                    DateTime t = dpdate.Value;
                    SqlCommand cmd = new SqlCommand("GetBomItems", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProdCode", Convert.ToInt32(cmbpname.SelectedValue));
                    cmd.Parameters.AddWithValue("@compname", logIn.company);
                    cmd.Parameters.AddWithValue("@Receipe", cmbReceipe.Text);
                    cmd.Parameters.AddWithValue("@fromDate", t);
                    cmd.Parameters.AddWithValue("@BatchQty", Convert.ToDecimal(txtQtyAccepted.Text));
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable Dt = new DataTable();

                    da.SelectCommand = cmd;
                    da.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        dgrmconsumed.DataSource = Dt;
                    }

                    //    var stock = (from data in db.GetBomItems(logIn.company, Convert.ToInt32(cmbpname.SelectedValue), Convert.ToInt32(cmbReceipe.Text), t, Convert.ToDecimal(txtQtyAccepted.Text))

                    //select new
                    //{
                    //    Item_ID = data.RM_Item_ID,
                    //    Item_Name = data.RM_Item_Name,
                    //    UOM = data.Uom_Descr,
                    //    Group_Name = data.Prod_Group_Name,
                    //    QtyReq = data.QtyReq,
                    //    QtyinStock = data.QtyStock,
                    //    data.QtyConsumed
                    //}
                    //).ToList();
                   
                    ////dgProductsList.DataSource = d;
                    //dgrmconsumed.DataSource = stock;
                    
                    
                    btnSave.Enabled = true;
                    foreach (DataGridViewRow Myrow in dgrmconsumed.Rows)
                    {            //Here 2 cell is target value and 1 cell is Volume
                        if (Convert.ToInt32(Myrow.Cells["QtyinStock"].Value) < Convert.ToInt32(Myrow.Cells["QtyConsumed"].Value))// Or your condition 
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.Red;
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.White;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtScrapQty_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal SUMAmount = 0;
                if (txtnoofbatchs.Text != "" && txtnoofbatchs.Text != null)
                {
                    decimal Consumption;
                    decimal qtyacc = 0;
                    decimal qtyRej = 0;
                    //txtQtyAccepted.Text = txtnoofbatchs.Text;

                    decimal batchsize = Convert.ToDecimal(txtnoofbatchs.Text);
                    if (txtScrapQty.Text != "")
                    {
                        qtyRej = Convert.ToDecimal(txtScrapQty.Text);
                        qtyacc = batchsize - qtyRej;
                        txtQtyAccepted.Text = qtyacc.ToString();
                    }
                    //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                    //{
                    //    var sa = (from a in db.BOMs where a.RM_Item_Name == dgrmconsumed.Rows[i].Cells["Item_Name"].Value && a.Bom_Item_ID == Convert.ToUInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company select new { a.QtyReq, a.Comp_Per }).ToList();
                    //    if (sa.Count > 0)
                    //    {
                    //        var sa1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgrmconsumed.Rows[i].Cells["Item_ID"].Value) && a.Company_ID == logIn.company select new { a.Prod_Type_Id }).ToList();
                    //        decimal qty = Convert.ToDecimal(sa[0].QtyReq);
                    //        decimal ConPer = Convert.ToDecimal(sa[0].Comp_Per);
                    //        if (sa1[0].Prod_Type_Id != 1)
                    //        {
                    //            Consumption = qtyacc * qty;
                    //        }
                    //        else
                    //        {
                    //            Consumption = Math.Round((Convert.ToDecimal(txtnoofbatchs.Text) / Convert.ToDecimal(txtbacthsize.Text)) * qty, 3);

                    //            // Consumption = (batchsize * ConPer * Convert.ToDecimal(textBox3.Text)) / Convert.ToDecimal(textBox4.Text);
                    //        }
                    //        dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value = Consumption;
                    //        //Get Stock Report
                    //        decimal stockqty = Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyinStock"].Value);
                    //        if (stockqty < Consumption)
                    //        {
                    //            dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    //        }
                    //        else
                    //        {
                    //            dgrmconsumed.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                    //        }
                    //    }
                    //    SUMAmount += (dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == "" || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == DBNull.Value || dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value);
                    //}
                }
                //txttotalqty.Text = SUMAmount.ToString(".00");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if ((from a in db.Production_Reports where a.Company_ID == logIn.company && a.Voucher_No == txtvchno.Text select a).Count() > 0)
                    {
                        db.Sp_delete_Production(logIn.company, txtvchno.Text,logIn.BU_ID);                        
                        MessageBox.Show("Recored Deleted Successfully");
                    }
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
                //DataTable dtexisting = new DataTable();

                if (cmbpname.Text != "" && txtQtyAccepted.Text != "")
                {
                    ItemCode = cmbpname.SelectedValue.ToString();
                    RecQty = txtQtyAccepted.Text;
                    GlobalVariables.FormName = "ProdReport";
                    
                        ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";       

                        DocNo = txtvchno.Text;


                        form.ShowDialog();
                        //dgProducts.Rows[i].Cells["ReceivedQty"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
                    }
                    else
                    {
                        MessageBox.Show("Cannot Proceed Without Item Code and Qty Accepted");
                    }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbpname_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbpname.Text!="")
                { 
                var sa = (from a in db.Bom_Data_Views where a.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company select new { a.Prod_Group_Name, a.Uom_Descr, a.Bom_Batchsize, a.Bom_Item_UnitWt, a.TotComp,a.Bom_Item_ID }).ToList();
                if (sa.Count > 0)
                {
                    txtcategory.Text = sa[0].Prod_Group_Name;
                    txtuom.Text = sa[0].Uom_Descr;
                    txtbacthsize.Text = sa[0].Bom_Batchsize.ToString();
                    textBox3.Text = sa[0].Bom_Item_UnitWt.ToString();
                    textBox4.Text = sa[0].TotComp.ToString();
                        textBox5.Text = sa[0].Bom_Item_ID.ToString();
                }
                var s = (from a in db.BOMs
                         where a.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company
                         select new { a.Bom_ReceipeCode }).Distinct().ToList();

                if (s.Count > 0)
                {
                    cmbReceipe.DataSource = s;
                    cmbReceipe.ValueMember = "Bom_ReceipeCode";
                    cmbReceipe.DisplayMember = "Bom_ReceipeCode";
                    if (cmbReceipe.Items.Count > 0)
                    {
                        cmbReceipe.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbReceipe.SelectedIndex = -1;
                    }
                }

                var d = (from a in db.Products
                         join b in db.Products on a.Prod_Scrap_Product_Id equals b.prod_ID
                         where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) && a.Company_ID == logIn.company
                         select new { b.Prod_Name, b.prod_ID }).ToList();

                if (d.Count > 0)
                {
                    cmbScrapProduct.DataSource = d;
                    cmbScrapProduct.ValueMember = "prod_ID";
                    cmbScrapProduct.DisplayMember = "Prod_Name";
                     
                        //if (cmbScrapProduct.Items.Count > 0)
                        //{
                        //    cmbScrapProduct.SelectedIndex = -1;
                        //}
                        //else
                        //{
                        //    cmbScrapProduct.SelectedIndex = -1;
                        //}
                    }

                //for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                //{
                //    var sa1 = (from a in db.StockStatements where a.Item_Name == dgrmconsumed.Rows[i].Cells["Item_Name"].Value select new { a.StockQty }).ToList();

                //    dgrmconsumed.Rows[i].Cells["QtyinStock"].Value = sa1[0].StockQty;

                //}



                //if (s.Count > 0)
                //{
                //    dgrmconsumed.DataSource = s;
               }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnViewBOM_Click(object sender, EventArgs e)
        {
            var = "0";
            iTem_Code = Convert.ToInt32(cmbpname.SelectedValue);
            RCode = cmbReceipe.Text;
            MaterialManagement.Masters.Bom frm = new MaterialManagement.Masters.Bom();
            frm.MdiParent = this.MdiParent;
            frm.Show();

        }

        private void txtScrapQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQtyAccepted_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgrmconsumed_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F3)
                {
                    ioneNet.MaterialManagement.Transactions.frmSelectRolls form = new ioneNet.MaterialManagement.Transactions.frmSelectRolls();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                    int i = dgrmconsumed.CurrentCell.RowIndex;
                    DocNo = txtvchno.Text;
                    ItemCode = dgrmconsumed.Rows[i].Cells["Item_ID"].Value.ToString();
                    //RecQty = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        //     form.ShowDialog();
                        if (ioneNet.MaterialManagement.Transactions.frmSelectRolls.TotQty > 0)
                        {
                            dgrmconsumed.Rows[i].Cells["QtyConsumed"].Value = ioneNet.MaterialManagement.Transactions.frmSelectRolls.TotQty;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    
    }
}
