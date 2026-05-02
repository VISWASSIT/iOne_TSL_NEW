using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GatePasReturnable : Form
    {


        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        System.Data.Common.DbTransaction transaction;
        public static int global = 0;

        public GatePasReturnable()
        {
            InitializeComponent();
        }


    

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region METHOS
        public void save()
        {
            try
            {
                //if ((from u in db.GatePass_Returnables where u.GateVch_No == txtVchNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                if (txtVchNo.Text != "")
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{

                    db.sp_Select_Delete_Gatepass_Returnable(logIn.company, txtVchNo.Text,logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                }

                GatePass_Master S = new GatePass_Master();
                {
                    S.GateVch_No = txtVchNo.Text;
                    S.GP_Date = dpgatepassdate.Value;
                    S.SendingFor = (cmbSendingfor.Text == "") ? "" : (cmbSendingfor.Text);
                    S.PartyName = Convert.ToInt32(cmbPartyName.SelectedValue.ToString());
                    S.Address = (txtGSTNo.Text == "") ? "" : (txtGSTNo.Text);
                    S.City = (txtCity.Text == "") ? "" : (txtCity.Text);
                    S.Wo_Ref = (cmbWONo.Text == "") ? "" : (cmbWONo.Text);
                    S.SentThrough = (txtSentThrough.Text == "") ? "" : (txtSentThrough.Text);
                    S.isDeleted = false;
                    S.VehicleNo = (txtVehicleNo.Text == "") ? "" : (txtVehicleNo.Text);
                    S.Any_SplInstructions = (txtAnySplinstructions.Text == "") ? "" : (txtAnySplinstructions.Text);
                    S.Material_tobeReturnedon = dpmaterialreturned.Value;
                    S.WayBillNo = txtLrno.Text;
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S.Non_Returnable = checkBox1.Checked;
                    S.Stock_Item = checkBox2.Checked;
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;                    
                    S.Created_By = logIn.username + "-" + DateTime.Now;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;                    
                    db.GatePass_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }



                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                GatePass_Child GP = new GatePass_Child();
                var d1 = (from a in db.GatePass_Masters where a.GateVch_No == txtVchNo.Text && a.isDeleted ==false && a.Non_Returnable == checkBox1.Checked && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                GP.GP_Master_ID = d1[0].Id;
                GP.GateVch_No = txtVchNo.Text;               
                GP.Prod_Code = (dgProducts.Rows[i].Cells["Prod_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Prod_Code"].Value);
                GP.Int_Prod_Code = (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value == null) ? "" : dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                GP.Product_Description = (dgProducts.Rows[i].Cells["Product_Name"].Value == null) ? "" : dgProducts.Rows[i].Cells["Product_Name"].Value.ToString();
                GP.UOM = (dgProducts.Rows[i].Cells["unit_purchase"].Value == null) ? "" : dgProducts.Rows[i].Cells["unit_purchase"].Value.ToString();
                GP.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value.ToString());
                GP.Price = (dgProducts.Rows[i].Cells["Price"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Price"].Value.ToString());
                GP.Category = (dgProducts.Rows[i].Cells["Category"].Value == null) ? "" : dgProducts.Rows[i].Cells["Category"].Value.ToString();
                GP.C_Stock = (dgProducts.Rows[i].Cells["CurrentStock"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CurrentStock"].Value.ToString());
                GP.Estimated_Value = (dgProducts.Rows[i].Cells["Estimated_Value"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Estimated_Value"].Value.ToString());
                GP.Remarks = (dgProducts.Rows[i].Cells["NatureofWorktobeCarriedOut"].Value == null) ? "" : dgProducts.Rows[i].Cells["NatureofWorktobeCarriedOut"].Value.ToString();
                GP.S_No = Convert.ToInt32(dgProducts.Rows[i].Cells["S_No"].Value);
                GP.Company_ID = logIn.company;
               
                db.GatePass_Childs.InsertOnSubmit(GP);                    
                db.SubmitChanges();
                }
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtVchNo.Text);
                clear();
            
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                foreach (Control x in this.Controls)
                {
                    foreach (Control d in tableLayoutPanel1.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;


                    }
                    foreach (Control d in tableLayoutPanel5.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                    }
                }
                if (dgProducts.Rows.Count > 0)
                {
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        dgProducts.Rows.RemoveAt(i);
                        i--;
                        while (dgProducts.Rows.Count == 0)
                            continue;
                    }
                }


                // BlindComboOngrid();
                bindbuyer();
                Bin();
                //AutoincrementId();
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While  clear Getting Error ", "BusinessLost", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Bind Bin
        public void Bin()
        {
            //var sa = (from k in db.BIN_Masters select new { k.BINName }).ToList();
            //if(sa.Count>0)
            //{
            //    cmbBin.DataSource = sa;
            //    cmbBin.DisplayMember = "BINName";
            //    cmbBin.ValueMember = "BINName";
            //    if (cmbBin.Items.Count>0)
            //    {
            //        cmbBin.SelectedIndex = -1;
            //    }
            //}
        }
        // // method to bind the suppliers
        public void bindbuyer()
        {
            try
            {
                //var Buyerblind = (from m in db.AccountMasters where m.SupAccount == "true" select m.AccName).Distinct().ToList();
                //if (Buyerblind.Count > 0)
                //{
                //    cmbPartyName.DataSource = Buyerblind;
                //}
                //if (cmbPartyName.Items.Count > 0)
                //{
                //    cmbPartyName.SelectedIndex = -1;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AutoincrementId()
        {
            try
            {
                if(checkBox1.Checked)
                {
                    var auto = db.Sp_autoincrement_GatePass_Non_Returnable(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtVchNo.Text = auto.FirstOrDefault().So_no;
                }
                else
                {
                    var auto = db.Sp_autoincrement_GatePass_Returnable(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtVchNo.Text = auto.FirstOrDefault().So_no;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //public void BlindComboOngrid()
        //{
        //    //blind combobox on grid Salable_Item
        //    var ds = (from c in db.ProdMasters where c.Comp_Name == AppCode.GlobalAccess.companyName select c.Product_Name).ToList();
        //    DataGridViewComboBoxColumn combo = (DataGridViewComboBoxColumn)dgvGatePass.Columns["Prod_Name"];

        //    if (ds.Count > 0)
        //    {
        //        combo.DataSource = ds;

        //    }
        //}
        public void delete()
        {
            try
            {
                //if ((from u in db.GatePass_Returnables where u.GateVch_No == txtVchNo.Text && u.Company == AppCode.GlobalAccess.companyName select u).Count() > 0)
                //{
                //    if (AppCode.GlobalAccess.Edit == "Yes")
                //    {
                //        DialogResult result = MessageBox.Show("Are You Sure Want to Delete this Record?", "Gate Pass", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                //        if (result == DialogResult.OK)
                //        {
                //            db.sp_Select_Delete_Gatepass_Returnable(AppCode.GlobalAccess.companyName, txtVchNo.Text);
                //            // transaction.Commit();
                //            MessageBox.Show("Record Deleted Successfully ", "Gate Pass", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            clear();
                //        }
                //        else
                //        {
                //            return;
                //            Cursor.Current = Cursors.Default;
                //            MessageBox.Show("Record Not Deleted While Getting Error", "Gate Pass", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        }

                //    }
                //    else
                //    {
                //        Cursor.Current = Cursors.Default;
                //        MessageBox.Show("You dont have privileges to Record this Record", "Material Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
                //else
                //{
                //    Cursor.Current = Cursors.Default;
                //    MessageBox.Show("This Record Not Exising ,Please Select The Existing  Record", "Gate Pass", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}


            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                //transaction.Rollback();
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


        #endregion

        private void GatePasReturnable_Load(object sender, EventArgs e)
        {
            try
            {
                dpgatepassdate.MinDate = logIn.fy_Start_Date;
                dpgatepassdate.MaxDate = logIn.fy_End_Date;
                // pictureBox1.Image = AppCode.GlobalAccess.comylogo;
                bindCustomer();
                if(logIn.company == 20)
                {
                    dgProducts.Columns[9].Visible = true;
                }
                else
                {
                    dgProducts.Columns[9].Visible = false;
                }

                if (GatePassList.var == "0")
                {
                    if (GatePassList.editMode == true)
                    {
                        bindedit();
                    }
                }
                else
                if (GatePassList.var == "1")
                {
                    if (GatePassList.editMode == true)
                    {
                        bindedit();
                        btnSave.Enabled = false;
                        btnClear.Enabled = false;
                    }
                }
                else
                {
                    //AutoincrementId();
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
                //if (txtVchNo.Text == "")
                //{
                //    MessageBox.Show("Gate Pass Number Cannot be Blank");
                //    txtVchNo.Focus();
                //    return;
                //}              
                
                if (cmbPartyName.Text == "")
                {
                    MessageBox.Show("Please Select Party Name");
                    cmbPartyName.Focus();
                    return;
                }
                else if (cmbSendingfor.Text == "")
                {
                    MessageBox.Show("Please Select Sending For From The List");
                    cmbSendingfor.Focus();
                    return;
                }
                else if (cmbStatus.Text == "")
                {
                    MessageBox.Show("Status Cannot Be Blank");
                    cmbStatus.Focus();
                    return;
                }
                save();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                delete();
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
              
                txtVchNo.Text = GatePassList.SO_No;
                var d = (from po in db.GatePass_Masters                         
                            where po.GateVch_No == GatePassList.SO_No && po.Company_ID == logIn.company && po.BU_ID ==logIn.BU_ID
                            select po).ToList();
                dpgatepassdate.Text = d[0].GP_Date.ToString();
                cmbSendingfor.Text = d[0].SendingFor;               
                cmbPartyName.SelectedValue = d[0].PartyName;
                //cmbPartyName.SelectedValue = d[0].PartyName;
                txtGSTNo.Text = d[0].Address.ToString();
                txtCity.Text = d[0].City;              
                txtLrno.Text = d[0].WayBillNo;
                cmbWONo.Text = d[0].Wo_Ref;
                txtSentThrough.Text = d[0].SentThrough;                
                txtVehicleNo.Text = d[0].VehicleNo;
                dpmaterialreturned.Text = d[0].Material_tobeReturnedon.ToString();
                txtAnySplinstructions.Text = d[0].Any_SplInstructions;        
                if (d[0].Non_Returnable != null)
                {
                    checkBox1.Checked = d[0].Non_Returnable.Value;
                }
                if (d[0].Stock_Item != null)
                {
                    checkBox2.Checked = d[0].Stock_Item.Value;
                }
                //MessageBox.Show(cmbPartyName.Text);

                //MessageBox.Show(cmbPartyName.SelectedValue.ToString());
                
                var p = (from po in db.GatePass_Childs                     
                         join g1 in db.GatePass_Masters on po.GP_Master_ID equals g1.Id      
                            where po.GP_Master_ID == GatePassList.gp_id && po.Company_ID == logIn.company && g1.BU_ID == logIn.BU_ID
                            select new
                            {
                                //po.Product_code,
                                po.S_No,
                                po.Prod_Code,
                                Int_Prod_Code = po.Int_Prod_Code,
                                Product_Name = po.Product_Description,
                                po.Category,
                                unit_purchase =po.UOM,
                                CurrentStock = po.C_Stock,   
                                po.Qty,
                                po.Price,
                                Estimated_Value = po.Estimated_Value,
                                NatureofWorktobeCarriedOut =po.Remarks,
                            });
                SqlCommand cmd = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dgProducts.DataSource = dt;
                int j = 0;
                for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                {
                    j = j + 1;
                    dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                }
               // MessageBox.Show(cmbPartyName.Text);
                //for (int i = 0; i < dgvRequistForQuot.Rows.Count; i++)
                //{
                //    dgvRequistForQuot.Rows[i].Cells[0].Value = i + 1;
                //}


            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnclose_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
              
            }
        }

        // // product name leave event to bind the address and city fields
        private void cmbPartyName_Leave(object sender, EventArgs e)
        {
            try
            {
                var ds = (from c in db.Supplier_informations
                          where c.Supplier_Name==cmbPartyName.Text 
                          select new
                          {
                             c.Address_1,
                             c.Address_2,
                             c.State,
                             c.Pincode,
                             c.City,
                             c.GSTIN_NO
                          }).ToList();
                if(ds.Count>0)
                {
                    txtGSTNo.Text = ds[0].GSTIN_NO;
                    txtCity.Text = ds[0].City;
                }
                else
                {
                    MessageBox.Show("Invalid Party Name Selected");
                    cmbPartyName.Focus();
                    return;
                }

                //Get PO Data
                var Buyerblind = (from m in db.SP_GetOrders_Sel(logIn.company,cmbPartyName.Text, logIn.BU_ID, "")
                                  select new { m.PO_No }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbWONo.DataSource = Buyerblind;
                    cmbWONo.ValueMember = "PO_No";
                    cmbWONo.DisplayMember = "PO_No";
                }
                cmbWONo.SelectedIndex = -1;
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

         // // bindmethod to bind the products in the grid field
        public void bindCustomer()
        {

            try
            {
                //sUPPLIER
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbPartyName.DataSource = Buyerblind;
                    cmbPartyName.ValueMember = "ID";
                    cmbPartyName.DisplayMember = "Supplier_Name";

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                cmbPartyName.SelectedIndex = -1;

              
                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // // cell end edit event to bind the uom and category details while selecting the product name
        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if (columnName == "Product_Name" && R1.Cells["Product_Name"].Value != null)
                {

                    //var getProductName = (from s in db.Products
                    //                      join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                    //                      join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                    //                      where s.Prod_Name == R1.Cells["Product_Name"].Value.ToString() && s.Company_ID == logIn.company
                    //                      select new { s.prod_ID, s.Prod_Code, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code }).FirstOrDefault();

                    var getProductName = (from s in db.Get_ProductsList(logIn.company,1, R1.Cells["Product_Name"].Value.ToString())                                         
                                          select s).FirstOrDefault();

                    if (getProductName == null)
                    {
                        if (checkBox2.Checked)
                        {
                            //R1.Cells["unit_purchase"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Prod_Code"].Value ="0";
                            R1.Cells["Int_Prod_Code"].Value = "NA";
                            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                            {
                                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Product Name");
                            return;
                        }
                    }
                    else
                    {  
                        R1.Cells["Category"].Value = getProductName.Prod_Group_Name.ToString();
                        R1.Cells["unit_purchase"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Prod_Code"].Value = getProductName.prod_ID.ToString();
                        R1.Cells["Int_Prod_Code"].Value = getProductName.Prod_Code.ToString();
                        if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                        {
                            R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                        }
                        //var s = (from data in db.sp_StockEnquiryReport_1(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dpgatepassdate.Value), R1.Cells["Product_Name"].Value.ToString(), cmbBin.Text.Trim(), R1.Cells["Product_Category"].Value.ToString()) select data.ClosingQty).FirstOrDefault();

                        //decimal de = s;

                        //R1.Cells["CurrentStock"].Value = s;

                    }
                    DateTime t = dpgatepassdate.Value;
                    string f1 = t.ToString("dd/MMM/yyyy");
                    if (R1.Cells["Int_Prod_Code"].Value != "NA")
                    {
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Prod_Code"].Value), t, logIn.BU_ID) select data).ToList();
                        //var stock = (from data in db.ShowStockLedger_New(logIn.company, Convert.ToInt32(R1.Cells["Prod_Code"].Value), t, t, logIn.BU_ID) select data).ToList();

                        if (stock.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            R1.Cells["CurrentStock"].Value = stock[0].ClosingQty;
                            R1.Cells["Price"].Value = stock[0].CBPrice;
                        }
                        else
                        {
                            R1.Cells["CurrentStock"].Value = "0";
                            R1.Cells["Price"].Value = "0";
                        }
                    }
                    else
                    {
                        R1.Cells["CurrentStock"].Value = "0";
                        R1.Cells["Price"].Value = "0";
                    }
                }
                if (columnName == "Qty" || columnName == "Price")
                {
                    if (R1.Cells["Qty"].Value != null && R1.Cells["Qty"].Value != DBNull.Value && R1.Cells["CurrentStock"].Value != null && R1.Cells["CurrentStock"].Value != DBNull.Value)

                    {
                        if (checkBox2.Checked == false)
                        {
                            if (Convert.ToDouble(R1.Cells["CurrentStock"].Value) < Convert.ToDouble(R1.Cells["Qty"].Value))
                            {
                                MessageBox.Show("Qty  Should not be Greater than Current Stock..");
                                R1.Cells["Qty"].Value = 0;
                                return;
                            }
                            else
                            {
                                decimal q = Convert.ToDecimal(R1.Cells["Qty"].Value);
                                decimal p = Convert.ToDecimal(R1.Cells["Price"].Value);
                                if (R1.Cells["Estimated_Value"].Value == null)
                                {
                                    R1.Cells["Estimated_Value"].Value = p;
                                }

                            }
                        }
                        else
                        {
                            decimal q = Convert.ToDecimal(R1.Cells["Qty"].Value);
                            decimal p = Convert.ToDecimal(R1.Cells["Price"].Value);
                            if (R1.Cells["Estimated_Value"].Value == null)
                            {
                                R1.Cells["Estimated_Value"].Value = p;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow R1 = dgProductData.Rows[dgProductData.CurrentRow.Index];

                if (dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex].Value == "Remove")
                {
                    if (dgProducts.Rows.Count > 0)
                    {
                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnApplicableDiscount_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "JobWorkChallan.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);              

                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new ioneNet.MaterialManagement.Transactions.GatePassReturnable();
                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }

                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int i = 0; i < crTables.Count; i++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[i].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[i].ApplyLogOnInfo(crTableLogOnInfo);

                }

                rep.SetParameterValue("Company", logIn.company);
                rep.SetParameterValue("DCNO", txtVchNo.Text);

                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                Process.Start(path);


                //CrstalReportViewer1 viewer = new CrstalReportViewer1();
                //viewer.crystalReportViewer1.ReportSource = rep;
                //viewer.ShowDialog();
                //viewer.Dispose();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "UOM")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;

                if (columnName == "UOM")
                {
                    var Prodname = (from d in db.UoM_Masters select new { d.Uom_Descr }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Uom_Descr");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Uom_Descr);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Get_ProductsList (logIn.company,0,"") select new { d.Prod_Name }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Prod_Name");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.Prod_Name);
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
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtLrno_TextChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "GatePass";
                    form.ShowDialog();

                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();
                        dtexisting.Columns.Add("S_No", typeof(string));
                        dtexisting.Columns.Add("Prod_Code", typeof(string));
                        dtexisting.Columns.Add("Int_Prod_Code", typeof(string));
                        dtexisting.Columns.Add("Product_Name", typeof(string));
                        dtexisting.Columns.Add("Category", typeof(string));
                        dtexisting.Columns.Add("unit_purchase", typeof(string));
                        dtexisting.Columns.Add("CurrentStock", typeof(decimal));
                        dtexisting.Columns.Add("Qty", typeof(string));
                        dtexisting.Columns.Add("Price", typeof(string));
                        dtexisting.Columns.Add("Estimated_Value", typeof(string));
                        dtexisting.Columns.Add("NatureofWorktobeCarriedOut", typeof(string));

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                            dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                            dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                            dr["Product_Name"] = dgProducts.Rows[i].Cells["Product_Name"].Value.ToString();
                            dr["Category"] = dgProducts.Rows[i].Cells["Category"].Value.ToString();
                            dr["unit_purchase"] = dgProducts.Rows[i].Cells["unit_purchase"].Value.ToString();
                            dr["CurrentStock"] = dgProducts.Rows[i].Cells["CurrentStock"].Value.ToString();
                            dr["Qty"] = (dgProducts.Rows[i].Cells["Qty"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Qty"].Value);
                            dr["Price"] = (dgProducts.Rows[i].Cells["Price"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Price"].Value);
                            dr["Estimated_Value"] = (dgProducts.Rows[i].Cells["Estimated_Value"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Estimated_Value"].Value);
                            dr["NatureofWorktobeCarriedOut"] = (dgProducts.Rows[i].Cells["NatureofWorktobeCarriedOut"].Value == null) ? "" : (dgProducts.Rows[i].Cells["NatureofWorktobeCarriedOut"].Value);
                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }


                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("S_No", typeof(string));
                        dt.Columns.Add("Prod_Code", typeof(string));
                        dt.Columns.Add("Int_Prod_Code", typeof(string));
                        dt.Columns.Add("Product_Name", typeof(string));
                        dt.Columns.Add("Category", typeof(string));
                        dt.Columns.Add("unit_purchase", typeof(string));
                        dt.Columns.Add("CurrentStock", typeof(decimal));
                        dt.Columns.Add("Qty", typeof(decimal));
                        dt.Columns.Add("Price", typeof(decimal));
                        dt.Columns.Add("Estimated_Value", typeof(decimal));
                        dt.Columns.Add("NatureofWorktobeCarriedOut", typeof(string));

                        //dt.Rows.Add();
                        int j = dgProducts.Rows.Count - 1;
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               join pg in db.Product_Groups on obj.Prod_Group_Id equals pg.ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {
                                                   Prod_Code = obj.prod_ID,
                                                   Int_Prod_Code = obj.Prod_Code,
                                                   Product_Name = obj.Prod_Name,
                                                   Category = pg.Prod_Group_Name,
                                                   unit_purchase = uom.Uom_Descr,
                                                   CurrentStock = 0,
                                                   Qty = 0,
                                                   Price = 0,
                                                   Estimated_Value = 0,
                                                   NatureofWorktobeCarriedOut = ""
                                               }).ToList();
                            j = j + 1;
                            dt.Rows.Add(j, getproducts[0].Prod_Code, getproducts[0].Int_Prod_Code, getproducts[0].Product_Name, getproducts[0].Category, getproducts[0].unit_purchase, getproducts[0].CurrentStock, getproducts[0].Qty, getproducts[0].Price, getproducts[0].Estimated_Value, getproducts[0].NatureofWorktobeCarriedOut);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;

                        int l = 0;
                        for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                        {
                            if (dgProducts.Rows[m].Cells["Product_Name"].Value != "")
                            {
                                l = l + 1;
                                dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                            }
                        }
                    }

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DateTime t = dpgatepassdate.Value;
                        string dt1 = t.ToString("yyyy/MM/dd");
                        string prodcode = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(prodcode), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        dgProducts.Rows[i].Cells["CurrentStock"].Value = "0";
                        if (stock.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            dgProducts.Rows[i].Cells["CurrentStock"].Value = stock[0].ClosingQty;
                            dgProducts.Rows[i].Cells["Price"].Value = stock[0].CBPrice;

                        }
                    }

                    
                }
                if (e.KeyCode == Keys.F6) //Remove Rows
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                            int l = 0;
                            for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                            {
                                if (dgProducts.Rows[m].Cells["Product_Name"].Value != "")
                                {
                                    l = l + 1;
                                    dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                                }
                            }
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPartyName_Leave_1(object sender, EventArgs e)
        {
            //Get Gate Pass Data
            var Buyerblind = (from m in db.Purchase_Order_Masters where m.Company_ID == logIn.company && m.BU_ID == logIn.BU_ID && m.SupplierName == Convert.ToInt32(cmbPartyName.SelectedValue) select new { m.PO_NO }).Distinct().ToList();
            if (Buyerblind.Count > 0)
            {
                cmbWONo.DataSource = Buyerblind;
                cmbWONo.ValueMember = "PO_NO";
                cmbWONo.DisplayMember = "PO_NO";
            }
            cmbWONo.SelectedIndex = -1;
        }

        private void cmbPartyName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
        }
    }
}
