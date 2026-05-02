using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using ioneNet.OrderManagement;
using Newtonsoft.Json.Linq;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Tools.Win32API;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ioneNet.MaterialManagement.Transactions
{


    public partial class RequestForQuotation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo, ItemCode, OrdQty;
        public int Vcode;
        public static string filepath = "";
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        
        public string json;
        public string authToken;
        public static string Inv_NO_for_EInv;
        public static Boolean editMode;
        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbQuotNo.Text == "With PR")
                {
                    var d = (from data in db.getRQdata(logIn.company, logIn.BU_ID) select data).ToList();

                    if (d.Count > 0)
                    {
                       
                        sfDataGrid1.DataSource = d;
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["PR_NO"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["PR_NO"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["PR_NO"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["PR_NO"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Req_Date"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Req_Date"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Req_Date"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Req_Date"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Product_Description"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Product_Description"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Product_Description"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Product_Description"].FilterRowCondition = FilterRowCondition.Contains;


                    }

                    groupBox3.Visible = true;
                    //txtSearch.Focus();
                }
                else if (cmbQuotNo.Text == "Without PR")
                {
                    
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
       
        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        
        private void btnOK_Click(object sender, EventArgs e)
        {

        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        public RequestForQuotation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //groupBox1.Visible = false;
        }

        private void txtVendor_Enter(object sender, EventArgs e)
        {
            txtVendor.AutoCompleteCustomSource = null;

            AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
            addvend(DataColl);
            txtVendor.AutoCompleteCustomSource = DataColl;

        }

        private void RequestForQuotation_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            
            AutoincrementId();
            bind();

            if (/*List_RequestForQuotation.editMode == true ||*/ frmCRMDashBoard.editMode == true)
            {
                //bindedit();
                
            }
            else
            {
                //if (logIn.company == 25 || logIn.company == 1042)
                //{
                //    txtSoNo.Enabled = true;
                //}

                //else
                //{
                //    // AutoincrementId();
                //}
            }

        }

        private void txtVendor_Leave(object sender, EventArgs e)
        {
            if (txtVendor.Text != "")
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Name == txtVendor.Text select new { m.ID }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    Vcode = Buyerblind[0].ID;

                }
                else
                {
                    MessageBox.Show("Enter Valid Supplier Name");
                    txtVendor.Text = "";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow nr = new DataGridViewRow();
            nr.CreateCells(vendortable);
            nr.Cells[0].Value = Vcode.ToString();
            nr.Cells[1].Value = txtVendor.Text;
            vendortable.Rows.Add(nr);
            txtVendor.Text = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            
            dpSODate.Value = DateTime.Now;
            cmbQuotNo.SelectedIndex = -1;
            cmbPriceBasis.SelectedIndex = -1;
            cmbInsurance.SelectedIndex = -1;
            cmbPaymentTerms.SelectedIndex = -1;
            txtWarrenty.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = -1;
            txtSplInstructions.Text = "";
            txtVendor.Text = "";
            cmbStatus.SelectedIndex = -1;
            dgProducts.Rows.Clear();
            vendortable.Rows.Clear();
            AutoincrementId();
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string heads = "";
            //    int i = Reptable.CurrentCell.RowIndex;
            //    var currentCellValue = Reptable.CurrentCell.Selected;
            //    var rowData = Reptable.GetRecordAtRowIndex(i);
            //    var mappingName = Reptable.Columns[0].MappingName;
            //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


            //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

            //    if (cellVaue.ToString() != "")
            //    {

            //        var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Name ==  select new { m.ID }).Distinct().ToList();


            //        CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            //        rep = new MaterialManagement.Transactions.rptRFQ();
            //        path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation Request.pdf");
            //        FileInfo fi1 = new FileInfo(path);
            //        SqlCommand cmd = new SqlCommand("sp_Rpt_RequestForQuotationReport", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@RQ_No", txtSoNo.Text);
            //        cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
            //        cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
            //        cmd.Parameters.AddWithValue("@Vendor_Name", Buyerblind[0].ID.ToString());

            //        SqlDataAdapter da = new SqlDataAdapter(cmd);

            //        DataTable Dt = new DataTable();

            //        da.SelectCommand = cmd;
            //        da.Fill(Dt);
            //        if (Dt.Rows.Count > 0)
            //        {


            //            crConnectionInfo.ServerName = frmMain.ServerIP;
            //            crConnectionInfo.DatabaseName = frmMain.Database;
            //            crConnectionInfo.UserID = frmMain.DBUserID;
            //            crConnectionInfo.Password = frmMain.Password;


            //            crDatabase = rep.Database;
            //            crTables = crDatabase.Tables;

            //            for (int k = 0; k < crTables.Count; k++)
            //            {
            //                //  crTable = crTables[i];
            //                crTableLogOnInfo = crTables[k].LogOnInfo;
            //                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
            //                crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

            //            }
            //            rep.SetDataSource(Dt);



            //            //rep.SetParameterValue("Creation_Company", logIn.company);
            //            ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();


            //            viewer.crystalReportViewer1.ReportSource = rep;
            //            viewer.crystalReportViewer1.Refresh();
            //            rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

            //            cmd.Parameters.Clear();
            //            Process.Start(path);
            //        }
            //        con.Close();


            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);

            //}

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            //bindedit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                

                //groupBox1.Visible = true;
                //DataTable dt = new DataTable();
                //Reptable.Rows.Clear();
               
                for (int i = 0; i < vendortable.Rows.Count; i++)
                {
                    string vcode = vendortable.Rows[i].Cells[0].Value.ToString();
                    string vname = vendortable.Rows[i].Cells[1].Value.ToString();
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rep = new MaterialManagement.Transactions.rptRFQ();
                    path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation Request" + vcode + ".pdf");
                    FileInfo fi1 = new FileInfo(path);
                    SqlCommand cmd = new SqlCommand("sp_Rpt_RequestForQuotationReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RQ_No", txtSoNo.Text);
                    cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Vendor_Name", vcode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable Dt = new DataTable();

                    da.SelectCommand = cmd;
                    da.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {


                        crConnectionInfo.ServerName = frmMain.ServerIP;
                        crConnectionInfo.DatabaseName = frmMain.Database;
                        crConnectionInfo.UserID = frmMain.DBUserID;
                        crConnectionInfo.Password = frmMain.Password;


                        crDatabase = rep.Database;
                        crTables = crDatabase.Tables;

                        for (int k = 0; k < crTables.Count; k++)
                        {
                            //  crTable = crTables[i];
                            crTableLogOnInfo = crTables[k].LogOnInfo;
                            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                            crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                        }
                        rep.SetDataSource(Dt);



                        //rep.SetParameterValue("Creation_Company", logIn.company);
                        ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();


                        viewer.crystalReportViewer1.ReportSource = rep;
                        viewer.crystalReportViewer1.Refresh();
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                        cmd.Parameters.Clear();
                        //Process.Start(path);
                    }
                    GlobalVariables.doctosend = "RFQ";
                    filepath = path;
                    GlobalVariables.docRefNo = txtSoNo.Text;
                    MaterialManagement.Transactions.SendEmailDailogcs form = new MaterialManagement.Transactions.SendEmailDailogcs();
                    form.ShowDialog();

                    
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                MaterialManagement.Transactions.frmRFQList obj = new MaterialManagement.Transactions.frmRFQList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSoNo.Text = MaterialManagement.Transactions.frmRFQList.voucherNo;

                    if (!string.IsNullOrEmpty(txtSoNo.Text))
                    {
                        var dm1 = (from s in db.Req_Qutation_Childs
                                   join a in db.Req_Quation_Masters on s.M_ID equals a.ID
                                   join pr in db.Products on s.Itemcode equals pr.prod_ID
                                   join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                                   where s.RFQ_No == txtSoNo.Text && s.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       //S_No = s.ProdSno,
                                       Item_Code = s.Itemcode,
                                       Prod_code = (logIn.company == 1044 ? pr.Prod_Alternative_Code : pr.Prod_Code),
                                       Item_Description = pr.Prod_Name,
                                       Prod_Spec = "",
                                       Item_Grade = pr.Prod_Field2,
                                       UOM = u.Uom_Descr,
                                       Indent_Qty = s.Qty,
                                       PR_No = s.PR_NO.Trim(),
                                       Remarks = s.Remarks.Trim()
                                   });


                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgProducts.DataSource = dtr;


                    }
                    var dm2 = (from s in db.SP_Get_Suppliers_RFQ (txtSoNo.Text,logIn.company,logIn.BU_ID)
                               select new
                               {
                                   //S_No = s.ProdSno,
                                   Supplier_Code = s.DataItem,
                                   Supplier_Name = s.supplier_name,
                                   s.E_Mail
                                   
                               }).ToList();


                    //SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    //SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    //DataTable dtr1 = new DataTable();
                    //da3.Fill(dtr1);
                    //if (dtr1.Rows.Count >= 0)
                        vendortable.DataSource =dm2;


                    var f = (from s in db.Req_Quation_Masters where s.RFQ_No == txtSoNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Q_Date.ToString();
                        cmbQuotNo.Text = f.Basis;
                        cmbPriceBasis.Text = f.Price_Basis;
                        cmbPaymentTerms.Text = f.Payment_Terms;
                        cmbInsurance.Text = f.Insurance;
                        cmbStatus.SelectedValue = f.Status;
                        comboBox1.Text = f.Delivery_At;
                        //ObDate.Text = f.OBDate.ToString();
                        txtSplInstructions.Text = f.SPL_Inst.ToString();
                        txtWarrenty.Text = f.Warrenty.ToString();
                        lblCreatedBy.Text = f.Created_By;
                        lblModified.Text = f.Modified_By;
                    }
                    

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6 || e.KeyCode == Keys.Delete)
            {
                if (dgProducts.Rows.Count > 0)
                {

                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);

                    int j = 0;
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        j = j + 1;
                        dgProducts.Rows[i].Cells["ProdSno"].Value = j.ToString();
                    }
                }

            }
            DataTable dtexisting = new DataTable();
            if (e.KeyCode == Keys.F2)
            {
                ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                ioneNet.Masters.ProdSearch.frmName = "RFQ";
                form.ShowDialog();
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("ProdSno", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Prod_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Indent_Qty", typeof(string));
                    dtexisting.Columns.Add("PR_No", typeof(string));           
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["ProdSno"] = (dgProducts.Rows[i].Cells["S_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["S_No"].Value);
                        dr["Item_Code"] = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value);
                        dr["Prod_Code"] = (dgProducts.Rows[i].Cells["Prod_Code"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Prod_Code"].Value);
                        dr["Item_Description"] = (dgProducts.Rows[i].Cells["Item_Description"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value);
                        dr["Prod_Spec"] = (dgProducts.Rows[i].Cells["Prod_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Prod_Spec"].Value);
                        dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                        dr["UOM"] = (dgProducts.Rows[i].Cells["UOM"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["UOM"].Value);
                        dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                        dr["PR_No"] = (dgProducts.Rows[i].Cells["PR_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_No"].Value);
                        dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }






                if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    // dt.Columns.Add("ProdSno", typeof(string));
                    dt.Columns.Add("Item_Code", typeof(string));
                    dt.Columns.Add("Prod_Code", typeof(string));
                    dt.Columns.Add("Item_Description", typeof(string));                   
                    dt.Columns.Add("Item_Grade", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Indent_Qty", typeof(decimal));                  
                    dt.Columns.Add("PR_No", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));

                    //dt.Rows.Add();
                    int j = dgProducts.Rows.Count - 1;
                    for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                    {
                        string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                       
                        string prod_code = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_code"].ToString();
                        var getproducts = (from obj in db.Products
                                           join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                           where obj.prod_ID == Convert.ToInt32(prodcode)
                                           select new
                                           {

                                               // {
                                               Prod_Code = prod_code,
                                               Item_Code = obj.prod_ID,
                                               Item_Description = obj.Prod_Name,                                              
                                               Item_Grade = obj.Prod_Field2,
                                               UOM = uom.Uom_Descr,
                                               Indent_Qty = 0,
                                               PR_No = "NA",
                                               Remarks = ""
                                           }).ToList();

                        dt.Rows.Add(getproducts[0].Item_Code, getproducts[0].Prod_Code, getproducts[0].Item_Description, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Indent_Qty, getproducts[0].PR_No, getproducts[0].Remarks);

                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    dgProducts.DataSource = dtexisting;


                    //if (dgProducts.Rows.Count > 2)
                    //{
                    //    for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                    //    {
                    //        int pc = 0;
                    //        for (int k = 0; k <= dgProducts.Rows.Count - 1; k++)
                    //        {
                    //            if (dgProducts.Rows[i].Cells["Item_Code"].Value != "" && dgProducts.Rows[k].Cells["Item_Code"].Value != "")
                    //            {
                    //                if (Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value) == Convert.ToInt32(dgProducts.Rows[k].Cells["Item_Code"].Value))
                    //                {
                    //                    pc = pc + 1;
                    //                    if (pc > 1)
                    //                    {
                    //                        dgProducts.Rows.RemoveAt(dgProducts.Rows[k].Index);

                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    int l = 0;
                    //    for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                    //    {
                    //        if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                    //        {
                    //            l = l + 1;
                    //            dgProducts.Rows[m].Cells["ProdSno"].Value = l.ToString();
                    //        }
                    //    }


                    //}


                }
            }
        }

        private void vendortable_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = vendortable.CurrentCell.ColumnIndex;
                string columnName = vendortable.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "Supplier_Name")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addvend(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void vendortable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = vendortable.Rows[vendortable.CurrentRow.Index];
                int columnIndex = vendortable.CurrentCell.ColumnIndex;
                string columnName = vendortable.Columns[columnIndex].Name;

            if (columnName == "Supplier_Name" && R1.Cells["Supplier_Name"].Value != null)
            {


                var getProductName = (from s in db.Supplier_informations
                                       where s.Supplier_Name == R1.Cells["Supplier_Name"].Value.ToString() && s.Company_ID == logIn.company
                                      select new { s.ID, s.Email_Id}).ToList();


                if (getProductName.Count > 0)
                {
                    R1.Cells["Supplier_Code"].Value = getProductName[0].ID.ToString();
                    if (getProductName[0].Email_Id != null)
                    {
                        R1.Cells["E_Mail"].Value = getProductName[0].Email_Id.ToString();
                    }
                    

                }

                else
                {
                    //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    MessageBox.Show("Invalid Supplier Name");
                    R1.Cells["Supplier_Name"].Value = "";
                    return;

                }                
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            System.Data.DataRow drgetproducts;
            DataTable dtexisting = new DataTable();
            DataTable dtgetSelectedprducts = new DataTable();
            if (dgProducts.Rows.Count > 1)
            {
                dtexisting.Rows.Clear();
                dtexisting.Columns.Clear();
                dtexisting.Columns.Add("ProdSno", typeof(string));
                dtexisting.Columns.Add("Item_Code", typeof(string));
                dtexisting.Columns.Add("Prod_Code", typeof(string));
                dtexisting.Columns.Add("Item_Description", typeof(string));
                dtexisting.Columns.Add("Prod_Spec", typeof(string));
                dtexisting.Columns.Add("Item_Grade", typeof(string));
                dtexisting.Columns.Add("UOM", typeof(string));
                dtexisting.Columns.Add("Indent_Qty", typeof(string));
                dtexisting.Columns.Add("PR_No", typeof(string));
                dtexisting.Columns.Add("Remarks", typeof(string));

                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    DataRow dr;
                    dr = dtexisting.NewRow();
                    dr["ProdSno"] = (dgProducts.Rows[i].Cells["S_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["S_No"].Value);
                    dr["Item_Code"] = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value);
                    dr["Prod_Code"] = (dgProducts.Rows[i].Cells["Prod_Code"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Prod_Code"].Value);
                    dr["Item_Description"] = (dgProducts.Rows[i].Cells["Item_Description"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value);
                    dr["Prod_Spec"] = (dgProducts.Rows[i].Cells["Prod_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Prod_Spec"].Value);
                    dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                    dr["UOM"] = (dgProducts.Rows[i].Cells["UOM"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["UOM"].Value);
                    dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                    dr["PR_No"] = (dgProducts.Rows[i].Cells["PR_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_No"].Value);
                    dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);
                    dtexisting.Rows.Add(dr);

                }
                dtexisting.AcceptChanges();
            }

            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("ProdSno", typeof(string));
            dtgetproducts.Columns.Add("Item_Code", typeof(string));
            dtgetproducts.Columns.Add("Prod_Code", typeof(string));
            dtgetproducts.Columns.Add("Item_Description", typeof(string));
            dtgetproducts.Columns.Add("Prod_Spec", typeof(string));
            dtgetproducts.Columns.Add("Item_Grade", typeof(string));
            dtgetproducts.Columns.Add("UOM", typeof(string));
            dtgetproducts.Columns.Add("Indent_Qty", typeof(string));
            dtgetproducts.Columns.Add("PR_No", typeof(string));
            dtgetproducts.Columns.Add("Remarks", typeof(string));
            dtgetfinalprducts.Rows.Clear();
            //string SoNo;
            for (int i = 1; i < sfDataGrid1.RowCount; i++)
            {
                foreach (var item in sfDataGrid1.SelectedItems)
                {

                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                    var SONoCol = sfDataGrid1.Columns[0].MappingName;
                    if (rowData == item)
                    {
                        var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
                        var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());
                        var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
                        var PO_Qty = (rowData.GetType().GetProperty("PR_Qty").GetValue(rowData, null).ToString());
                        var PR_No = (rowData.GetType().GetProperty("PR_NO").GetValue(rowData, null).ToString());
                        drgetproducts = dtgetproducts.NewRow();
                        drgetproducts["Prod_Code"] = Item_Code.ToString();

                        //var getproducts = (from s in db.Products                                           
                        //                   join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                        //                   where s.prod_ID == Convert.ToInt32(prodcode)
                        //                   select new
                        //                   { 
                        //                       /*s.Prod_HSN_Code, t.Gst_Rate, s.Prod_Name, u.Uom_Descr*/
                        //                   }).ToList();

                        drgetproducts["Item_Description"] = Item_Description.ToString();
                        drgetproducts["UOM"] = UOM.ToString();
                        drgetproducts["Indent_Qty"] = PO_Qty.ToString();
                        //drgetproducts["Stock_Qty"] =0;
                        drgetproducts["PR_No"] = PR_No.ToString();
                        drgetproducts["ProdSno"] = "";

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
            
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        public void bindedit()
        {
            try
            {
                //if (MaterialManagement.Transactions.List_RequestForQuotation.SO_No != null)
                //{
                //    txtSoNo.Text = MaterialManagement.Transactions.List_RequestForQuotation.SO_No;
                //}
                //else
                //{
                //    txtSoNo.Text = frmCRMDashBoard.SO_No;
                //}
                if (txtSoNo.Text != "")
                {
                    String myString = "";
                    myString = txtSoNo.Text;
                    var da = (from obj in db.Req_Quation_Masters
                              where obj.RFQ_No == txtSoNo.Text && obj.Company_ID == logIn.company
                              select obj).ToList();

                    if (da.Count > 0)
                    {
                        txtSoNo.Text = da[0].RFQ_No.ToString();
                        dpSODate.Text = da[0].Q_Date.ToString();
                        //cmbInvType.Text = da[0].InvType;


                        //bindCustomer();
                        cmbQuotNo.Text = da[0].Basis;

                        cmbPriceBasis.Text = da[0].Price_Basis;
                        cmbInsurance.Text = da[0].Insurance;
                        cmbPaymentTerms.Text = da[0].Payment_Terms;
                        txtWarrenty.Text = da[0].Warrenty;
                        comboBox1.Text = da[0].Delivery_At;
                        dateTimePicker1.Text = da[0].Q_Reach_Date.ToString();

                        //var ven = (from obj in db.Get_suppliers_RFQ(logIn.company, logIn.BU_ID, da[0].RFQ_No.ToString()) select obj).ToList();
                        //if (ven.Count>0)
                        //{
                        //    for (int j = 0; j < ven.Count; j++)
                        //    {

                        //        string m = ven[j].Supplier_Name;

                        //        multiSelectionComboBox1.AddVisualItem(m);

                        //        //   multiSelectionComboBox1.
                        //        //multiSelectionComboBox1.Text = m;

                        //    }

                        //}


                        txtSplInstructions.Text = da[0].SPL_Inst;
                        cmbStatus.SelectedValue = da[0].Status;

                        var dm1 = (from s in db.Req_Qutation_Childs
                                   join u in db.Products on s.Itemcode equals u.prod_ID
                                   where s.RFQ_No == txtSoNo.Text && s.Company_ID == logIn.company
                                   select new

                                   {

                                       Item_Description = u.Prod_Name.Trim(),
                                       Item_Code = s.Itemcode,
                                       Item_spec = s.length_size.Trim(),
                                       Item_Grade = s.model_grade.Trim(),
                                       UOM = s.UOM.Trim(),
                                       Qty = s.Qty,
                                       PR_No = s.PR_NO,
                                       Remarks = s.Remarks




                                   });





                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                        {
                            dgProducts.DataSource = dtr;

                        }

                    }

                    else
                    {
                        MessageBox.Show("Enter Valid REQ NO");
                    }

                }
                else
                {
                    MessageBox.Show("REQ NO Cannot be empty");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bind()
        {
            try
            {


                //var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                //if (Buyerblind.Count > 0)
                //{

                //    multiSelectionComboBox1.DataSource = Buyerblind;
                //    multiSelectionComboBox1.ValueMember = "ID";
                //    multiSelectionComboBox1.DisplayMember = "Supplier_Name";



                //}

                var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PBasis.Count > 0)
                {
                    cmbPriceBasis.DataSource = PBasis;
                    cmbPriceBasis.ValueMember = "ID";
                    cmbPriceBasis.DisplayMember = "Descr";
                }

                var PBasis1 = (from m in db.Costing_Units where m.Company == logIn.company select new { m.id,m.BU_Name }).Distinct().ToList();
                if (PBasis1.Count > 0)
                {
                    comboBox1.DataSource = PBasis1;
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "BU_Name";
                }

                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                }
                var pIns = (from m in db.Attributes_Datas where m.Head_Name == "Insurance" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbInsurance.DataSource = pIns;
                    cmbInsurance.ValueMember = "ID";
                    cmbInsurance.DisplayMember = "Descr";
                }
                //if (cmbQuotNo.Items.Count > 0)
                
                cmbPriceBasis.SelectedIndex = -1;
                cmbPaymentTerms.SelectedIndex = -1;
                cmbInsurance.SelectedIndex = -1;
                comboBox1.SelectedIndex = -1;
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                cmbStatus.SelectedIndex = -1;
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
                var getSufix = (from m in db.Financial_Year_Masters where m.Company_ID == logIn.company && m.Start_Date == logIn.fy_Start_Date select new { m.Uses_AsSufix }).Distinct().ToList();
                if (getSufix.Count > 0)
                {

                    var result = db.Sp_autoincrement_req_Quotation(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtSoNo.Text = result.FirstOrDefault().RFQ_No;

                }


                //txtSoNo.Enabled = true;
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
                //Check Invoice Qty >0
                Boolean recval = false;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    if (dgProducts.Rows[i].Cells["Item_Code"].Value != null)
                    {
                        double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                        if (amt > 0)
                        {
                            recval = true;
                        }
                        else
                        {
                            recval = false;
                        }

                    }
                }

                


                if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }

                else
                {
                    SaveNew_Sql_proc();
                    // Save();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;

                if (R1.Cells["Item_Description"].Value != null)
                {

                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Field2, }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        //R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_Grade"].Value = getProductName.Prod_Field2.ToString();
                    }

                    else
                    {
                        R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                        

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        public void addvend(AutoCompleteStringCollection coll)
        {
            try
            {
                //var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                //if (Buyerblind.Count > 0)
                //{
                //    DataTable dt = new DataTable();


                //    dt.Columns.Add("Supplier_Name");
                //    foreach (var item in Buyerblind)
                //    {
                //        dt.Rows.Add(item.Supplier_Name);
                //    }
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        coll.Add(dt.Rows[i][0].ToString());
                //    }
                //}
                DataGridViewRow R1 = vendortable.Rows[vendortable.CurrentRow.Index];

                int columnIndex = vendortable.CurrentCell.ColumnIndex;
                string columnName = vendortable.Columns[columnIndex].HeaderText;


                if (columnName == "Supplier_Name")
                {
                    var Prodname = (from d in db.Supplier_informations where d.Company_ID == logIn.company select new { d.Supplier_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Supplier_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Supplier_Name);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
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
                    var Prodname = (from d in db.UoM_Masters where d.Company_ID == logIn.company select new { d.Uom_Descr }).ToList();
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



                DataGridViewRow R2 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex1 = dgProducts.CurrentCell.ColumnIndex;
                string columnName1 = dgProducts.Columns[columnIndex1].HeaderText;
                if (columnName1 == "Item Description")
                {
                    var Prodname = (from a in db.Products
                                    join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                    join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
                                    where a.Company_ID == logIn.company /*&& a.Prod_Type_Id == 140*/
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
                int columnIndex1 = dgProducts.CurrentCell.ColumnIndex;
                string columnName1 = dgProducts.Columns[columnIndex1].HeaderText;
                TextBox tb1 = e.Control as TextBox;
                tb1.AutoCompleteCustomSource = null;
                if (tb1 != null && columnName1 == "Item Description")
                {
                    tb1.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb1.AutoCompleteCustomSource = DataColl;

                }

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_rpt_Click(object sender, EventArgs e)

        {
            try
            {
                //groupBox1.Visible = true;
                //DataTable dt = new DataTable();
                //Reptable.Rows.Clear();
                for (int i = 0; i < vendortable.Rows.Count; i++)
                {
                    string vcode = vendortable.Rows[i].Cells[0].Value.ToString();
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    rep = new MaterialManagement.Transactions.rptRFQ();
                    path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation Request" + vcode + ".pdf");
                    FileInfo fi1 = new FileInfo(path);
                    SqlCommand cmd = new SqlCommand("sp_Rpt_RequestForQuotationReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RQ_No", txtSoNo.Text);
                    cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Vendor_Name", vcode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable Dt = new DataTable();

                    da.SelectCommand = cmd;
                    da.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {


                        crConnectionInfo.ServerName = frmMain.ServerIP;
                        crConnectionInfo.DatabaseName = frmMain.Database;
                        crConnectionInfo.UserID = frmMain.DBUserID;
                        crConnectionInfo.Password = frmMain.Password;


                        crDatabase = rep.Database;
                        crTables = crDatabase.Tables;

                        for (int k = 0; k < crTables.Count; k++)
                        {
                            //  crTable = crTables[i];
                            crTableLogOnInfo = crTables[k].LogOnInfo;
                            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                            crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                        }
                        rep.SetDataSource(Dt);



                        //rep.SetParameterValue("Creation_Company", logIn.company);
                        ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();


                        viewer.crystalReportViewer1.ReportSource = rep;
                        viewer.crystalReportViewer1.Refresh();
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        cmd.Parameters.Clear();
                        Process.Start(path);
                    }
                }                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

        }

        public void SaveNew_Sql_proc()
        {
            try
            {


                string s = "";
                SqlCommand cmd = new SqlCommand("SaveReqQuotation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RFQ_No", txtSoNo.Text);
                cmd.Parameters.AddWithValue("@Q_Date", dpSODate.Value);
                cmd.Parameters.AddWithValue("@Basis", cmbQuotNo.Text);
                cmd.Parameters.AddWithValue("@Price_Basis", cmbPriceBasis.Text);  
                cmd.Parameters.AddWithValue("@Insurance", cmbInsurance.Text);
                cmd.Parameters.AddWithValue("@Payment_Terms", cmbPaymentTerms.Text);                   
                cmd.Parameters.AddWithValue("@Warrenty", (txtWarrenty.Text == "") ? "" : txtWarrenty.Text);
                cmd.Parameters.AddWithValue("@Q_Reach_Date", dateTimePicker1 .Value );
                cmd.Parameters.AddWithValue("@Delivery_At", comboBox1.Text);
               
                cmd.Parameters.AddWithValue("@SPL_Inst", txtSplInstructions.Text);

                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);

                string heads = "";
                for (int i = 0; i < vendortable.RowCount - 1; i++)
                {
                    if (heads != "")
                    {
                        heads = heads + "," + Convert.ToString(vendortable.Rows[i].Cells["Supplier_Code"].Value).Trim(); 
                    }
                    else
                    {
                        heads = Convert.ToString(vendortable.Rows[i].Cells["Supplier_Code"].Value).Trim(); 
                    }
                }
                //foreach (VisualItem obj in this.multiSelectionComboBox1.VisualItems)
                //{
                //    var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Name==obj.Text  select new { m.ID}).Distinct().ToList();
                //    if (Buyerblind.Count>0) 
                //    {
                //        if (heads != "")
                //        {
                //            heads = heads + "," + Buyerblind[0].ID;
                //        }
                //        else
                //        {
                //            heads = Buyerblind[0].ID.ToString();
                //        }
                //    }
                //}


                cmd.Parameters.AddWithValue("@Vendor_Name", heads);
               

                string Prod_Code = "";
               
                string Prod_Grade = "";
                string Prod_length = "";
                string Uom = "";
              
               
                string Qty = "";
                
                string Remarks = "";
                string ProdRno = "";
                
                int rowcount = 0;

                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {

                   
                    Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).Trim().PadRight(250);
                    Prod_length = Prod_length + Convert.ToString(dgProducts.Rows[i].Cells["Prod_Code"].Value).Trim().PadRight(250);
                    Prod_Grade = Prod_Grade + Convert.ToString(dgProducts.Rows[i].Cells["Item_Grade"].Value).PadRight(14);
                    Uom = Uom + Convert.ToString(dgProducts.Rows[i].Cells["UOM"].Value).Trim().PadRight(14);

                    Qty = Qty + Convert.ToString(dgProducts.Rows[i].Cells["Indent_Qty"].Value).PadRight(14);
                    ProdRno = ProdRno + Convert.ToString(dgProducts.Rows[i].Cells["PR_No"].Value).PadRight(14);
                    Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).Trim().PadRight(14);
                    

                 
                    rowcount += 1;
                }

               
                cmd.Parameters.AddWithValue("@txt_Item_Name", Prod_Code);
                cmd.Parameters.AddWithValue("@txt_length_size", Prod_length);
                cmd.Parameters.AddWithValue("@txt_model_grade", Prod_Grade);
                cmd.Parameters.AddWithValue("@txt_UOM", Uom);
                cmd.Parameters.AddWithValue("@txt_Qty", Qty );
                cmd.Parameters.AddWithValue("@txt_PR_NO", ProdRno);
                
                cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);
                cmd.Parameters.AddWithValue("@txt_CompanyID", logIn.company);
                cmd.Parameters.AddWithValue("@gridcount", rowcount);

                try
                {
                    con.Close();
                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(result))
                    {
                        MessageBox.Show("Record has been successfully Saved/Updated with RFQ No :" + txtSoNo.Text);
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
        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

    }
}
