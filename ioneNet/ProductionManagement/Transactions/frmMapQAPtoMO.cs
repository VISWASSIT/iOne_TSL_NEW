using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmMapQAPtoMO : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string Enq_NO, Item_Shape, MtrlGrade, ItemCode;
        public frmMapQAPtoMO()
        {
            InitializeComponent();
        }

        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //bindCustomer();
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }


            var da = (from obj in db.TestMasters
                      where obj.Company_ID == logIn.company
                      select new
                      { 
                          Characterstics = obj.TestName });

            SqlCommand cmd2 = (SqlCommand)db.GetCommand(da);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count >= 0)
                dataGridView1.DataSource = dtr;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = row.Index + 1;
            }

            //GetPendingSO();

            //if (frmForge_MO_List.editMode == true)
            //{
                bindedit();
            //}
        }
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                CmbBuyerName.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void GetPendingSO()
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("SP_Orders_ToRaise_MO", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);


                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                for (int i = 0; i < ds2.Rows.Count; i++)
                {
                    cmbQuotNo.Items.Add(ds2.Rows[i]["SO_NO"].ToString());
                }
                //var d = (from data in db.SP_GetQuotesForSaleOrder(logIn.company) select new { data.Quot_NO }).Distinct().ToList();

                //if (d.Count > 0)
                ////    var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                ////if (Buyerblind.Count > 0)
                ////{
                //    cmbQuotNo.DataSource = d;
                //    CmbBuyerName.ValueMember = "ID";
                //    CmbBuyerName.DisplayMember = "Supplier_Name";

                //}
                ////if (CmbBuyerName.Items.Count > 0)
                cmbQuotNo.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbQuotNo_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                int MO_Master_ID = 0;
                if (cmbQuotNo.Text != "NA" && cmbQuotNo.Text != "")
                {
                    var da = (from obj in db.Forge_MFG_Order_Masters
                              where obj.MO_No == txtSoNo.Text && obj.Company_ID == logIn.company
                              select obj).ToList();

                    if (da.Count > 0)
                    {
                        MO_Master_ID = da[0].id;
                        var da1 = (from obj in db.Forge_Mfg_Order_Childs
                                   where obj.MO_Master_ID == MO_Master_ID && obj.Company_ID == logIn.company && obj.Mo_S_No == Convert.ToInt32(cmbQuotNo.Text)
                                   select obj).ToList();
                        if (da1.Count > 0)
                        {
                            CmbBuyerName.Text = da1[0].Product_Description;
                            txtCustPoNo.Text = da1[0].Prod_Grade;
                            txtDrawingNo.Text = da1[0].Drawing_No;
                            txtForgingSize.Text = da1[0].Forging_Size_Prod;
                           // txtProofSize.Text = da1[0].Drawing_No;

                        }
                    }


                    SqlCommand cmd2 = new SqlCommand("Get_MOSpec_MO", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@mono", MO_Master_ID);
                    cmd2.Parameters.AddWithValue("@itemno", Convert.ToInt32(cmbQuotNo.Text));
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    if (ds2.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = ds2;
                    }



                    //var dm2 = (from s in db.Forge_Mfg_Order_Specs
                    //           where s.MO_Master_ID == MO_Master_ID && s.Company_ID == logIn.company && s.S_No == Convert.ToInt32(cmbQuotNo.Text)

                    //           select new

                    //           {
                    //               Characterstics = s.Characterstics,
                    //               Spec = s.Specification,
                    //               Acceptance_Criteria = s.Acceptance_Criteria,
                    //               s.NABL,
                    //               Internal = s.INT,
                    //               s.SEP
                    //           });




                    //SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    //SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    //DataTable dtr1 = new DataTable();
                    //da3.Fill(dtr1);
                    //if (dtr1.Rows.Count > 0)
                    //{
                    //    dataGridView1.DataSource = dtr1;
                    //}

                    //else
                    //{
                    //    var t1 = (from obj in db.TestMasters
                    //              where obj.Company_ID == logIn.company
                    //              select new
                    //              { Characterstics = obj.TestName,
                    //                  Spec = "",
                    //                  Acceptance_Criteria ="",
                    //                  NABL ="",
                    //                  Internal = "",
                    //                  SEP =""
                    //              });

                    //    SqlCommand cmd2 = (SqlCommand)db.GetCommand(t1);
                    //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //    DataTable dtr = new DataTable();
                    //    da2.Fill(dtr);
                    //    if (dtr.Rows.Count >= 0)
                    //        dataGridView1.DataSource = dtr;

                    //    foreach (DataGridViewRow row in dataGridView1.Rows)
                    //    {
                    //        row.HeaderCell.Value = row.Index + 1;
                    //    }
                    //}
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (cmbQuotNo.Text == string.Empty)
                {
                    MessageBox.Show("Select Item No  To Proceed", "MO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbQuotNo.Focus();
                    return;
                }              
               
                                   
                
                else
               
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                int MO_Master_ID = 0;                

                var da = (from obj in db.Forge_MFG_Order_Masters
                          where obj.MO_No == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    MO_Master_ID = da[0].id;
                }

                 if ((from u in db.Forge_Mfg_Order_Specs where u.MO_Master_ID == MO_Master_ID && u.Company_ID == logIn.company && u.S_No == Convert.ToInt32(cmbQuotNo.Text) select u).Count() > 0)
                {
                    SqlCommand cmd1 = new SqlCommand("delete  from [Forge_Mfg_Order_Specs] where MO_Master_ID =@ProdID and S_No = @sno", con);
                    cmd1.Parameters.AddWithValue("@ProdID", MO_Master_ID);
                    cmd1.Parameters.AddWithValue("@sno", Convert.ToInt32(cmbQuotNo.Text));

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    //con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Spec SC = new Forge_Mfg_Order_Spec();
                        //var d3 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == txtSoNo.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = MO_Master_ID;
                        SC.S_No = Convert.ToInt32(cmbQuotNo.Text);
                        SC.Characterstics = (dataGridView1.Rows[i].Cells["Characterstics"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Characterstics"].Value).ToString();
                        SC.Specification = (dataGridView1.Rows[i].Cells["Spec"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Spec"].Value).ToString();
                        SC.Acceptance_Criteria = (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value).ToString();

                        if (dataGridView1.Rows[i].Cells["NABL"].Value == null || dataGridView1.Rows[i].Cells["NABL"].Value == DBNull.Value)
                        {
                            SC.NABL = false;
                        }
                        else
                        {
                            SC.NABL = Convert.ToBoolean(dataGridView1.Rows[i].Cells["NABL"].Value);
                        }

                        if (dataGridView1.Rows[i].Cells["Internal"].Value == null || dataGridView1.Rows[i].Cells["Internal"].Value == DBNull.Value)
                        {
                            SC.Int_Insp = false;
                        }
                        else
                        {
                            SC.Int_Insp = Convert.ToBoolean(dataGridView1.Rows[i].Cells["Internal"].Value);
                        }
                        if (dataGridView1.Rows[i].Cells["SEP"].Value == null || dataGridView1.Rows[i].Cells["SEP"].Value == DBNull.Value)
                        {
                            SC.SEP = false;
                        }
                        else
                        {
                            SC.SEP = Convert.ToBoolean(dataGridView1.Rows[i].Cells["SEP"].Value);
                        }
                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Specs.InsertOnSubmit(SC);
                    }
                    //transaction.Commit(); 
                    db.SubmitChanges();



                }
                else
                {
                    //AutoincrementId();

                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Spec SC = new Forge_Mfg_Order_Spec();
                        //var d1 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == txtSoNo.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = MO_Master_ID;
                        SC.S_No = Convert.ToInt32(cmbQuotNo.Text);
                        SC.Characterstics = (dataGridView1.Rows[i].Cells["Characterstics"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Characterstics"].Value).ToString();
                        SC.Specification = (dataGridView1.Rows[i].Cells["Spec"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Spec"].Value).ToString();

                        SC.Acceptance_Criteria = (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value).ToString();
                        if (dataGridView1.Rows[i].Cells["NABL"].Value == null || dataGridView1.Rows[i].Cells["NABL"].Value == DBNull.Value)
                        {
                            SC.NABL = false;
                        }
                        else
                        {
                            SC.NABL = Convert.ToBoolean(dataGridView1.Rows[i].Cells["NABL"].Value);
                        }

                        if (dataGridView1.Rows[i].Cells["Internal"].Value == null || dataGridView1.Rows[i].Cells["Internal"].Value == DBNull.Value)
                        {
                            SC.Int_Insp = false;
                        }
                        else
                        {
                            SC.Int_Insp = Convert.ToBoolean(dataGridView1.Rows[i].Cells["Internal"].Value);
                        }
                        if (dataGridView1.Rows[i].Cells["SEP"].Value == null || dataGridView1.Rows[i].Cells["SEP"].Value == DBNull.Value)
                        {
                            SC.SEP = false;
                        }
                        else
                        {
                            SC.SEP = Convert.ToBoolean(dataGridView1.Rows[i].Cells["SEP"].Value);
                        }
                        //SC.SEP = Convert.ToBoolean(dataGridView1.Rows[i].Cells["SEP"].Value);
                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Specs.InsertOnSubmit(SC);
                    }
                    //transaction.Commit(); 
                    db.SubmitChanges();
                }
                
                //db.Transaction = transaction;
                
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public void bindedit()
        {
            try
            {
                txtSoNo.Text = ProductionManagement.Transactions.frmForge_MO_List.MO_No;                
                String myString = "";
                int QuoteMasterID = 0;
                myString = txtSoNo.Text;
                var da = (from obj in db.Forge_MFG_Order_Masters
                          where obj.MO_No == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    QuoteMasterID = da[0].id;                  
                    dpSODate.Text = da[0].MO_Date.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].Customer_Name;
                    var pStatus = (from m in db.Forge_Mfg_Order_Childs where m.MO_Master_ID == QuoteMasterID select new { m.Mo_S_No }).Distinct().ToList();
                    if (pStatus.Count > 0)
                    {
                        cmbQuotNo.DataSource = pStatus;
                        cmbQuotNo.ValueMember = "Mo_S_No";
                        cmbQuotNo.DisplayMember = "Mo_S_No";
                    }
                    
                    cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                }


                
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
        
        }

        private void cmbQuotNo_LocationChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                if (tb3 != null && columnName == "Specification")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addQP(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void addQP(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;
                string TestName = dataGridView1.Rows[rowindex].Cells["Characterstics"].Value.ToString();
                if (columnName == "Specification")
                {


                    var Prodname = (from d in db.Get_QAP_To_MAP(logIn.company,TestName,txtCustPoNo.Text.Trim())                                   
                                    select new { d.Plan_Ref_No,d.Plan_Description }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Plan_Ref_No");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Plan_Description);
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
            string columnName = dataGridView1.Columns[columnIndex].Name;


            if (columnName == "Spec")
            {
                var getProductName = (from s in db.TestMasters
                                      where s.TestName == R1.Cells["Characterstics"].Value.ToString() && s.Company_ID == logIn.company
                                      select new { s.TestType }).FirstOrDefault();

                if (getProductName.TestType != "Others")
                {
                    var qap = (from u in db.Quaity_Plans
                               where u.Plan_Description == R1.Cells["Spec"].Value
                                    && u.Company_ID == logIn.company
                               select new { u.Plan_Ref_No }).ToList();

                    if (qap.Count > 0)
                    {
                        R1.Cells["Acceptance_Criteria"].Value = qap[0].Plan_Ref_No;
                    }
                    else

                    {
                        MessageBox.Show("Please  select the valid  QAP Ref No");
                        R1.Cells["Spec"].Value = "";
                        return;
                    }
                }
            }

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (txtSoNo.Text != "")
                //{

                //    if (frmForge_MO_List.editMode == true)
                //    {
                //    }
                //    else
                //    {

                //        if ((from u in db.Forge_MFG_Order_Masters where u.MO_No == txtSoNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                //        {
                //            MessageBox.Show("MO No Cannot Be Duplicate", "MO Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            txtSoNo.Focus();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MO Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
        
        }
    }
}
