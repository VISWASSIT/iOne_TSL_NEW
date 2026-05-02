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

namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_RM_Reservation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmForge_RM_Reservation()
        {
            InitializeComponent();
        }

        private void frmForge_RM_Reservation_Load(object sender, EventArgs e)
        {
            txtSONo.Text = frmforge_Production_Scheduler.SO_No;
            txtSOID.Text = frmforge_Production_Scheduler.SO_ID;
            txt_fg_Item_Code.Text = frmforge_Production_Scheduler.SO_Item_Code;
            txtSO_ItemNo.Text = frmforge_Production_Scheduler.SO_Item_No;
            txt_fg_Item_Name.Text = frmforge_Production_Scheduler.SO_Item_Name;
            txtOrdQty.Text = frmforge_Production_Scheduler.SO_Qty;

            // dtDelDate.Text = frmforge_Production_Scheduler.del_date.ToString();
            var Prodname = (from m in db.So_RM_Price_Datas
                            where m.company_id == logIn.company && m.id == Convert.ToInt32(txtSOID.Text)
                            select new { m.RM_Basic_Price }).FirstOrDefault();
            if (Prodname != null)
            {
               txtRMPrice.Text = Prodname.RM_Basic_Price.ToString();
            }

        }

        private void dgRM_Items_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
            string columnName = dgRM_Items.Columns[columnIndex].HeaderText;
            TextBox tb3 = e.Control as TextBox;
            tb3.AutoCompleteMode = AutoCompleteMode.None;
            tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
            tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
            addItems(DataColl);
            tb3.AutoCompleteCustomSource = DataColl;
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];

                int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
                string columnName = dgRM_Items.Columns[columnIndex].HeaderText;

                if (columnName == "Heat Code")
                {
                   
                }
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Forging_FinishedGoods_RMs
                                        join f in db.Forging_Finished_Goods on d.FG_Item_ID equals f.prod_ID
                                        join p in db.Products on d.RM_Prod_ID equals p.prod_ID
                                        where f.Prod_Customer_Code == txt_fg_Item_Code.Text
                                        select new { p.Prod_Name }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Raw_Material");
                        foreach (var item in Prodname)
                        {

                            string MP = item.Prod_Name;
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                dt.Rows.Add(m);
                            }
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgRM_Items_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
                int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
                string columnName = dgRM_Items.Columns[columnIndex].Name;


                if (columnName == "Item_Code" && R1.Cells["Item_Code"].Value != null)
                {

                    var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_code"].Value.ToString())
                                          select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        if (getProductName.Uom_Descr != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        }
                        R1.Cells["Item_ID"].Value = getProductName.prod_ID.ToString();
                        //Get Input Wt
                        var fgWt = (from s in db.Forging_Finished_Goods
                                    where s.Prod_Customer_Code == txt_fg_Item_Code.Text
                                    select new { s.Input_Weight }).FirstOrDefault();

                        if (fgWt != null)
                        {
                            if (txtOrdQty.Text != "")
                            {
                                decimal jQty = Convert.ToDecimal(txtOrdQty.Text);

                                decimal InputWt = Convert.ToDecimal(fgWt.Input_Weight.ToString());
                                R1.Cells["Qty_Req"].Value = jQty * InputWt;
                            }
                            else
                            {
                                MessageBox.Show("Enter Job Card Qty");
                                txtOrdQty.Focus();
                            }

                        }

                        var stock = (from data in db.Forge_StockReport_JobCard(logIn.company, Convert.ToInt32(R1.Cells["Item_ID"].Value)) select data).ToList();

                        if (stock.Count > 0)
                        {
                            dataGridView1.DataSource = stock;
                            groupBox1.Visible = true;
                            //R1.Cells["Remarks"].Value = getProductName.Prod_Customer_Code.ToString();
                            //taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                        }
                    }

                    else
                    {
                        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    }


                }
                if (columnName == "Heat_Code")
                {

                    //Get Stock Report
                    //DateTime t = dtJDate.Value;
                    //string f1 = t.ToString("dd/MMM/yyyy");
                    ////DataGridViewRow R2 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
                    //if (R1.Cells["Heat_Code"].Value.ToString() != "")
                    //{
                    //    string heatno = Mid(R1.Cells["Heat_Code"].Value.ToString(), 1, 4);
                    //    //var stock = (from data in db.Forge_StockReport_JobCard(logIn.company, Convert.ToInt32(R1.Cells["Item_ID"].Value), R1.Cells["Heat_Code"].Value.ToString(), t) select data).ToList();

                    //    //if (stock.Count > 0)
                    //    //{
                    //    //    //dgProductsList.DataSource = d;
                    //    //    R1.Cells["Qty_Stock"].Value = stock[0].StkQty;

                    //    //    //                        dgRM_Items.Rows[i].Cells["QtyinStock"].Value = stock[0].ClosingQty;
                    //    //}

                    //}
                }

                if (columnName == "Qty_Allocated")
                {
                    if (Convert.ToDecimal(R1.Cells["Qty_Allocated"].Value) > 0)
                    {
                        //Get Stock Report
                        decimal stkqty = Convert.ToDecimal(R1.Cells["Qty_Stock"].Value);
                        decimal AltQty = Convert.ToDecimal(R1.Cells["Qty_Allocated"].Value);
                        decimal ReqQty = Convert.ToDecimal(R1.Cells["Qty_Req"].Value);
                        if (AltQty > ReqQty)
                        {
                            MessageBox.Show("Alloted Qty Cannot Be More than Required Qty");
                            R1.Cells["Qty_Allocated"].Value = "";
                        }
                        else
                        {
                            if (AltQty > stkqty)
                            {
                                //dgProductsList.DataSource = d;
                                MessageBox.Show("Alloted Qty Cannot Be More than Stock Qty");
                                R1.Cells["Qty_Allocated"].Value = "";
                            }
                        }
                    }
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
            int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
            string columnName = dgRM_Items.Columns[columnIndex].Name;
            R1.Cells["Heat_Code"].Value = dataGridView1.Rows[e.RowIndex].Cells["Heat_No"].Value.ToString();
            R1.Cells["RM_Basic_Price"].Value = dataGridView1.Rows[e.RowIndex].Cells["Basic_Price"].Value.ToString();
            R1.Cells["Qty_Stock"].Value = dataGridView1.Rows[e.RowIndex].Cells["StkQty"].Value.ToString();

            decimal QtyReq = Convert.ToDecimal(R1.Cells["Qty_Req"].Value);
            decimal QtyStock = Convert.ToDecimal(R1.Cells["Qty_Stock"].Value);
            if (QtyReq >= QtyStock)
            {
                MessageBox.Show("Required Qty Not Available In The Stock");
                R1.Cells["Qty_Allocated"].Value = "0";
            }
            else if (QtyStock > QtyReq)
            {
                R1.Cells["Qty_Allocated"].Value = QtyReq;
            }
            groupBox1.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgRM_Items.RowCount - 1; i++)
            {
                Forging_JobCardRM SC = new Forging_JobCardRM();
                //var d1 = (from a in db.Forging_JobCards where a.Job_CardNo == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                //SC.JobCard_ID = d1[0].id;
                SC.SO_Master_ID = Convert.ToInt32(txtSOID.Text); ;
                SC.SO_Item_No = Convert.ToInt32(txtSO_ItemNo.Text); ;
                SC.RM_Code = Convert.ToInt32(dgRM_Items.Rows[i].Cells["Item_ID"].Value);
                SC.RM_Name = (dgRM_Items.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgRM_Items.Rows[i].Cells["Item_Code"].Value).ToString();
                SC.RM_Spec = (dgRM_Items.Rows[i].Cells["Item_Spec"].Value == null) ? "" : (dgRM_Items.Rows[i].Cells["Item_Spec"].Value).ToString();
                SC.RM_Uom = (dgRM_Items.Rows[i].Cells["UOM"].Value == null) ? "" : dgRM_Items.Rows[i].Cells["UOM"].Value.ToString();
                SC.Heat_Code = (dgRM_Items.Rows[i].Cells["Heat_Code"].Value == null) ? "" : dgRM_Items.Rows[i].Cells["Heat_Code"].Value.ToString();
                //SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                SC.Qty_Req_MT = (dgRM_Items.Rows[i].Cells["Qty_Req"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Req"].Value);

                SC.Qty_Stock = (dgRM_Items.Rows[i].Cells["Qty_Stock"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Stock"].Value);
                SC.Qty_Alloted = (dgRM_Items.Rows[i].Cells["Qty_Allocated"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Allocated"].Value);
                SC.RM_Price = (dgRM_Items.Rows[i].Cells["RM_Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["RM_Basic_Price"].Value);
                db.Forging_JobCardRMs.InsertOnSubmit(SC);
            }
            db.SubmitChanges();
            MessageBox.Show("Material Reservation Updated Successfully");
            this.Close();
        }
    }
}
