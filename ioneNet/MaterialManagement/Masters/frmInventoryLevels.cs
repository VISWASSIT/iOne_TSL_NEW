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
using OpenCvSharp;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class frmInventoryLevels : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, ItemCode, RecQty, Suppname;
        public frmInventoryLevels()
        {
            InitializeComponent();
        }

        private void frmOpeningStock_Load(object sender, EventArgs e)
        {
            try
            {
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                AutoincrementId();
                //Product Groups
                var pStatus = (from m in db.ProductGroup_Lists where m.Company_ID == logIn.company select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbAccountGroup.DataSource = pStatus;
                    cmbAccountGroup.ValueMember = "ID";
                    cmbAccountGroup.DisplayMember = "Prod_Group_Name";
                }
                //Storage Locations
                var plocation = (from m in db.Storage_Locations where m.Company_ID == logIn.company select new { m.Storage_Loc_Id, m.Storage_Loc_Name }).Distinct().ToList();
                if (plocation.Count > 0)
                {
                    //cmbStorageLocation.DataSource = plocation;
                    //cmbStorageLocation.ValueMember = "Storage_Loc_Id";
                    //cmbStorageLocation.DisplayMember = "Storage_Loc_Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetAccounts_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("Get_Product_InventoryLevel", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                if (chkAllAccounts.Checked)
                {
                    cmd2.Parameters.AddWithValue("@ProdGroup", 0);
                    
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@ProdGroup", Convert.ToInt32(cmbAccountGroup.SelectedValue.ToString()));
                }
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);                
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dgvJournalVouchar.DataSource = ds2;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (cmbAccountName.Text == "")
                //{
                //    MessageBox.Show("Account Name Should not be empty");
                //    return;
                //}
                SaveNew_Sql_proc();
                //Save();
            }
            catch (Exception ex)
            {


            }
        }

        public void SaveNew_Sql_proc()
        {
            try
            {
                String myString = "";
                                  
                    SqlCommand cmd = new SqlCommand("Save_Inventory_Levels", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));

                    string Item_id = "";
                    string Min_Stock = "";
                    string Max_Stock = "";
                    string RO_Level = "";
                    string RO_Qty = "";
                    string Lead_Time = "";
                    string Storage_Location = "";                   
                    int rowcount = 0;
                    int PSno = 0;
                    decimal PrQty = 0;
                    string passnull = "0.00";
                for (int i = 0; i < dgvJournalVouchar.RowCount - 1; i++)
                {
                    if (dgvJournalVouchar.Rows[i].Cells["Storage_Location"].Value != DBNull.Value)
                    {
                        Item_id = Item_id + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value).PadRight(14);
                        if (dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value == DBNull.Value)
                        {
                            Min_Stock = Min_Stock + passnull.PadRight(14);
                        }
                        else
                        {
                            Min_Stock = Min_Stock + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value).PadRight(14);
                        }

                        if (dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value == DBNull.Value)
                        {
                            Max_Stock = Max_Stock + passnull.PadRight(14);
                        }
                        else
                        {
                            Max_Stock = Max_Stock + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value).PadRight(14);
                        }
                        if (dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value == DBNull.Value)
                        {
                            RO_Level = RO_Level + passnull.PadRight(14);
                        }
                        else
                        {
                            RO_Level = RO_Level + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value).PadRight(14);
                        }

                        if (dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value == DBNull.Value)
                        {
                            RO_Qty = RO_Qty + passnull.PadRight(14);
                        }
                        else
                        {
                            RO_Qty = RO_Qty + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value).PadRight(14);
                        }
                        if (dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value == DBNull.Value)
                        {
                            Lead_Time = Lead_Time + passnull.PadRight(14);
                        }
                        else
                        {
                            Lead_Time = Lead_Time + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value).PadRight(14);
                        }



                        Storage_Location = Storage_Location + Convert.ToString(dgvJournalVouchar.Rows[i].Cells["Storage_Location"].Value).PadRight(50);


                        rowcount += 1;
                    }
                }

                cmd.Parameters.AddWithValue("@txt_Item_id", Item_id);
                cmd.Parameters.AddWithValue("@txt_Min_Stock", Min_Stock);
                cmd.Parameters.AddWithValue("@txt_Max_Stock", Max_Stock);
                cmd.Parameters.AddWithValue("@txt_RO_Level", RO_Level);
                cmd.Parameters.AddWithValue("@txt_RO_Qty", RO_Qty);
                cmd.Parameters.AddWithValue("@txt_Lead_Time", Lead_Time);
                cmd.Parameters.AddWithValue("@txt_Storage_Location", Storage_Location);
                    
                cmd.Parameters.AddWithValue("@gridcount", rowcount);

                try
                {
                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(result))
                    {
                        MessageBox.Show("Record has been successfully saved..");

                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
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
        private void Save()
        {
            try
            {
                if ((from u in db.Product_InventoryLevels where u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    //db.sp_DeleteOpeningStocks(txtVoucherNo.Text, logIn.company,Convert.ToInt32(cmbStorageLocation.SelectedValue));
                    var p1 = db.Product_InventoryLevels.Where(w => w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();

                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                     
                        p1.Company_ID = logIn.company;
                        p1.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                        //p1.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                        //p1.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                        p1.Product_Cateogry = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                        p1.Min_Stock = (dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value);
                        p1.Max_Stock = (dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value);
                        p1.RO_Level = (dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value);
                        p1.RO_Qty = (dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value);
                        p1.Lead_Time = (dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value);
                        p1.Storage_Location = dgvJournalVouchar.Rows[i].Cells["Storage_Location"].Value.ToString(); ;
                        p1.BU_ID = logIn.BU_ID;
                        p1.Created_By = lblCreatedBy.Text;
                        p1.Modified_BY = logIn.username + "-" + DateTime.Now;                        
                        db.SubmitChanges();
                    }

                   
                    MessageBox.Show("Record Updated Successfully");
                    clear();
                    ////}
                    ////else
                    ////{
                    ////    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////    clear();
                    ////}


                }
                else
                {
                    try
                    {

                        //if (AppCode.GlobalAccess.Add == "Yes")
                        //{
                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {

                            Product_InventoryLevel p = new Product_InventoryLevel();
                            p.Company_ID = logIn.company;
                            p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                            //p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                            //p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                            p.Product_Cateogry = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                            p.Min_Stock = (dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Min_Qty"].Value);
                            p.Max_Stock = (dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Max_Qty"].Value);
                            p.RO_Level = (dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Re_Ord_Level"].Value);
                            p.RO_Qty = (dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Re_Order_Qty"].Value);
                            p.Lead_Time = (dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Lead_Time"].Value);
                            p.Storage_Location = dgvJournalVouchar.Rows[i].Cells["Storage_Location"].Value.ToString();
                            p.BU_ID = logIn.BU_ID;
                            p.Created_By = lblCreatedBy.Text;
                            p.Modified_BY = logIn.username + "-" + DateTime.Now;

                            db.Product_InventoryLevels.InsertOnSubmit(p);

                       
                        }
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        clear();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    clear();
                        //}
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
            catch (Exception ex)
            {
            }

        }
        public void clear()
        {
            try
            {

                foreach (Control x in this.Controls)
                {

                    foreach (Control d in groupBox3.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is CheckBox)
                            (d as CheckBox).Checked = false;
                    }
                }
                if (dgvJournalVouchar.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                        dgvJournalVouchar.Rows.RemoveAt(i);
                        i--;
                        while (dgvJournalVouchar.Rows.Count == 0)
                            continue;
                    }
                }
                //cmbAccountName.Text = "";
                //txtCreditAmtTotal.Text = txtDebitAmtTotal.Text = "";
                AutoincrementId();
                // txtTotalAmt.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
        public void AutoincrementId()
        {
            try
            {
                //var auto = db.Sp_autoincrement_Prod_OpeningStock(logIn.company);
                //txtVoucherNo.Text = auto.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void dgvJournalVouchar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow dgProductData.Rows[i] = dgProductData.Rows[dgProductData.CurrentRow.Index];
                if (dgvJournalVouchar.Rows.Count > 1)
                {
                    if (dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index].Cells[dgvJournalVouchar.CurrentCell.ColumnIndex].Value == "Remove")
                    {
                        if (dgvJournalVouchar.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                            {
                                if (oneCell.Selected)
                                    dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //  bindCashAct();
                MaterialManagement.Masters.FG_OBStockVouchers obj = new MaterialManagement.Masters.FG_OBStockVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    //txtVoucherNo.Text = MaterialManagement.Masters.OBStockVouchers.voucherNo;

                    //if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    //{
                        var dm1 = (from s in db.Product_OpeningStocks
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Company_ID == logIn.company 
                                   select new
                                   {
                                       s.Prod_ID,
                                       s.Prod_Name,
                                       Uom_Descr=s.UOM,
                                       Prod_Unit_Wt=s.Unit_wt,
                                       s.Prod_Grade,
                                       s.Prod_length,
                                       s.OB_Stock_qty,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value
                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;


                    }

                    //var f = (from s in db.Account_Opening_Balances where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company select s).FirstOrDefault();
                    //if (f != null)
                    //{
                    //    AsAtdate.Text = f.OB_date.ToString();
                    //    //cmbAccountName.Text = f.Account;
                    //    txtRemarks.Text = f.Remarks;
                    //    //ObDate.Text = f.OBDate.ToString();
                    //    //txtCreditAmtTotal.Text = f.TotalCreditAmt.ToString();
                    //    //txtDebitAmtTotal.Text = f.TotalDebitAmt.ToString();

                    //}
                    //double Qty = 0, Amout = 0;

                    //for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
                    //{

                    //    if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                    //    {

                    //        Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                    //        Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                    //    }

                    //}
                    //txtDebitAmtTotal.Text = Qty.ToString(".00");
                    //txtCreditAmtTotal.Text = Amout.ToString(".00");

                //}

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                int i = dgvJournalVouchar.CurrentCell.RowIndex;
                SONo = "OB";
                ItemCode = dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString();
                //RecQty = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                form.ShowDialog();
                dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
            }
        }
    }
}
