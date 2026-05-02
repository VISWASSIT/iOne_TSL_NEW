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
using Ione_DAL;
using OpenCvSharp;
using Syncfusion.Windows.Forms.Chart.SvgBase;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmProductionRequirement : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string FPress, MGroup;
        public frmProductionRequirement()
        {
            InitializeComponent();
        }

        private void frmForge_ProductionPlanning_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Conversion_Party" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbConvPartyName.DataSource = pStatus;
                cmbConvPartyName.ValueMember = "ID";
                cmbConvPartyName.DisplayMember = "Descr";
                cmbConvPartyName.SelectedIndex = -1;
            }
            //bindDroupDown_Lookup();
            AutoincrementId();

            //if (frmForge_Planning_List.editMode == true)
            //{
            //    bindedit();
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var d = (from data in db.ShowSOList_Planning(Convert.ToInt32(cmbConvPartyName.SelectedValue), dpSODate.Value, logIn.BU_ID) select data).ToList();
                sfDataGrid1.DataSource = null;
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;


                }

                groupBox2.Visible = true;
                //txtSearch.Focus();
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

               
                if (dgProducts.Rows[0].Cells["Item_Description"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Planning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Save();
                }

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

                var result = db.Sp_autoincrement_Forging_ProdPlanning(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtSlipNo.Text = result.FirstOrDefault().Report_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindDroupDown_Lookup()
        {
            try
            {



                
               
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

        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSlipNo.Text;
                if (txtSlipNo.Text != "")
                //if ((from u in db.GoodsReceiptNote_Masters where u.Grn_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Forge_Plan_Delete(myString, logIn.company);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSlipNo.Text;

                }

                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Production_Requirement SC = new Production_Requirement();
                    SC.Planning_Ref_No = myString;
                    SC.Planning_Date = dpSODate.Value;
                    SC.Planning_Make_To_Stock = checkBox1.Checked;
                    SC.Planning_Conversion = checkBox2.Checked;
                    SC.Planning_For = Convert.ToInt32(cmbConvPartyName.SelectedValue);

                    //SC.Mo_Date = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                    SC.Item_Code = (dgProducts.Rows[i].Cells["Item_No"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_No"].Value);
                    var d1 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString() && a.Company_ID == logIn.company select new { a.id }).ToList();
                   
                    SC.Item_Grade = d1[0].id; 
                     SC.Prod_Length = (dgProducts.Rows[i].Cells["Prod_Length"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Prod_Length"].Value).ToString();
                    SC.Order_Qty = (dgProducts.Rows[i].Cells["Order_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Order_Qty"].Value);
                    SC.Stock_Available = (dgProducts.Rows[i].Cells["Stock_Available"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Available"].Value);

                    SC.Required_Qty_MT = (dgProducts.Rows[i].Cells["To_Be_Rolled"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["To_Be_Rolled"].Value);
                   SC.Required_Qty_Nos = (dgProducts.Rows[i].Cells["No_Of_Pieces"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["No_Of_Pieces"].Value);
                 
                    SC.SO_Ref_No = (dgProducts.Rows[i].Cells["SO_NO"].Value == null) ? "" : (dgProducts.Rows[i].Cells["SO_NO"].Value).ToString();
                    SC.SO_Item_No = (dgProducts.Rows[i].Cells["So_line_Item_No"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["So_line_Item_No"].Value);

                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Company_Id = logIn.company;
                    SC.Created_By = lblCreatedBy.Text;
                    SC.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Production_Requirements.InsertOnSubmit(SC);
                }
                db.SubmitChanges();

                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            
            db.sp_Forge_Plan_Delete(txtSlipNo.Text, logIn.company);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }
        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_No", typeof(string));                    
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("Prod_Length", typeof(string));
                    dtexisting.Columns.Add("No_Of_Pieces", typeof(string));                   
                    dtexisting.Columns.Add("Order_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Available", typeof(string));
                    dtexisting.Columns.Add("To_Be_Rolled", typeof(string));                  
                   
                    dtexisting.Columns.Add("SO_NO", typeof(string));
                    dtexisting.Columns.Add("So_line_Item_No", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_No"] = dgProducts.Rows[i].Cells["Item_No"].Value.ToString();                      
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["Prod_Length"] = dgProducts.Rows[i].Cells["Prod_Length"].Value.ToString();
                        dr["No_Of_Pieces"] = dgProducts.Rows[i].Cells["No_Of_Pieces"].Value.ToString();                        
                        dr["Order_Qty"] = dgProducts.Rows[i].Cells["Order_Qty"].Value.ToString();                       
                        dr["Stock_Available"] = dgProducts.Rows[i].Cells["Stock_Available"].Value.ToString();
                        dr["To_Be_Rolled"] = dgProducts.Rows[i].Cells["To_Be_Rolled"].Value.ToString();                       
                        dr["SO_NO"] = dgProducts.Rows[i].Cells["SO_NO"].Value.ToString();
                        dr["So_line_Item_No"] = dgProducts.Rows[i].Cells["So_line_Item_No"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();

                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }

                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_No", typeof(string));              
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("Prod_Length", typeof(string));
                dtgetproducts.Columns.Add("No_Of_Pieces", typeof(string));                
                dtgetproducts.Columns.Add("Order_Qty", typeof(string));
                dtgetproducts.Columns.Add("Stock_Available", typeof(string));
                dtgetproducts.Columns.Add("To_Be_Rolled", typeof(string));
                dtgetproducts.Columns.Add("SO_NO", typeof(string));
                dtgetproducts.Columns.Add("So_line_Item_No", typeof(string));
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                //listBox.Items.Clear();
                // Get the selected items of SfDataGrid
                //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                //var row = this.sfDataGrid1.SelectedItem;

                //string ProdCode;
                //string SoNo;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                        var SONoCol = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var Item_Code = (rowData.GetType().GetProperty("Item_No").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("Item_Description").GetValue(rowData, null).ToString());
                            var Item_Grade = (rowData.GetType().GetProperty("Item_Grade").GetValue(rowData, null).ToString());
                            var Prod_Length = (rowData.GetType().GetProperty("Prod_Length").GetValue(rowData, null).ToString());
                            var PO_Qty = (rowData.GetType().GetProperty("Order_Qty").GetValue(rowData, null).ToString());
                            var Bal_Qty = (rowData.GetType().GetProperty("To_Be_Rolled").GetValue(rowData, null).ToString());
                            var PO_No_Pcs = (rowData.GetType().GetProperty("No_Of_Pieces").GetValue(rowData, null).ToString());                           
                            var SO_Ref_No = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());
                            var So_line_Item_No = (rowData.GetType().GetProperty("So_line_Item_No").GetValue(rowData, null).ToString());

                            drgetproducts = dtgetproducts.NewRow();
                           

                            drgetproducts["Item_No"] = Item_Code.ToString(); ;
                            drgetproducts["Item_Description"] = Item_Description.ToString(); ;
                            drgetproducts["Item_Grade"] = Item_Grade.ToString();
                            drgetproducts["Prod_Length"] = Prod_Length.ToString();
                            drgetproducts["No_Of_Pieces"] = PO_No_Pcs.ToString();
                            drgetproducts["Order_Qty"] = PO_Qty.ToString();
                           
                            drgetproducts["To_Be_Rolled"] = Bal_Qty.ToString();
                            drgetproducts["SO_NO"] = SO_Ref_No.ToString();
                            drgetproducts["So_line_Item_No"] = So_line_Item_No.ToString();
                            drgetproducts["Remarks"] = "";


                            int ordId = 0;
                            
                            //drgetproducts["Disc_Per"] = 0;                            
                           
                            //drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            //drgetproducts["Bal_Qty"] = Bal_Qty.ToString();
                            DateTime t = dpSODate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                            drgetproducts["Stock_Available"] = "0";
                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                drgetproducts["Stock_Available"] = stock[0].ClosingQty;
                                //drgetproducts["Stock_Price"] = stock[0].CBPrice;
                            }
                           
                           
                           

                            //drgetproducts["SO_Item_No"] = "";
                            //drgetproducts["SO_Ref_No"] = SO_Ref_No.ToString();
                            //drgetproducts["Item_No"] = "";
                            //drgetproducts["Remarks"] = "";

                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                        }
                    }
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                //dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;
               
                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "Prod Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                //{
                //    y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                //    q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                //    v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                //}
                //TxtrolledQty.Text = (y - q - v).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addSections(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                if (columnName == "Item Description")
                {
                    var Prodname = (from a in db.Products
                                    join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                    join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
                                    where a.Company_ID == logIn.company && a.Prod_Type_Id == 139
                                    select new { a.Prod_Name, a.prod_ID }).ToList();
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
                if (columnName == "Prod Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mtrl_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
            }
            catch
            {
            }

        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Rolled_Qty = 0, Finsihed_Qty = 0, QC_Rej_Qty = 0, EC_Qty = 0, MR_Qty = 0, BL_Qty = 0, Local_Qty = 0, totA = 0, sgp = 0, igp = 0;

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                //DataGridViewRow R2 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index-1];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                string prodgrade = "";
                string prodname = "";
                string Prevprodgrade = "";
                string Prevprodname = "";
                if (columnName == "Item_Description")
                {


                    prodname = R1.Cells["Item_Description"].Value.ToString();
                    //Prevprodname = R2.Cells["Section_Produced"].Value.ToString();


                    var getProductName = (from s in db.Get_ProductsList_TSL(logIn.company, 1, prodname, prodgrade)
                                          select new { s.prod_ID, s.Prod_Code, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code, s.Price }).FirstOrDefault();



                    if (getProductName != null)
                    {

                        R1.Cells["Item_No"].Value = getProductName.prod_ID.ToString();

                    }
                }
                if (columnName == "Item_Grade")
                {

                    if (R1.Cells["Item_Grade"].Value != null || R1.Cells["Item_Grade"].Value.ToString() != "")
                    {
                        prodgrade = R1.Cells["Item_Grade"].Value.ToString();
                        //Prevprodgrade = R2.Cells["Mtrl_Grade"].Value.ToString();

                        if (prodgrade != "")
                        {
                            var S = (from a in db.QA_Mtrl_Grade_Masters
                                     where a.Company_ID == logIn.company && a.Material_Grade == prodgrade
                                     select a).ToList();
                            if (S.Count > 0)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Material Grade Entered is invalid");
                                R1.Cells["Item_Grade"].Value = "";
                                return;
                            }
                        }

                    }

                }
                if (columnName == "Stock_Available")
                {

                    if (R1.Cells["Item_Grade"].Value != null || R1.Cells["Item_Grade"].Value.ToString() != "")
                    {
                        decimal  reqQty = Convert.ToDecimal(R1.Cells["Order_Qty"].Value.ToString());
                        decimal stkQty = Convert.ToDecimal(R1.Cells["Stock_Available"].Value.ToString());
                        //Prevprodgrade = R2.Cells["Mtrl_Grade"].Value.ToString();

                        R1.Cells["To_Be_Rolled"].Value = reqQty - stkQty;

                    }

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
                //txtSlipNo.Text = ProductionManagement.Transactions.frmForge_Planning_List.SO_No;
                //String myString = "";
                //int QuoteMasterID = 0;               
                //var da = (from obj in db.Forge_ProdPlannings
                //          where obj.PlanningRef == txtSlipNo.Text && obj.Company_ID == logIn.company
                //          select obj).ToList();

                //if (da.Count > 0)
                //{
                //    QuoteMasterID = da[0].id;
                //    dpSODate.Text = da[0].PlanningRefDate.ToString();
                  
                //    cmbShift.Text = da[0].Prod_Shift;
                //    cmbForgingPress.SelectedValue = da[0].Prod_Machine;
                //    cmbMtrlGroup.SelectedValue = da[0].Material_Type;              
                //   // cmbStatus.SelectedValue = da[0].Status;
                //    lblCreatedBy.Text = da[0].Created_By;
                //    lblModified.Text = da[0].Modified_BY;
                //}


                //var dm1 = (from s in db.Forge_ProdPlannings
                //           join mc in db.Forge_Mfg_Order_Childs on new {x1= s.MO_Master_ID, x2 = s.Prod_Code} equals new {x1 = mc.MO_Master_ID, x2 = mc.Prod_Code}
                //           join m in db.Forge_MFG_Order_Masters on s.Mo_No equals m.MO_No
                //           join c in db.Supplier_informations on m.Customer_Name equals c.ID
                //           where s.PlanningRef == txtSlipNo.Text && s.Company_ID == logIn.company

                //           select new

                //           {
                //               s.Mo_No, 
                //               Customer_Name= c.Supplier_Name,
                //               s.MO_Sno,
                //               Item_No = s.Prod_Code,
                //               Item_Description = mc.Product_Description,
                //               Item_Grade = s.MaterialGrade,
                //               BalQty=s.Qty,
                //               ForgingSize= s.Forging_Size,
                //               Forging_Wt= s.ForgingWt,
                //               RM_Sec= s.RMSec,
                //               RM_WT = s.RMAvbl,
                //               stage = s.Stage,
                //               sno = s.F_SNo   
                //           });




                //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataTable dtr = new DataTable();
                //da2.Fill(dtr);
                //if (dtr.Rows.Count >= 0)
                //    dgProducts.DataSource = dtr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
