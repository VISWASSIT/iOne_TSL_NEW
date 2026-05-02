using Ione_DAL;
using OpenCvSharp;

//using Microsoft.Office.Interop.Excel;
using Syncfusion.Windows.Forms.Chart.SvgBase;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
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
    public partial class frmDayProductionPlan: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string ReportNo, ItemCode, RecQty, Suppname, var, RCode, ItemName, MtrlGrade;
        public static DateTime PlanDate;
        public static Boolean convprod = false;
        public static int plan_master_id;
        public frmDayProductionPlan()
        {
            InitializeComponent();
        }

        private void frmDayProductionPlan_Load(object sender, EventArgs e)
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
        }

        private void dgSpecs_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgItemDetails.CurrentCell.ColumnIndex;
                string columnName = dgItemDetails.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "Item_Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "Item_Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                //if (tb3 != null && columnName == "Production_For")
                //{
                //    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                //    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                //    addSections(DataColl);
                //    tb3.AutoCompleteCustomSource = DataColl;
                //}


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
                DataGridViewRow R1 = dgItemDetails.Rows[dgItemDetails.CurrentRow.Index];

                int columnIndex = dgItemDetails.CurrentCell.ColumnIndex;
                string columnName = dgItemDetails.Columns[columnIndex].HeaderText;
                if (columnName == "Item_Description")
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
                if (columnName == "Item_Grade")
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
                if (columnName == "Production_For")
                {
                     DataTable dt = new DataTable();
                    dt.Columns.Add("Mtrl_Grade");
                    
                        dt.Rows.Add("Own");
                        dt.Rows.Add("Conversion - RINL");
                        dt.Rows.Add("Conversion - TATA");


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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var d = (from data in db.ShowRequirement_For_Scheduling(logIn.company,dtRevDate.Value,  logIn.BU_ID) select data).ToList();
                sfDataGrid1.DataSource = null;
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["prod_name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_name"].FilterRowCondition = FilterRowCondition.Contains;


                    this.sfDataGrid1.Columns["Item_Grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Grade"].FilterRowCondition = FilterRowCondition.Contains;


                }

                groupBox2.Visible = true;
                //txtSearch.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void dtRevDate_Leave(object sender, EventArgs e)
        {
            if (cmbConvPartyName.Text != "")
            {
                GetPlanningData();
            }
        }

        private void dtRevDate_LocationChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            SqlCommand cmd2 = new SqlCommand("delete  from [Temp_Prod_Plan]", con);
            con.Open();
            cmd2.ExecuteNonQuery();
            con.Close();
            for (int i = 0; i < dgItemDetails.RowCount - 1; i++)
            {
                decimal pqty = (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Plan_Qty"].Value);
                if (pqty > 0)
                {
                    Temp_Prod_Plan SC = new Temp_Prod_Plan();
                    SC.Item_Code = (dgItemDetails.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgItemDetails.Rows[i].Cells["Item_Code"].Value);
                    SC.Item_Grade = dgItemDetails.Rows[i].Cells["Item_Grade"].Value.ToString();
                    SC.Prod_Length = (dgItemDetails.Rows[i].Cells["Req_Length"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Req_Length"].Value).ToString();

                    SC.Required_Qty = (dgItemDetails.Rows[i].Cells["Required_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Required_Qty_MT"].Value);
                    SC.Planned_Qty = (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Plan_Qty"].Value);
                    SC.Production_For = (dgItemDetails.Rows[i].Cells["Production_For"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Production_For"].Value).ToString();
                    db.Temp_Prod_Plans.InsertOnSubmit(SC);
                }
            }
            db.SubmitChanges();

            //Get Data from temp table

            var dm1 = (from s in db.View_Temp_DayProdPlans





                       select new

                       {
                           Item_Code = s.Item_Code,

                           Item_Description = s.Item_Description,
                           Item_Grade = s.Item_Grade,
                           Required_Qty_MT = s.Required_Qty_MT,
                           Plan_Qty = s.Plan_Qty,
                           Production_For = s.Production_For,
                           s.Batch_No,
                           s.RM_Lot_Alloted


                       });
            SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm1);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd3);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count >= 0)
                dgPlanningSummary.DataSource = dtr;
            DateTime t = dtRevDate.Value;
            string f1 = "";
            decimal tplanqty=0;
            decimal trmqty = 0;
            for (int i = 0; i < dgPlanningSummary.RowCount - 1; i++)
            {
                if (cmbConvPartyName.Text != "Own")
                {
                    string s = cmbConvPartyName.Text.Substring(0, 1);
                    f1 = s+"_"+t.ToString("yyyyMMdd");
                }
                else
                {
                    f1 = "S_"+ t.ToString("yyyyMMdd");
                }
                if (i == 0)
                {
                    dgPlanningSummary.Rows[i].Cells["Batch_No"].Value = f1;

                }
                else
                {
                    dgPlanningSummary.Rows[i].Cells["Batch_No"].Value = f1 + "/" + i;
                }
                tplanqty += (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == "" || dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == null || dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value);
                trmqty += (dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == "" || dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == null || dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value);

                
            }
            txtTPlanQty.Text = tplanqty.ToString();
            txtTRMAlloted.Text = trmqty.ToString();
        }

        private void dgPlanningSummary_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.KeyCode == Keys.F4)
                {
                    if(txtID.Text == string.Empty)
                    {
                        Production_DayPlan_Master SC = new Production_DayPlan_Master();
                        SC.Prod_Date = Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd"));
                        SC.Conversion_Prod = checkBox1.Checked;
                        //if (checkBox1.Checked == true)
                        //{
                        SC.Production_For = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                        SC.Company_ID = logIn.company;
                        SC.Created_By = lblCreatedBy.Text;
                        SC.Modfied_By = logIn.username + "-" + DateTime.Now;
                        db.Production_DayPlan_Masters.InsertOnSubmit(SC);
                        db.SubmitChanges();
                    }

                    
                    var d1 = (from a in db.Production_DayPlan_Masters where a.Prod_Date == Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd")) && a.Production_For == Convert.ToInt32(cmbConvPartyName.SelectedValue) && a.Company_ID == logIn.company select new { a.id }).ToList();
                    txtID.Text = d1[0].id.ToString();
                    
                   
                    plan_master_id =  Convert.ToInt32(txtID.Text);

                    int i = dgPlanningSummary.CurrentCell.RowIndex;
                    ItemCode = (dgPlanningSummary.Rows[i].Cells["s_Item_Code"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["s_Item_Code"].Value).ToString();
                    RecQty = (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value).ToString();
                    ItemName = (dgPlanningSummary.Rows[i].Cells["s_Item_Description"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["s_Item_Description"].Value).ToString();
                    MtrlGrade = (dgPlanningSummary.Rows[i].Cells["s_Item_Grade"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["s_Item_Grade"].Value).ToString();

                    //if (checkBox1.Checked == true )
                    //{
                    //    convprod = true;
                        Suppname = cmbConvPartyName.Text;
                    //}
                    //else
                    //{
                    //    convprod = false;
                    //}
                    if (ItemCode != "" && RecQty != "")
                        if (ItemCode != "" && RecQty != "")

                        {
                            // GlobalVariables.FormName = "Production_Repot";
                            ioneNet.ProductionManagement.Transactions.frmRMLotAllotment form = new ioneNet.ProductionManagement.Transactions.frmRMLotAllotment();
                            //ioneNet.Masters.ProdSearch.frmName = "SOrder";       

                            PlanDate = dtRevDate.Value;


                            form.ShowDialog();

                            dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value = ioneNet.ProductionManagement.Transactions.frmRMLotAllotment.TotQty;
                            decimal tplanqty = 0;
                            decimal trmqty = 0;
                            for (int j = 0; j < dgPlanningSummary.RowCount - 1; j++)
                            {
                                
                                
                                tplanqty += (dgPlanningSummary.Rows[j].Cells["s_Plan_Qty"].Value == "" || dgPlanningSummary.Rows[j].Cells["s_Plan_Qty"].Value == null || dgPlanningSummary.Rows[j].Cells["s_Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[j].Cells["s_Plan_Qty"].Value);
                                trmqty += (dgPlanningSummary.Rows[j].Cells["RM_Lot_Alloted"].Value == "" || dgPlanningSummary.Rows[j].Cells["RM_Lot_Alloted"].Value == null || dgPlanningSummary.Rows[j].Cells["RM_Lot_Alloted"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[j].Cells["RM_Lot_Alloted"].Value);


                            }
                            txtTPlanQty.Text = tplanqty.ToString();
                            txtTRMAlloted.Text = trmqty.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Cannot Proceed Without Item Code and Total Qty Rolled");
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                //if ((from a in db.sp_DayProduction_Plan_Delete_All where a.Company_Id == logIn.company && a.TC_NO == txtTCNo.Text select a).Count() > 0)
                //{
                    db.sp_DayProduction_Plan_Delete_All(Convert.ToInt32(txtID.Text), logIn.company);
                //}

                MessageBox.Show("Record Deleted Successfully");
                this.Close();

            }
        }

        private void dtRevDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {




                var p = (from s in db.Production_DayPlan_Masters
                          

                         select new
                         {
                             s.id,
                             s.Prod_Date,
                             s.Production_For
                            
                         }
                        ).Distinct().ToList();

                if (p.Count >= 0)
                {
                    sfDataGrid2.DataSource = p;
                    //this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    //this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["Report_Ref_No"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["Report_Ref_No"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Report_Ref_No"].FilterRowCondition = FilterRowCondition.Contains;

                    //this.sfDataGrid1.Columns["Fg_Lot_No"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["Fg_Lot_No"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["Fg_Lot_No"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Fg_Lot_No"].FilterRowCondition = FilterRowCondition.Contains;
                }
                else
                {

                }
                groupBox1.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfDataGrid2_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            int i = sfDataGrid2.CurrentCell.RowIndex;
            var rowData = sfDataGrid2.GetRecordAtRowIndex(i);

            var mappingName = sfDataGrid2.Columns["id"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellValue = (rowData.GetType().GetProperty("id").GetValue(rowData, null).ToString());
            var dm1 = (from s in db.Production_DayPlan_Masters where s.id == Convert.ToInt32(cellVaue)
                       select new { s.id, s.Prod_Date, s.Production_For, s.Conversion_Prod, s.Created_By, s.Modfied_By }).ToList();

            //checkBox1.Checked = false;
            dtRevDate.Text = dm1[0].Prod_Date.ToString();
            cmbConvPartyName.SelectedValue = Convert.ToInt32(dm1[0].Production_For);
            //if (dm1[0].Conversion_Prod == true)
            //{
            //    checkBox1.Checked = true;
            //}
            lblCreatedBy.Text = dm1[0].Created_By;
            lblModified.Text = dm1[0].Modfied_By;
            txtID.Text = dm1[0].id.ToString();
            //txtInvNo.Text = cellVaue.ToString();
            var dm2 = (from s in db.Production_DayPlans
                       join p in db.Products on s.Item_Code equals p.prod_ID
                       join g in db.QA_Mtrl_Grade_Masters on s.Item_Grade equals g.id
                       where s.Master_ID == Convert.ToInt32(txtID.Text) 
                       select new
                       {
                           Plan_Ref_No = s.Planning_Ref_No,
                           s.Item_Code,

                           Item_Description = p.Prod_Name,
                           Item_Grade = g.Material_Grade,
                           Req_Length = s.Prod_Length,
                           Required_Qty_Nos = s.Required_Qty_Nos,
                           Required_Qty_MT = s.Required_Qty,
                           Plan_Qty = s.Planned_Qty,
                           Plan_Qty_Nos = s.Planned_Qty_Nos,
                           //Batch_No = s.Batch_No,
                           Production_For = s.Production_For,

                           //RM_Lot_Alloted = s.RM_Lot_Alloted,
                           s.Remarks

                       });
            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm2);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count >= 0)
            {
                dgItemDetails.DataSource = dtr;
            }
            else
            {
                //dgItemDetails.DataSource = null;
            }

            var dm3 = (from s in db.Production_DayPlan_Summaries
                       
                       join p in db.Products on s.Item_Code equals p.prod_ID
                       join g in db.QA_Mtrl_Grade_Masters on s.Item_Grade equals g.id
                       where s.Master_Id == Convert.ToInt32(txtID.Text) 

                       orderby s.id
                       select new
                       {
                           s.Item_Code,
                           Item_Description = p.Prod_Name,
                           Item_Grade = g.Material_Grade,

                           Required_Qty_MT = s.Required_Qty,
                           Plan_Qty = s.Planned_Qty,
                           Production_For = s.Production_For,
                           Batch_No = s.Batch_No,

                           RM_Lot_Alloted = s.RM_Lot_Alloted,

                       });

            SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm3);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dtr1 = new DataTable();
            da3.Fill(dtr1);
            if (dtr1.Rows.Count >= 0)
            {
                dgPlanningSummary.DataSource = dtr1;
            }
            else
            {
                //dgPlanningSummary.DataSource = null;
            }
            groupBox1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        System.Data.DataRow drgetproducts;

        private void cmbConvPartyName_Leave(object sender, EventArgs e)
        {
            if(cmbConvPartyName.Text !="")
            {
                GetPlanningData();
            }
        }

        DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgItemDetails.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Plan_Ref_No", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("Req_Length", typeof(string));
                    dtexisting.Columns.Add("Required_Qty_Nos", typeof(string));
                    dtexisting.Columns.Add("Required_Qty_MT", typeof(string));
                    dtexisting.Columns.Add("Plan_Qty", typeof(string));
                    dtexisting.Columns.Add("Plan_Qty_Nos", typeof(string));
                    //dtexisting.Columns.Add("Batch_No", typeof(string));
                    dtexisting.Columns.Add("Production_For", typeof(string));
                    //dtexisting.Columns.Add("RM_Lot_Alloted", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgItemDetails.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Plan_Ref_No"] = dgItemDetails.Rows[i].Cells["Plan_Ref_No"].Value.ToString();
                        dr["Item_Code"] = dgItemDetails.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Item_Description"] = dgItemDetails.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgItemDetails.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["Req_Length"] = dgItemDetails.Rows[i].Cells["Req_Length"].Value.ToString();
                        dr["Required_Qty_Nos"] = dgItemDetails.Rows[i].Cells["Required_Qty_Nos"].Value.ToString();
                        dr["Required_Qty_MT"] = dgItemDetails.Rows[i].Cells["Required_Qty_MT"].Value.ToString();
                        dr["Plan_Qty"] = dgItemDetails.Rows[i].Cells["Plan_Qty"].Value.ToString();
                        dr["Plan_Qty_Nos"] = dgItemDetails.Rows[i].Cells["Plan_Qty_Nos"].Value.ToString();
                        //dr["Batch_No"] = dgItemDetails.Rows[i].Cells["Batch_No"].Value.ToString();
                        dr["Production_For"] = dgItemDetails.Rows[i].Cells["Production_For"].Value.ToString();
                        //dr["RM_Lot_Alloted"] = dgItemDetails.Rows[i].Cells["RM_Lot_Alloted"].Value.ToString();
                        dr["Remarks"] = dgItemDetails.Rows[i].Cells["Remarks"].Value.ToString();

                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }

                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Plan_Ref_No", typeof(string));
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("Req_Length", typeof(string));
                dtgetproducts.Columns.Add("Required_Qty_Nos", typeof(string));
                dtgetproducts.Columns.Add("Required_Qty_MT", typeof(string));
                dtgetproducts.Columns.Add("Plan_Qty", typeof(string));
                dtgetproducts.Columns.Add("Plan_Qty_Nos", typeof(string));
                //dtgetproducts.Columns.Add("Batch_No", typeof(string));
                dtgetproducts.Columns.Add("Production_For", typeof(string));
                //dtgetproducts.Columns.Add("RM_Lot_Alloted", typeof(string));
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
                            var Plan_Ref_No = (rowData.GetType().GetProperty("Planning_Ref_No").GetValue(rowData, null).ToString());
                            var Item_Code = (rowData.GetType().GetProperty("item_code").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("prod_name").GetValue(rowData, null).ToString());
                            var Item_Grade = (rowData.GetType().GetProperty("Item_Grade").GetValue(rowData, null).ToString());
                            var Prod_Length = (rowData.GetType().GetProperty("Prod_Length").GetValue(rowData, null).ToString());
                            var To_Plan_Qty_Nos = (rowData.GetType().GetProperty("To_Plan_Qty_Nos").GetValue(rowData, null).ToString());
                            var To_Plan_Qty = (rowData.GetType().GetProperty("To_Plan_Qty").GetValue(rowData, null).ToString());
                            var Plan_Basis = (rowData.GetType().GetProperty("Plan_Basis").GetValue(rowData, null).ToString());

                            drgetproducts = dtgetproducts.NewRow();

                            drgetproducts["Plan_Ref_No"] = Plan_Ref_No.ToString(); ;
                            drgetproducts["Item_Code"] = Item_Code.ToString(); ;
                            drgetproducts["Item_Description"] = Item_Description.ToString(); ;
                            drgetproducts["Item_Grade"] = Item_Grade.ToString();
                            drgetproducts["Req_Length"] = Prod_Length.ToString();
                            drgetproducts["Required_Qty_Nos"] = To_Plan_Qty_Nos.ToString();
                            drgetproducts["Required_Qty_MT"] = To_Plan_Qty.ToString();

                            drgetproducts["Plan_Qty"] ="0";
                            drgetproducts["Plan_Qty_Nos"] = "0";
                            //drgetproducts["Batch_No"] = "";
                            drgetproducts["Production_For"] = Plan_Basis.ToString();
                            //drgetproducts["RM_Lot_Alloted"] = "";
                            drgetproducts["Remarks"] = "";


                            int ordId = 0;

                         

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

                dgItemDetails.DataSource = dtgetSelectedprducts;

                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {


                if (dgPlanningSummary.Rows[0].Cells["s_Item_Description"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Schedule", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
                decimal tplanqty = 0;
                decimal trmqty = 0;

                for (int i = 0; i < dgPlanningSummary.RowCount - 1; i++)
                {
                        
                    tplanqty += (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == "" || dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == null || dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value);
                    trmqty += (dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == "" || dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == null || dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value);


                }
                if(trmqty < tplanqty)
                {
                    MessageBox.Show("Total RM Allotment Qty Should be More Than Total Planned Qty, Cannot Proceed with Save The Data");
                    return;
                }
                    

                Save();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GetPlanningData()
        {
            try
            {

                string pd = dtRevDate.Value.ToString("yyyy-MM-dd");

                int pid = 0;
                //if(checkBox1.Checked)
                //{
                if (cmbConvPartyName.Text != "")
                {

                    var dm = (from s in db.Production_DayPlan_Masters

                              where s.Prod_Date == Convert.ToDateTime(pd) && s.Company_ID == logIn.company && s.Production_For == Convert.ToInt32(cmbConvPartyName.SelectedValue)

                              select s).ToList();
                    if (dm.Count > 0)
                    {
                        pid = dm[0].id;
                        txtID.Text = dm[0].id.ToString();
                    }
                    else
                    {
                        txtID.Text = "";
                        return;
                    }

                    var dm1 = (from d in db.Production_DayPlans
                               join p in db.Products on d.Item_Code equals p.prod_ID
                               join g in db.QA_Mtrl_Grade_Masters on d.Item_Grade equals g.id

                               where d.Master_ID == Convert.ToInt32(pid)
                               orderby d.id
                               select new
                               {
                                   Plan_Ref_No = d.Planning_Ref_No,
                                   d.Item_Code,
                                   Item_Description = p.Prod_Name,
                                   Item_Grade = g.Material_Grade,
                                   Req_Length = d.Prod_Length,
                                   Required_Qty_Nos = d.Required_Qty_Nos,
                                   Required_Qty_MT = d.Required_Qty,
                                   Plan_Qty = d.Planned_Qty,
                                   Plan_Qty_Nos = d.Planned_Qty_Nos,
                                   //Batch_No = s.Batch_No,
                                   Production_For = d.Production_For,

                                   //RM_Lot_Alloted = s.RM_Lot_Alloted,
                                   d.Remarks


                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgItemDetails.DataSource = dtr;
                    }
                    else
                    {
                        //dgItemDetails.DataSource = null;
                    }

                    var dm2 = (from s in db.Production_DayPlan_Summaries
                               join d in db.Production_DayPlans on s.Prod_Date equals d.pDate
                               join p in db.Products on s.Item_Code equals p.prod_ID
                               join g in db.QA_Mtrl_Grade_Masters on s.Item_Grade equals g.id
                               where s.Master_Id == pid

                               orderby s.id
                               select new
                               {
                                   s.Item_Code,
                                   Item_Description = p.Prod_Name,
                                   Item_Grade = g.Material_Grade,

                                   Required_Qty_MT = s.Required_Qty,
                                   Plan_Qty = s.Planned_Qty,
                                   Production_For = s.Production_For,
                                   Batch_No = s.Batch_No,

                                   RM_Lot_Alloted = s.RM_Lot_Alloted,

                               });

                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dtr1 = new DataTable();
                    da3.Fill(dtr1);
                    if (dtr1.Rows.Count >= 0)
                    {
                        dgPlanningSummary.DataSource = dtr1;
                    }
                    else
                    {
                        dgPlanningSummary.DataSource = null;
                    }
                }
                else
                {
                    MessageBox.Show("Select Convertion Party Name");
                    cmbConvPartyName.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        public void Save()
        {
            try
            {
                string pd = dtRevDate.Value.ToString("yyyy-MM-dd");

                int pid = (txtID.Text ==string.Empty) ? Convert.ToInt32("0") : Convert.ToInt32(txtID.Text);



                if ((from a in db.Production_DayPlan_Masters where a.id == pid select a).Count() > 0)
                {
                    var pb1 = db.Production_DayPlan_Masters.Where(w => w.id == pid).FirstOrDefault();
                    pb1.Prod_Date = Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd"));
                    //pb1.Conversion_Prod = checkBox1.Checked;
                    pb1.Production_For = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                    pb1.Company_ID = logIn.company;
                    pb1.Created_By = lblCreatedBy.Text;
                    pb1.Modfied_By = logIn.username + "-" + DateTime.Now;
                    db.SubmitChanges();
                }
                else
                {
                    Production_DayPlan_Master SC = new Production_DayPlan_Master();
                    SC.Prod_Date = Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd"));
                    //SC.Conversion_Prod = checkBox1.Checked;
                    SC.Production_For = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                    SC.Company_ID = logIn.company;
                    SC.Created_By = lblCreatedBy.Text;
                    SC.Modfied_By = logIn.username + "-" + DateTime.Now;
                    db.Production_DayPlan_Masters.InsertOnSubmit(SC);
                    db.SubmitChanges();
                }
                var d3 = (from a in db.Production_DayPlan_Masters where a.Prod_Date == Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd")) && a.Production_For == Convert.ToInt32(cmbConvPartyName.SelectedValue) && a.Company_ID == logIn.company select new { a.id }).ToList();
                txtID.Text = d3[0].id.ToString();

                db.sp_DayProduction_Plan_Delete(Convert.ToInt32(txtID.Text), logIn.company);
                for (int i = 0; i < dgItemDetails.RowCount - 1; i++)
                {
                    decimal pqty = (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Plan_Qty"].Value);
                    if (pqty > 0)
                    {
                        Production_DayPlan SC1 = new Production_DayPlan();
                        var d1 = (from a in db.Production_DayPlan_Masters where a.Prod_Date == Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd")) && a.Production_For == Convert.ToInt32(cmbConvPartyName.SelectedValue) && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC1.Master_ID = d1[0].id;
                        
                        SC1.pDate = Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd"));
                       
                        SC1.Planning_Ref_No = (dgItemDetails.Rows[i].Cells["Plan_Ref_No"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Plan_Ref_No"].Value).ToString();
                        //SC.Mo_Date = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                        SC1.Item_Code = (dgItemDetails.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgItemDetails.Rows[i].Cells["Item_Code"].Value);
                        var d2 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == dgItemDetails.Rows[i].Cells["Item_Grade"].Value.ToString() && a.Company_ID == logIn.company select new { a.id }).ToList();

                        SC1.Item_Grade = d2[0].id;
                        SC1.Prod_Length = (dgItemDetails.Rows[i].Cells["Req_Length"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Req_Length"].Value).ToString();

                        SC1.Required_Qty = (dgItemDetails.Rows[i].Cells["Required_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Required_Qty_MT"].Value);
                        SC1.Required_Qty_Nos = (dgItemDetails.Rows[i].Cells["Required_Qty_Nos"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgItemDetails.Rows[i].Cells["Required_Qty_Nos"].Value);
                        SC1.Planned_Qty = (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgItemDetails.Rows[i].Cells["Plan_Qty"].Value);

                        SC1.Planned_Qty_Nos = (dgItemDetails.Rows[i].Cells["Plan_Qty_Nos"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgItemDetails.Rows[i].Cells["Plan_Qty_Nos"].Value);

                        //SC.Batch_No = (dgItemDetails.Rows[i].Cells["Batch_No"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Batch_No"].Value).ToString();
                        SC1.Production_For = cmbConvPartyName.Text;
                        //SC.RM_Lot_Alloted = (dgItemDetails.Rows[i].Cells["RM_Lot_Alloted"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["RM_Lot_Alloted"].Value).ToString();
                        SC1.Remarks = (dgItemDetails.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Remarks"].Value).ToString();
                        
                        db.Production_DayPlans.InsertOnSubmit(SC1);
                    }
                }
                db.SubmitChanges();
                //Save Summary
                for (int i = 0; i < dgPlanningSummary.RowCount - 1; i++)
                {
                    decimal pqty = (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value);
                    if (pqty > 0)
                    {
                        Production_DayPlan_Summary SC2 = new Production_DayPlan_Summary();

                        var d1 = (from a in db.Production_DayPlan_Masters where a.Prod_Date == Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd")) && a.Production_For == Convert.ToInt32(cmbConvPartyName.SelectedValue) && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC2.Master_Id = d1[0].id;

                        SC2.Prod_Date = Convert.ToDateTime(dtRevDate.Value.ToString("yyyy-MM-dd"));

                        //SC.Mo_Date = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                        SC2.Item_Code = (dgPlanningSummary.Rows[i].Cells["s_Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgPlanningSummary.Rows[i].Cells["s_Item_Code"].Value);
                        var d2 = (from a in db.QA_Mtrl_Grade_Masters where a.Material_Grade == dgPlanningSummary.Rows[i].Cells["s_Item_Grade"].Value.ToString() && a.Company_ID == logIn.company select new { a.id }).ToList();

                        SC2.Item_Grade = d2[0].id;
                        
                        SC2.Required_Qty = (dgPlanningSummary.Rows[i].Cells["s_Required_Qty_MT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["s_Required_Qty_MT"].Value);
                        SC2.Planned_Qty = (dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPlanningSummary.Rows[i].Cells["s_Plan_Qty"].Value);

                       
                        SC2.Batch_No = (dgPlanningSummary.Rows[i].Cells["Batch_No"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["Batch_No"].Value).ToString();
                        SC2.Production_For = (dgPlanningSummary.Rows[i].Cells["s_Production_For"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["s_Production_For"].Value).ToString();
                        SC2.RM_Lot_Alloted = (dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value == null) ? "" : (dgPlanningSummary.Rows[i].Cells["RM_Lot_Alloted"].Value).ToString();
                       
                        db.Production_DayPlan_Summaries.InsertOnSubmit(SC2);
                    }
                }
                db.SubmitChanges();

                MessageBox.Show("Record Saved / Updated Successfully");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgItemDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DateTime t = dtRevDate.Value;
            DataGridViewRow R1 = dgItemDetails.Rows[dgItemDetails.CurrentRow.Index];
            //DataGridViewRow R2 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index-1];
            int columnIndex = dgItemDetails.CurrentCell.ColumnIndex;
            string columnName = dgItemDetails.Columns[columnIndex].Name;
            string f1 = t.ToString("yyyyMMdd");
            int m = dgItemDetails.CurrentRow.Index;
            var prod = (from data in db.Products where data.Company_ID ==  logIn.company && data.Prod_Name == R1.Cells["Item_Description"].Value.ToString() 
                        select


                            new
                            {
                                Item_Code = data.prod_ID,                               
                                
                            }).ToList();


            R1.Cells["Item_Code"].Value = prod[0].Item_Code.ToString();
            R1.Cells["Production_For"].Value = cmbConvPartyName.Text;

        }

        private void dgItemDetails_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
                //if (e.KeyCode == Keys.F4)
                //{

                //    int i = dgItemDetails.CurrentCell.RowIndex;
                //    ItemCode = (dgItemDetails.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Item_Code"].Value).ToString();
                //    RecQty = (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Plan_Qty"].Value).ToString();
                //    ItemName = (dgItemDetails.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Item_Description"].Value).ToString();
                //    MtrlGrade = (dgItemDetails.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgItemDetails.Rows[i].Cells["Item_Grade"].Value).ToString();

                //    if (dgItemDetails.Rows[i].Cells["Production_For"].Value.ToString() == "Conversion")
                //    {
                //        convprod = true;
                //    }
                //    else
                //    {
                //        convprod = false;
                //    }
                //    if (ItemCode != "" && RecQty != "")
                //        if (ItemCode != "" && RecQty != "")

                //        {
                //            // GlobalVariables.FormName = "Production_Repot";
                //            ioneNet.ProductionManagement.Transactions.frmRMLotAllotment form = new ioneNet.ProductionManagement.Transactions.frmRMLotAllotment();
                //            //ioneNet.Masters.ProdSearch.frmName = "SOrder";       

                //            PlanDate = dtRevDate.Value;


                //            form.ShowDialog();

                //            //dgSectionProduced.Rows[i].Cells["ReceivedQty"].Value = ioneNet.ProductionManagement.Transactions.frmProdSectionWiseLengths.TotQty;
                //        }
                //        else
                //        {
                //            MessageBox.Show("Cannot Proceed Without Item Code and Total Qty Rolled");
                //        }

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
