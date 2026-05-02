using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmMRP : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmMRP()
        {
            InitializeComponent();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMRP_Load(object sender, EventArgs e)
        {
            BindMasters();
            AutoincrementId();
        }
        public void BindMasters()
        {
            try
            {

                //Bind Products
                var sa = (from a in db.Engg_Mfg_Orders
                          where a.Company_ID == logIn.company
                          select new { a.MO_No }).ToList();
                if (sa.Count > 0)
                {
                    cmbMONo.DataSource = sa;
                    cmbMONo.DisplayMember = "MO_No";
                    cmbMONo.ValueMember = "MO_No";
                    if (cmbMONo.Items.Count > 0)
                    {
                        cmbMONo.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbMONo.SelectedIndex = -1;
                    }
                }
               

                              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbMONo_Leave(object sender, EventArgs e)
        {
            try
            {

                var sa = (from s in db.Engg_Mfg_Orders
                          join u in db.Project_code_Masters on s.Project_ID equals u.id
                          where s.MO_No == cmbMONo.Text && s.Company_ID==logIn.company
                          select new { u.Project_Code, s.Project_ID,s.MO_Qty }).ToList();
                //var sa = (from a in db.Products where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) select new { a.Prod_Primary_UOM_Id, a.Prod_Group_Id,a.Prod_Unit_Wt }).ToList();
                if (sa.Count > 0)
                {

                    txtProjectCode.Text = sa[0].Project_Code.ToString();
                    txtProjectID.Text = sa[0].Project_ID.ToString();
                    txtMRPQty.Text = sa[0].MO_Qty.ToString();
                }


                //Bind product Groups
                var bindGroups = (from m in db.BOM_Projects
                                  join g in db.Product_Groups on m.Material_Group equals g.ID
                                  where m.Company_ID == logIn.company && m.MO_No == cmbMONo.Text
                                  select new
                                  {
                                      g.Prod_Group_Name,
                                      m.Material_Group,
                                  }).Distinct().ToList();

                if (bindGroups.Count > 0)
                {
                    cmbProdGroup.DataSource = bindGroups;
                    cmbProdGroup.DisplayMember = "Prod_Group_Name";
                    cmbProdGroup.ValueMember = "Material_Group";
                    cmbProdGroup.SelectedIndex = -1;

                }

                var ca = (from sq in db.MRPs                         
                          where sq.Company_ID == logIn.company && sq.MO_No == cmbMONo.Text 
                          orderby sq.MRPNo
                          select new
                          {
                              SNo = sq.MRPNo,
                              Prod_Code = sq.MRP_Date
                              
                          }).Distinct();
                SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dt1 = new DataTable();
                da3.Fill(dt1);
                if (dt1.Rows.Count > 0)

                linkLabel1.Text = "Total " + dt1.Rows.Count + " MRP(s) Raised On Selected MO No";
                if(dt1.Rows.Count>= 2)
                {
                    DialogResult result = MessageBox.Show("Already More than 2 MRP Generated Against Selected MO No, Do You to Still Proceed to Generate?", "MRP Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        cmbMONo.Text = "";
                        cmbMONo.Focus();
                    }
                }

                    //dataGridView1.DataSource = dt1;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMONo.Text != "")
                {
                    DateTime dt = dateTimePicker1.Value;
                    string dt1 = dt.ToString("yyyy/MM/dd");
                    sfDataGrid1.DataSource = null;

                    //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();
                    AutoincrementId();
                    SqlDataReader rdr = null;
                    SqlCommand cmd2 = new SqlCommand("SAVEMRP_new", con);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@mrpno", txtMRPRefNo.Text);
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@mrpDate", Convert.ToDateTime(dt1));
                    cmd2.Parameters.AddWithValue("@pmodule", txtModule.Text);
                    cmd2.Parameters.AddWithValue("@qty", Convert.ToInt32(txtMRPQty.Text));
                    cmd2.Parameters.AddWithValue("@project", Convert.ToInt32(txtProjectID.Text));
                    ////cmd2.Parameters.AddWithValue("@category", Convert.ToInt32(cmbProdGroup.SelectedValue.ToString()));
                    cmd2.Parameters.AddWithValue("@bin", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@createdBy", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));
                    cmd2.Parameters.AddWithValue("@ModifiedBy", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));
                    cmd2.Parameters.AddWithValue("@mono", cmbMONo.Text);
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Close();
                        con.Open();
                    }
                    //  con.Open();
                    rdr = cmd2.ExecuteReader();
                    con.Close();
                    // cmd2.ExecuteNonQuery();
                    //DataSet ds2 = new DataSet();
                    //DataTable ds2 = new DataTable();
                    //// da2.Fill(ds2, "x");
                    //da2.Fill(ds2);

                    var d = (from data in db.tempMRPs

                             select new

                             {
                                 SNo = data.SNo,
                                 Item_Code = data.Prod_Code,
                                 Prod_Code = data.PartNo,
                                 Item_Name = data.Prod_Name,
                                 Group_Name = data.Category,
                                 data.uom,
                                 QtyReq = data.QTY,
                                 data.QTY_Avbl,
                                 data.PR_Raised,
                                 data.QTY_To_Procure,
                                 Remarks = data.remarksline
                             }
                             ).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;

                        this.sfDataGrid1.TableSummaryRows.Clear();
                        //this.sfDataGrid1.GroupSummaryRows.Clear();
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Item_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Item_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["Group_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Group_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Group_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Group_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.TableSummaryRows.Clear();
                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        summaryColumn1.Name = "Total Products";
                        summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn1.Format = "Total Products: {Count}";
                        summaryColumn1.MappingName = "Prod_Code";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);







                        DialogResult result = MessageBox.Show("MRP Generated Successdfully, Click Yes to confirm and proceed with PR Generation", "Confirm MRP", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            SqlCommand cmd1 = new SqlCommand();
                            //inv_No1 = row.Cells["Invoice_No"].Value.ToString();
                            cmd1.CommandText = "Delete from MRP where mrpno = @mrpno and company_id = @compname";
                            cmd1.Parameters.AddWithValue("@mrpno", txtMRPRefNo.Text);
                            cmd1.Parameters.AddWithValue("@compname", logIn.company);
                            cmd1.Connection = con;

                            con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();


                            SqlCommand cmd = new SqlCommand();
                            //inv_No1 = row.Cells["Invoice_No"].Value.ToString();
                            cmd.CommandText = "INSERT INTO MRP([MRPNo],[MRP_Date],[MO_No],[Project],[MtrlGroup],[ProjModule],[Project_qty],[Company_ID],[Created_By],[Modified_By],[SNo],[Prod Code],[Prod Name],[PartNo],[Category],[uom],[QTY],[QTY_Avbl],[QTY_To_Procure],[remarksline],Status) select [MRPNo],[MRP_Date],[MO_No],[Project],[MtrlGroup],[ProjModule],[Project_qty],[Company_ID],[Created_By],[Modified_By],[SNo],[Prod Code],[Prod Name],[PartNo],[Category],[uom],[QTY],[QTY_Avbl],[QTY_To_Procure],[remarksline],'6' from tempmrp where mrpno = @mrpno and company_id = @compname";
                            cmd.Parameters.AddWithValue("@mrpno", txtMRPRefNo.Text);
                            cmd.Parameters.AddWithValue("@compname", logIn.company);
                            cmd.Connection = con;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();


                            //Save Purchase Req

                            //Checke Weather PR Aleady Generated and it is approved
                            if ((from u in db.Purchase_Req_Masters where u.Ref_Doc == txtMRPRefNo.Text && u.Company_ID == logIn.company && u.Status == 3 select u).Count() > 0)
                            {
                                DialogResult PRRaised = MessageBox.Show("PR Already Generated But Not Approved, Click Yes  To Update The Same or No To Generated New PR", "Confirm PR Raising", MessageBoxButtons.YesNo);

                                if (PRRaised == DialogResult.Yes)
                                {

                                }
                            }

                            var PRNo = db.Sp_autoincrement_PurchaseReq(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                            string PR_No = PRNo.FirstOrDefault().Pr_No;

                            SqlCommand cmd3 = new SqlCommand("SAVE_PURCHAE_REQ", con);
                            cmd3.CommandType = CommandType.StoredProcedure;

                            cmd3.Parameters.AddWithValue("@prpno", PR_No);
                            cmd3.Parameters.AddWithValue("@compname", logIn.company);
                            cmd3.Parameters.AddWithValue("@mrpno", txtMRPRefNo.Text);
                            cmd3.Parameters.AddWithValue("@struser", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));
                            cmd3.Parameters.AddWithValue("@buid", logIn.BU_ID);

                            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                            if (con.State != ConnectionState.Open)
                            {
                                con.Close();
                                con.Open();
                            }
                            //  con.Open();
                            cmd3.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("MRP Generated and PR Also Raised Sucessfully");
                            clear();
                        }
                        else
                        {
                            //clear();
                        }

                    }
                    else
                    {
                        MessageBox.Show("No Data Found To Generate MRP Or MRP Already generated against selected MO Combination");
                    }                   

                }
                else
                {
                    MessageBox.Show("Select MO No To Proceed");
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

                var result = db.Sp_autoincrement_MRP(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtMRPRefNo.Text = result.FirstOrDefault().Mrp_no;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionManagement.Transactions.MRP_Search obj = new ProductionManagement.Transactions.MRP_Search();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtMRPRefNo.Text = ProductionManagement.Transactions.MRP_Search.FInname;
                    GetMethod();
                    //sumqty();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void GetMethod()
        {

            var sa = (from a in db.MRPLists
                      where a.company_id == logIn.company && a.MRPNo == txtMRPRefNo.Text
                      select new
                      {
                          a.Project,
                          a.MO_No,
                          a.MtrlGroup,
                          a.ProjModule,    
                          a.Project_qty,                     
                          a.Created_By,
                          a.Modified_By,
                          a.Project_Code,
                          a.MRP_Date
                      }).ToList();
            if (sa.Count > 0)
            {
                //cmbpname.SelectedValue = sa[0].Bom_Item_ID;
                cmbProdGroup.SelectedValue = sa[0].MtrlGroup;
                txtModule.Text = sa[0].ProjModule;
                txtProjectCode.Text = sa[0].Project_Code;
                txtProjectID.Text = sa[0].Project.ToString();
                txtMRPQty.Text = sa[0].Project_qty.ToString();
                dateTimePicker1.Text = sa[0].MRP_Date.ToString();
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_By;
                cmbMONo.Text = sa[0].MO_No;
            }
            var ca = (from sq in db.MRPs
                      where sq.Company_ID == logIn.company && sq.MRPNo == txtMRPRefNo.Text orderby sq.id
                      select new
                      {

                          SNo = sq.SNo,
                          Item_Code = sq.Prod_Code,
                          Prod_Code = sq.PartNo,
                          Item_Name = sq.Prod_Name,
                          Group_Name = sq.Category,                          
                          UOM = sq.uom,
                          QtyReq= sq.QTY,
                          sq.QTY_Avbl,
                          sq.QTY_To_Procure,
                          Remarks=sq.remarksline
                      });
            SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt1 = new DataTable();
            da3.Fill(dt1);
            if (dt1.Rows.Count > 0)
                sfDataGrid1.DataSource = dt1;
            this.sfDataGrid1.TableSummaryRows.Clear();
            //this.sfDataGrid1.GroupSummaryRows.Clear();
            this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
            this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
            this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;
            this.sfDataGrid1.Columns["Item_Name"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Item_Name"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Item_Name"].ImmediateUpdateColumnFilter = true;
            this.sfDataGrid1.Columns["Item_Name"].FilterRowCondition = FilterRowCondition.Contains;
            this.sfDataGrid1.Columns["Group_Name"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Group_Name"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Group_Name"].ImmediateUpdateColumnFilter = true;
            this.sfDataGrid1.Columns["Group_Name"].FilterRowCondition = FilterRowCondition.Contains;

            this.sfDataGrid1.TableSummaryRows.Clear();
            GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            tableSummaryRow1.Name = "TableSummary";
            tableSummaryRow1.ShowSummaryInRow = false;
            tableSummaryRow1.Position = VerticalPosition.Bottom;

            GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
            summaryColumn1.Name = "Total Products";
            summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn1.Format = "Total Products: {Count}";
            summaryColumn1.MappingName = "Prod_Code";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

            this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if ((from u in db.Purchase_Req_Masters where u.Ref_Doc == txtMRPRefNo.Text && u.Company_ID == logIn.company && u.Status !=24 select u).Count() > 0)
                {
                    DialogResult PRRaised = MessageBox.Show("PR Already Generated , Click Yes  Also To Delete The  Related PR", "Confirm PR Deletting", MessageBoxButtons.YesNo);

                    if (PRRaised == DialogResult.Yes)
                    {
                        db.sp_PurchaseReq_MRP_Delete(txtMRPRefNo.Text, logIn.company, logIn.BU_ID);
                    }
                    else
                    {
                        return;
                    }
                }

                SqlCommand cmd1 = new SqlCommand("delete  from [MRP] where MRPNo =@RepID and company_id = @compName", con);
                cmd1.Parameters.AddWithValue("@RepID", txtMRPRefNo.Text);
                cmd1.Parameters.AddWithValue("@compName", logIn.company);

                if (con.State != ConnectionState.Open)
                    con.Open();
                //con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();                
                MessageBox.Show("Record Deleted Successfully");
                clear();
            }
        }
        public void clear()
        {
            txtMRPRefNo.Text = "";
            AutoincrementId();
            dateTimePicker1.Value = DateTime.Now;
            cmbMONo.Text = "";
            cmbProdGroup.SelectedValue = -1;
            txtProjectCode.Text= "";
            // cmbSubProcess.SelectedValue = -1;
            txtProjectID.Text = "";
            txtModule.Text = "";
            txtMRPQty.Text = "";
            sfDataGrid1.DataSource = null;
            //if (dgMRPItems.Rows.Count >= 1)
            //{
            //    for (int i = 0; i < dgMRPItems.Rows.Count - 1; i++)
            //    {
            //        dgMRPItems.Rows.RemoveAt(i);
            //        i--;
            //        while (dgMRPItems.Rows.Count == 0)
            //            continue;
            //    }
            //}
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            try
            {

                
                var options = new ExcelExportingOptions();
                options.StartRowIndex = 3;
                var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                var ws = excelEngine.Excel.Worksheets[1];
                workBook.Worksheets[0].Range["A3:L100"].AutofitColumns();
                workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
                workBook.Worksheets[0].Range["A2"].Value = "MRP :" + txtMRPRefNo; 
                workBook.Worksheets[0].Range["D2"].Value = "Project :" + txtProjectCode;
                workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
                workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
                workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
                workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
                workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
                workBook.Worksheets[0].PageSetup.Zoom = 85;
                workBook.Worksheets[0].PageSetup.PrintGridlines = true;
                string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                workBook.SaveAs(Fname + "\\MRP.xlsx");
                string doc = Fname + "\\MRP.xlsx";
                Process prc = new Process();
                prc.StartInfo.FileName = doc;
                prc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgMRPItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) //Remove Rows
            {
                //if (dgMRPItems.Rows.Count > 0)
                //{

                //    foreach (DataGridViewCell oneCell in dgMRPItems.SelectedCells)
                //    {
                //        if (oneCell.Selected)
                //            dgMRPItems.Rows.RemoveAt(oneCell.RowIndex);
                //    }
                //}
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
