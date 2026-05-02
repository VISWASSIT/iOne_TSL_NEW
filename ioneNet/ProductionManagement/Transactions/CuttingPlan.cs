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
using System.Data.OleDb;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class cuttingPlan : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public cuttingPlan()
        {
            InitializeComponent();
        }
        private void cuttingPlan_Load(object sender, EventArgs e)
        {
            try
            {
                var pCode = (from m in db.Project_code_Masters where m.Company_ID == logIn.company select new { m.id, m.Project_Code }).Distinct().ToList();
                if (pCode.Count > 0)
                {
                    cmbProjectCode.DataSource = pCode;
                    cmbProjectCode.ValueMember = "id";
                    cmbProjectCode.DisplayMember = "Project_Code";

                }
                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                if (CuttingPlansList.editMode==true)
                {
                    EditData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       

        private void dgrmconsumed_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "Cplan";
                    form.ShowDialog();
                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();
                        dtexisting.Columns.Add("Item_Code", typeof(string));
                        dtexisting.Columns.Add("Item_Description", typeof(string));
                        dtexisting.Columns.Add("Mark_No", typeof(string));
                        dtexisting.Columns.Add("Prod_Width", typeof(string));
                        dtexisting.Columns.Add("Prod_Length", typeof(string));
                        dtexisting.Columns.Add("Qty_Nos", typeof(decimal));
                        dtexisting.Columns.Add("Unit_Wt", typeof(decimal));
                        dtexisting.Columns.Add("Qty_Per_PC", typeof(decimal));
                        dtexisting.Columns.Add("Qty_wt", typeof(decimal));
                        dtexisting.Columns.Add("Remarks", typeof(string));

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                            dr["Mark_No"] = dgProducts.Rows[i].Cells["Mark_No"].Value.ToString();
                            dr["Prod_Width"] = dgProducts.Rows[i].Cells["Prod_Width"].Value.ToString();
                            dr["Prod_Length"] = dgProducts.Rows[i].Cells["Prod_Length"].Value.ToString();
                            dr["Qty_Nos"] = dgProducts.Rows[i].Cells["Qty_Nos"].Value.ToString();
                            dr["Unit_Wt"] = dgProducts.Rows[i].Cells["Unit_Wt"].Value.ToString();
                            dr["Qty_Per_PC"] = dgProducts.Rows[i].Cells["Qty_Per_PC"].Value.ToString();
                            dr["Qty_wt"] = dgProducts.Rows[i].Cells["Qty_wt"].Value.ToString();
                            dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }

                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Item_Code", typeof(string));
                        dt.Columns.Add("Item_Description", typeof(string));
                        dt.Columns.Add("Mark_No", typeof(string));
                        dt.Columns.Add("Prod_Width", typeof(string));
                        dt.Columns.Add("Prod_Length", typeof(string));
                        dt.Columns.Add("Qty_Nos", typeof(decimal));
                        dt.Columns.Add("Unit_Wt", typeof(decimal));
                        dt.Columns.Add("Qty_Per_PC", typeof(decimal));
                        dt.Columns.Add("Qty_wt", typeof(decimal));
                        dt.Columns.Add("Remarks", typeof(string));

                        //dt.Rows.Add();
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               join tm in db.Tax_Class_Masters on obj.Prod_Tax_Class equals tm.ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {
                                                   Item_Code = obj.prod_ID,
                                                   Item_Description = obj.Prod_Alias_Name,
                                                   Mark_No = "",
                                                   Prod_Width = "",
                                                   Prod_Length = "",
                                                   Qty_Nos = 0,
                                                   Unit_Wt = obj.Prod_Unit_Wt,
                                                   Qty_Per_PC = 0,
                                                   Qty_wt = 0,
                                                   Remarks = ""
                                               }).ToList();
                            dt.Rows.Add(getproducts[0].Item_Code, getproducts[0].Item_Description, getproducts[0].Mark_No, getproducts[0].Prod_Width, getproducts[0].Prod_Length, getproducts[0].Qty_Nos, getproducts[0].Unit_Wt, getproducts[0].Qty_Per_PC, getproducts[0].Qty_wt, getproducts[0].Remarks);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;
                    }
                }
                if (e.KeyCode == Keys.F6)
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        decimal x = 0;
                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += ( dgProducts.Rows[i].Cells["Qty_wt"].Value == "" || dgProducts.Rows[i].Cells["Qty_wt"].Value == null || dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                    }
                }
            }
              
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if (columnName == "Prod_Width" && R1.Cells["Prod_Width"].Value != null)
                {

                                                           
                        
                    
                    decimal Width = (R1.Cells["Prod_Width"].Value == "" || R1.Cells["Prod_Width"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Prod_Width"].Value);
                    decimal unitWt = (R1.Cells["Unit_Wt"].Value == "" || R1.Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Unit_Wt"].Value);
                    
                }
                if (columnName == "Prod_Length" && R1.Cells["Prod_Length"].Value != null)
                {
                    decimal b, c, d;                    
                    decimal Width = (R1.Cells["Prod_Width"].Value == "" || R1.Cells["Prod_Width"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Prod_Width"].Value);
                    decimal Length = (R1.Cells["Prod_Length"].Value == "" || R1.Cells["Prod_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Prod_Length"].Value);

                    decimal unitWt = (R1.Cells["Unit_Wt"].Value == "" || R1.Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Unit_Wt"].Value);
                    if(Width>0)
                    {
                        b = Width / 1000 * Length / 1000 * unitWt;
                        
                    }
                    else
                    {
                        b = Length / 1000 * unitWt;
                        
                    }
                    R1.Cells["Qty_Per_PC"].Value = b.ToString("0.00");
                    c = (R1.Cells["Qty_Nos"].Value == "" || R1.Cells["Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Nos"].Value);
                    d = b * c;
                    R1.Cells["Qty_wt"].Value = d.ToString("0.00");
                }
                if (columnName == "Qty_Nos" && R1.Cells["Qty_Nos"].Value != null)
                {
                    decimal b, c, d;                    
                    decimal Width = (R1.Cells["Prod_Width"].Value == "" || R1.Cells["Prod_Width"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Prod_Width"].Value);
                    decimal Length = (R1.Cells["Prod_Length"].Value == "" || R1.Cells["Prod_Length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Prod_Length"].Value);

                    decimal unitWt = (R1.Cells["Unit_Wt"].Value == "" || R1.Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Unit_Wt"].Value);
                    if (Width > 0)
                    {
                        b = Width / 1000 * Length / 1000 * unitWt;

                    }
                    else
                    {
                        b = Length / 1000 * unitWt;

                    }
                    R1.Cells["Qty_Per_PC"].Value = b.ToString("0.00");
                    c = (R1.Cells["Qty_Nos"].Value == "" || R1.Cells["Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Nos"].Value);
                    d = b * c;
                    R1.Cells["Qty_wt"].Value = d.ToString("0.00");
                }
                decimal x = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {

                    x += (dgProducts.Rows[i].Cells["Qty_wt"].Value == "" || dgProducts.Rows[i].Cells["Qty_wt"].Value == null || dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);
                 
                }

                txtTotalQty.Text = x.ToString(".00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProjectCode.Text == "")
                {
                    MessageBox.Show("Cutting Plan Cannot Be Saved Without Proejct Code...!");
                    cmbProjectCode.Focus();
                    return;


                }
                else
                if (txtCPNo.Text == "")
                {
                    MessageBox.Show("Please Enter the CP No...!");
                    txtCPNo.Focus();
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
        private void Save()
        {
            try
            {
                if ((from a in db.Cuttingplans where a.Company_ID == logIn.company && a.CP_No == txtCPNo.Text select a).Count() > 0)
                {
                    db.Sp_delete_CuttingPlan(logIn.company, txtCPNo.Text);
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        Cuttingplan bo1 = new Cuttingplan();
                        bo1.CP_No = txtCPNo.Text;                       
                        bo1.CP_Date = dpdate.Value;
                        bo1.Project_ID = Convert.ToInt32(cmbProjectCode.SelectedValue);
                    
                        bo1.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                       
                        bo1.Product_Name = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        bo1.Mark_No = dgProducts.Rows[i].Cells["Mark_No"].Value.ToString();
                        bo1.i_Width = dgProducts.Rows[i].Cells["Prod_Width"].Value.ToString();
                        bo1.i_Length = dgProducts.Rows[i].Cells["Prod_Length"].Value.ToString();
                        bo1.Qty_nos = (dgProducts.Rows[i].Cells["Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Nos"].Value);
                        bo1.Unit_Wt = (dgProducts.Rows[i].Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Unit_Wt"].Value);
                        bo1.Wt_per_Pc = (dgProducts.Rows[i].Cells["Qty_Per_PC"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Per_PC"].Value);
                        bo1.Total_Wt = (dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);

                        bo1.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                        bo1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        bo1.Company_ID = logIn.company;
                        bo1.Created_By = lblCreatedBy.Text;
                        bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.Cuttingplans.InsertOnSubmit(bo1);
                    }

                    db.SubmitChanges();
                    MessageBox.Show("Recored Updated Succesfully");
                    //clear();
                    return;
                }
                else
                {
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        Cuttingplan bo1 = new Cuttingplan();
                        bo1.CP_No = txtCPNo.Text;
                        bo1.CP_Date = dpdate.Value;
                        bo1.Project_ID = Convert.ToInt32(cmbProjectCode.SelectedValue);

                        bo1.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);

                        bo1.Product_Name = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        bo1.Mark_No = dgProducts.Rows[i].Cells["Mark_No"].Value.ToString();
                        bo1.i_Width = dgProducts.Rows[i].Cells["Prod_Width"].Value.ToString();
                        bo1.i_Length = dgProducts.Rows[i].Cells["Prod_Length"].Value.ToString();
                        bo1.Qty_nos = (dgProducts.Rows[i].Cells["Qty_Nos"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Nos"].Value);
                        bo1.Unit_Wt = (dgProducts.Rows[i].Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Unit_Wt"].Value);
                        bo1.Wt_per_Pc = (dgProducts.Rows[i].Cells["Qty_Per_PC"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Per_PC"].Value);
                        bo1.Total_Wt = (dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);

                        bo1.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                        bo1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        bo1.Company_ID = logIn.company;
                        bo1.Created_By = lblCreatedBy.Text;
                        bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.Cuttingplans.InsertOnSubmit(bo1);
                    }


                    db.SubmitChanges();
                    MessageBox.Show("Recored Saved Succesfully");
                   
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void EditData()
        {
            txtCPNo.Text = CuttingPlansList.SO_No;
            var sa = (from a in db.Cuttingplans
                      where a.Company_ID == logIn.company && a.CP_No == txtCPNo.Text
                      select new
                      {
                          a.CP_Date,
                          a.Project_ID,                         
                          a.Status,
                          a.Created_By,
                          a.Modified_By,
                      }).ToList();
            if (sa.Count > 0)
            {
                //cmbpname.SelectedValue = sa[0].Bom_Item_ID;
                dpdate.Text = sa[0].CP_Date.ToString();
                cmbProjectCode.SelectedValue = sa[0].Project_ID;             
                
                if (sa[0].Status != null)
                {
                    cmbStatus.SelectedValue = (sa[0].Status);
                }
                //cmbStatus.SelectedValue = sa[0].Status;
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_By;
            }
            var ca = (from sq in db.Cuttingplans
                      where sq.Company_ID == logIn.company && sq.CP_No == txtCPNo.Text
                      select new
                      {
                          Item_Code = sq.Prod_Code,
                          Item_Description = sq.Product_Name,
                          sq.Mark_No,
                          Prod_Width= sq.i_Width,
                          Prod_Length = sq.i_Length,
                          Qty_Nos =sq.Qty_nos,
                          Unit_Wt = sq.Unit_Wt,
                          Qty_Per_PC = sq.Wt_per_Pc,
                          Qty_wt = sq.Total_Wt,
                          sq.Remarks
                      });
            SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt1 = new DataTable();
            da3.Fill(dt1);
            if (dt1.Rows.Count > 0)
                dgProducts.DataSource = dt1;

            decimal x = 0;
            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
            {

                x += (dgProducts.Rows[i].Cells["Qty_wt"].Value == "" || dgProducts.Rows[i].Cells["Qty_wt"].Value == null || dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);

            }

            txtTotalQty.Text = x.ToString(".00");
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                fdlg.FileName = txtChooseFile.Text;
                fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    txtChooseFile.Text = fdlg.FileName;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (txtChooseFile.Text == "")
                {
                    MessageBox.Show("Please select the File to Import");
                    txtChooseFile.Focus();
                }
                else 
                {
                    Cursor.Current = Cursors.WaitCursor;
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.DataTable DtSet;
                    System.Data.OleDb.OleDbDataAdapter MyCommand;

                    string filename = txtChooseFile.Text;
                    // string ExcellSheet = ;

                    string str = "Provider = Microsoft.jet.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                    MyConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtChooseFile.Text + ";Extended Properties='Excel 8.0;HDR=Yes'");
                    string sheetname = "COMBINED";
                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + sheetname + "$]", MyConnection);
                    //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
                    MyCommand.TableMappings.Add("Table", filename);
                    DtSet = new System.Data.DataTable();
                    MyCommand.Fill(DtSet);

                    int count =DtSet.Rows.Count;
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add(new DataColumn("Item_Code", typeof(string)));
                    dt.Columns.Add(new DataColumn("Item_Description", typeof(string)));
                    dt.Columns.Add(new DataColumn("Mark_No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Prod_Width", typeof(string)));
                    dt.Columns.Add(new DataColumn("Prod_Length", typeof(string)));
                    dt.Columns.Add(new DataColumn("Qty_Nos", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("Unit_Wt", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("Qty_Per_PC", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("Qty_wt", typeof(decimal)));
                    dt.Columns.Add(new DataColumn("Remarks", typeof(string)));

                    
                    for (int i = 0; i < count; i++)
                    {
                        if (DtSet.Rows[i][0].ToString() != "End")
                        {
                            if (DtSet.Rows[i][0].ToString() != "")
                            {
                                dr = dt.NewRow();
                                dr["Item_Code"] = "";
                                dr["Item_Description"] = DtSet.Rows[i][2].ToString();
                                dr["Mark_No"] = DtSet.Rows[i][1].ToString();
                                dr["Prod_Width"] = DtSet.Rows[i][3].ToString();
                                dr["Prod_Length"] = DtSet.Rows[i][4].ToString();
                                dr["Qty_Nos"] = DtSet.Rows[i][5].ToString();
                                dr["Unit_Wt"] = DtSet.Rows[i][6].ToString();
                                dr["Qty_Per_PC"] = DtSet.Rows[i][7].ToString();
                                dr["Qty_wt"] = DtSet.Rows[i][8].ToString();
                                dr["Remarks"] = DtSet.Rows[i][9].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    dgProducts.DataSource = dt;
                    MyConnection.Close();
                    Cursor.Current = Cursors.Default;
                    //Get Product Code
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        
                        if (dgProducts.Rows[i].Cells["Item_Description"].Value != null)
                        {
                            var s = (from d in db.Products where d.Prod_Alias_Name == dgProducts.Rows[i].Cells["Item_Description"].Value.ToString() && d.Company_ID == logIn.company select d).ToList();
                            if (s.Count > 0)
                            {
                                dgProducts.Rows[i].Cells["Item_Code"].Value = s[0].prod_ID;
                                //dgProductData.Rows[i].Cells["MAC"].Value = s[0].MoldAmortisationCost;
                            }
                            else
                            {
                               // MessageBox.Show("Some Items Data Importing Not Matching With Master Data Available");

                            }
                        }
                    }
                        
                    
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are You Sure Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    db.Sp_delete_CuttingPlan(logIn.company, txtCPNo.Text);
                    MessageBox.Show("Recored Deleted Successfully");
                    clear();
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            txtCPNo.Text = "";
            dpdate.Value = DateTime.Now;
            cmbProjectCode.Text = "";
            txtTotalQty.Text = "";
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

            if (dgProducts.Rows.Count >= 1)
            {
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    dgProducts.Rows.RemoveAt(i);
                    i--;
                    while (dgProducts.Rows.Count == 0)
                        continue;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
