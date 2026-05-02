using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
using Syncfusion.Windows.Forms.Grid;
using ioneNet.MaterialManagement;

namespace ioneNet.ProductionManagement.Masters
{

    public partial class Bom_ProjectWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public Bom_ProjectWise()
        {
            InitializeComponent();
        }

        private void Bom_Load(object sender, EventArgs e)
        {

            BindMasters();
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


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
                //Bind product Groups
                var bindGroups = (from m in db.Product_Groups
                                  where m.Company_ID == logIn.company
                                  select new
                                  {
                                      m.Prod_Group_Name,
                                      m.ID,
                                  }).ToList();

                if (bindGroups.Count > 0)
                {
                    cmbProdGroup.DataSource = bindGroups;
                    cmbProdGroup.DisplayMember = "Prod_Group_Name";
                    cmbProdGroup.ValueMember = "ID";
                    cmbProdGroup.SelectedIndex = -1;

                }




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
        public void clear()
        {
            cmbMONo.Text = "";
            //txtuom.Text = "";
            //txtcategory.Text = "";
            txtModule.Text = "";
            txtTotalComp.Text = "";
            txtTotalQuantity.Text = "";
            if (dgbom.Rows.Count > 0)
            {
                for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                {
                    dgbom.Rows.RemoveAt(i);
                    i--;
                    while (dgbom.Rows.Count == 0)
                        continue;
                }
            }
        }
        private void cmbpname_Leave(object sender, EventArgs e)
        {
            try
            {

                if (cmbMONo.Text != "")
                {
                    var sa = (from s in db.Engg_Mfg_Orders
                              join u in db.Project_code_Masters on s.Project_ID equals u.id
                              where s.MO_No == cmbMONo.Text && s.Company_ID == logIn.company
                              select new { u.Project_Code, s.Project_ID }).ToList();
                    //var sa = (from a in db.Products where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) select new { a.Prod_Primary_UOM_Id, a.Prod_Group_Id,a.Prod_Unit_Wt }).ToList();
                    if (sa.Count > 0)
                    {

                        txtProjectCode.Text = sa[0].Project_Code.ToString();
                        txtProjectID.Text = sa[0].Project_ID.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Invalid MO No Selected");
                        cmbMONo.Focus();
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

                DataGridViewRow R1 = dgbom.Rows[dgbom.CurrentRow.Index];
                int columnIndex = dgbom.CurrentCell.ColumnIndex;
                string columnName = dgbom.Columns[columnIndex].HeaderText;

                if (columnName == "Item Name")
                {
                    var Prodname = (from d in db.Products where d.Company_ID == logIn.company select new { d.Prod_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ProductName");
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
            }
        }
        private void dgbom_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (dgbom.Rows.Count > 0)
                {
                    int columnIndex = dgbom.CurrentCell.ColumnIndex;
                    string columnName = dgbom.Columns[columnIndex].HeaderText;


                    TextBox tb3 = e.Control as TextBox;
                    if (columnName == "Item Name")
                    {
                        if (tb3 != null && columnName == "Item Name")
                        {
                            tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                            tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                            addItems(DataColl);
                            tb3.AutoCompleteCustomSource = DataColl;
                        }

                        //Grade

                    }
                    else
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.None;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void sumqty()
        {
            decimal sum = 0;
            try
            {
                txtTotalQuantity.Text = "";
                for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                {
                    sum += (dgbom.Rows[i].Cells["QtyReq"].Value == "" || dgbom.Rows[i].Cells["QtyReq"].Value == null || dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);

                    //sum += Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                }
                txtTotalQuantity.Text = Convert.ToString(Math.Round((sum), 2));

                txtTotalComp.Text = "";
                sum = 0;
                for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                {
                    sum += (dgbom.Rows[i].Cells["CompPer"].Value == "" || dgbom.Rows[i].Cells["CompPer"].Value == null || dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);

                    //sum += Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                }
                txtTotalComp.Text = Convert.ToString(Math.Round((sum), 2));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgbom_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgbom.Rows[dgbom.CurrentRow.Index];
                int columnIndex = dgbom.CurrentCell.ColumnIndex;
                string columnName = dgbom.Columns[columnIndex].Name;
                decimal Composition, Batchsize, UnitWt, QtyRequiredBatch, TotalComposition = 0;
                if (cmbMONo.Text == "")
                {
                    MessageBox.Show("Select MO No");
                    cmbMONo.Focus();
                    return;
                }
                if (cmbProdGroup.Text == "")
                {
                    MessageBox.Show("Material Group cannot Be Blank");
                    return;
                }
                if (columnName == "Item_Name")
                {
                    if (R1.Cells["Item_Name"].Value != null)
                    {

                        var getProductName = (from s in db.Products
                                              join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                              join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                              where s.Prod_Name == R1.Cells["Item_Name"].Value.ToString() && s.Company_ID == logIn.company
                                              select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_Code }).FirstOrDefault();

                        if (getProductName != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            R1.Cells["Group_Name"].Value = getProductName.Prod_Group_Name.ToString();

                            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                            //{
                            if(R1.Cells["SNo"].Value == null)
                            { 
                            R1.Cells["Prod_Code"].Value = getProductName.Prod_Code.ToString();
                            var result = db.Sp_autoincrement_BOM_ItemNo(logIn.company,cmbMONo.Text,Convert.ToInt32(txtProjectID.Text),Convert.ToInt32(cmbProdGroup.SelectedValue),txtModule.Text );
                           
                            R1.Cells["SNo"].Value = result.FirstOrDefault().Item_No;
                            }
                        }
                    }
                }
                else
                if (columnName == "QtyReq")
                {


                    decimal c = 0;
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        c += Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                    }
                    txtTotalQuantity.Text = c.ToString();
                    TotalComposition = c;
                    //Batchsize = Convert.ToDecimal(txtModule.Text);
                    //UnitWt = Convert.ToDecimal(txtProjectCode.Text);
                    //for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    //{
                    //    decimal Composition1 = 0;
                    //    if (dgbom.Rows[i].Cells["QtyReq"].Value != null)
                    //    {

                    //        Composition1 = Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);

                    //        QtyRequiredBatch = ((Composition1 / Batchsize) *100) ;
                    //        //dgbom.Rows[i].Cells["CompPer"].Value = QtyRequiredBatch;

                    //       }
                    //}
                    //sumqty();
                }

                //DataGridViewRow R1 = dgbom.Rows[dgbom.CurrentRow.Index];
                //int columnIndex = dgbom.CurrentCell.ColumnIndex;
                //string columnName = dgbom.Columns[columnIndex].HeaderText;
                //if (columnName == "ProductName")
                //{

                //    string pname =  dgbom.Rows[columnIndex].Cells["Item_Name"].Value.ToString(); 
                //    var sa = (from a in db.SP_BIND_Items(pname) select a).ToList();
                //    if (sa.Count > 0)
                //    {
                //        dgbom.DataSource = sa;
                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are You Sure Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                db.Sp_delete_BomProject(logIn.company,cmbMONo.Text, Convert.ToInt32(txtProjectID.Text),txtModule.Text);
                MessageBox.Show("Recored Deleted Successfully");
                clear();
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        private void SaveBOM()
        {
            try
            {
                String myString = "";

                //myString = txtSoNo.Text;
                //string AmendNo = txtAmendNo.Text;
                if ((from a in db.BOM_Projects where a.Company_ID == logIn.company && a.MO_No == cmbMONo.Text && a.Project_Id == Convert.ToInt32(txtProjectID.Text) && a.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && a.Module_No == txtModule.Text select a).Count() > 0)
                {
                    db.Sp_delete_BomProject(logIn.company, cmbMONo.Text, Convert.ToInt32(txtProjectID.Text), txtModule.Text);
                }

                   
                SqlCommand cmd = new SqlCommand("SaveBOM_PROJECT", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MO_No", cmbMONo.Text);                  
                cmd.Parameters.AddWithValue("@Project_Id", Convert.ToInt32(txtProjectID.Text.ToString()));                   
                cmd.Parameters.AddWithValue("@Material_Group", Convert.ToInt32(cmbProdGroup.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@Module_No", (txtModule.Text == "") ? "" : txtModule.Text);
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));

                string Item_Sno = "";
                string RM_Item_ID = "";
                string QtyReq = "";
                string Remarks = "";                   
                int rowcount = 0;
                int PSno = 0;
                decimal PrQty = 0;
                for (int i = 0; i < dgbom.RowCount - 1; i++)
                {

                       
                    //Product_Description = Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).PadRight(50); 
                    RM_Item_ID = RM_Item_ID + Convert.ToString(dgbom.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                    QtyReq = QtyReq + Convert.ToString(dgbom.Rows[i].Cells["QtyReq"].Value).PadRight(14);
                    Remarks = Remarks + Convert.ToString(dgbom.Rows[i].Cells["Remarks"].Value).PadRight(50);
                    //PSno = PSno +Convert.ToInt32( dgProducts.Rows[i].Cells["S_No"].Value);
                    if (dgbom.Rows[i].Cells["SNo"].Value == null || dgbom.Rows[i].Cells["SNo"].Value.ToString() == "")
                    {


                        //var d1 = (from a in db.Purchase_Order_Childs where a.PO_NO == myString && a.Company_ID == logIn.company select a.ProdSno).ToList().Max(a => a.ProdSno);
                        //  string sno = d1[0].prod;

                        if (PSno == 0)
                        {
                            Item_Sno = Item_Sno + Convert.ToString(i + 1).PadRight(14);
                            PSno = i + 1;
                        }
                        else
                        {
                            Item_Sno = Item_Sno + Convert.ToString(PSno + 1).PadRight(14);
                            PSno = PSno + 1;
                        }
                    }
                    else
                    {
                        Item_Sno = Item_Sno + Convert.ToString(dgbom.Rows[i].Cells["SNo"].Value).PadRight(14);
                        PSno = Convert.ToInt32(dgbom.Rows[i].Cells["SNo"].Value);
                    }


                     
                    rowcount += 1;
                }

                cmd.Parameters.AddWithValue("@txt_RM_Item_ID", RM_Item_ID);                            
                cmd.Parameters.AddWithValue("@txt_QtyReq", QtyReq);               
                cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);               
                cmd.Parameters.AddWithValue("@txt_Item_Sno", Item_Sno);
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
                if ((from a in db.BOM_Projects where a.Company_ID == logIn.company && a.MO_No == cmbMONo.Text && a.Project_Id == Convert.ToInt32(txtProjectID.Text) && a.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && a.Module_No == txtModule.Text select a).Count() > 0)
                {
                    //db.Sp_delete_BomProject(logIn.company, cmbMONo.Text, Convert.ToInt32(txtProjectID.Text), txtModule.Text);
                    for (int i = 0; i < dgbom.Rows.Count; i++)
                    {
                        if (dgbom.Rows[i].Cells["Item_Name"].Value != null)
                        {
                            int RMID = Convert.ToInt32(dgbom.Rows[i].Cells["SNo"].Value);

                            if ((from u in db.BOM_Projects where u.MO_No == cmbMONo.Text && u.Item_Sno == RMID && u.Project_Id == Convert.ToInt32(txtProjectID.Text) && u.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && u.Module_No == txtModule.Text && u.Company_ID == logIn.company select u).Count() > 0)
                            {

                                var p1 = db.BOM_Projects.Where(w => w.MO_No == cmbMONo.Text && w.Item_Sno == RMID && w.Project_Id == Convert.ToInt32(txtProjectID.Text) && w.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && w.Module_No == txtModule.Text && w.Company_ID == logIn.company).FirstOrDefault();
                                p1.Project_Id = Convert.ToInt32(txtProjectID.Text);
                                p1.Project_Code = txtProjectCode.Text;
                                p1.Material_Group = Convert.ToInt32(cmbProdGroup.SelectedValue);
                                p1.MO_No = cmbMONo.Text;

                                p1.Module_No = txtModule.Text;
                                p1.Item_Sno = (dgbom.Rows[i].Cells["SNo"].Value == DBNull.Value) ? Convert.ToInt32(i + 1) : Convert.ToInt32(dgbom.Rows[i].Cells["SNo"].Value);

                                p1.RM_Item_Name = (dgbom.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Item_Name"].Value.ToString();
                                p1.RM_Prod_Code = (dgbom.Rows[i].Cells["Prod_Code"].Value == null) ? "" : dgbom.Rows[i].Cells["Prod_Code"].Value.ToString();


                                p1.RM_Item_ID = Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value);
                                var d1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value) select new { a.Prod_Group_Id, a.Prod_Primary_UOM_Id }).ToList();
                                if (d1.Count > 0)
                                {
                                    p1.RM_Group_Name = d1[0].Prod_Group_Id; //(dgbom.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                                    p1.RM_UOM_ID = d1[0].Prod_Primary_UOM_Id; //(dgbom.Rows[i].Cells["UOM"].Value == null) ? "" : dgbom.Rows[i].Cells["UOM"].Value.ToString();

                                }
                                p1.QtyReq = (dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                                p1.Remarks = (dgbom.Rows[i].Cells["Remarks"].Value == null) ? "" : dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                                p1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                                p1.Company_ID = logIn.company;
                                p1.Created_By = lblCreatedBy.Text;
                                p1.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();
                            }

                            else
                            {
                                BOM_Project bo1 = new BOM_Project();
                                bo1.Project_Id = Convert.ToInt32(txtProjectID.Text);
                                bo1.Project_Code = txtProjectCode.Text;
                                bo1.Material_Group = Convert.ToInt32(cmbProdGroup.SelectedValue);
                                bo1.MO_No = cmbMONo.Text;

                                bo1.Module_No = txtModule.Text;
                                bo1.Item_Sno = (dgbom.Rows[i].Cells["SNo"].Value == DBNull.Value) ? Convert.ToInt32(i + 1) : Convert.ToInt32(dgbom.Rows[i].Cells["SNo"].Value);

                                bo1.RM_Item_Name = (dgbom.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Item_Name"].Value.ToString();
                                bo1.RM_Prod_Code = (dgbom.Rows[i].Cells["Prod_Code"].Value == null) ? "" : dgbom.Rows[i].Cells["Prod_Code"].Value.ToString();


                                bo1.RM_Item_ID = Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value);
                                var d1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value) select new { a.Prod_Group_Id, a.Prod_Primary_UOM_Id }).ToList();
                                if (d1.Count > 0)
                                {
                                    bo1.RM_Group_Name = d1[0].Prod_Group_Id; //(dgbom.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                                    bo1.RM_UOM_ID = d1[0].Prod_Primary_UOM_Id; //(dgbom.Rows[i].Cells["UOM"].Value == null) ? "" : dgbom.Rows[i].Cells["UOM"].Value.ToString();

                                }
                                bo1.QtyReq = (dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                                bo1.Remarks = (dgbom.Rows[i].Cells["Remarks"].Value == null) ? "" : dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                                bo1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                                bo1.Company_ID = logIn.company;
                                bo1.Created_By = lblCreatedBy.Text;
                                bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.BOM_Projects.InsertOnSubmit(bo1);
                            }
                        }
                    }

                    db.SubmitChanges();
                    //MessageBox.Show("Recored Updated Successfully");
                    //clear();
                    return;
                }
                else
                {
                    for (int i = 0; i < dgbom.Rows.Count; i++)
                    {
                        if (dgbom.Rows[i].Cells["Item_Name"].Value != null)
                        {
                            BOM_Project bo1 = new BOM_Project();
                            bo1.Project_Id = Convert.ToInt32(txtProjectID.Text);
                            bo1.Project_Code = txtProjectCode.Text;
                            bo1.Material_Group = Convert.ToInt32(cmbProdGroup.SelectedValue);
                            bo1.MO_No = cmbMONo.Text;

                            bo1.Module_No = txtModule.Text;
                            bo1.Item_Sno = (dgbom.Rows[i].Cells["SNo"].Value == DBNull.Value) ? Convert.ToInt32(i + 1) : Convert.ToInt32(dgbom.Rows[i].Cells["SNo"].Value);

                            bo1.RM_Item_Name = (dgbom.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Item_Name"].Value.ToString();
                            bo1.RM_Prod_Code = (dgbom.Rows[i].Cells["Prod_Code"].Value == null) ? "" : dgbom.Rows[i].Cells["Prod_Code"].Value.ToString();


                            bo1.RM_Item_ID = Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value);
                            var d1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value) select new { a.Prod_Group_Id, a.Prod_Primary_UOM_Id }).ToList();
                            if (d1.Count > 0)
                            {
                                bo1.RM_Group_Name = d1[0].Prod_Group_Id; //(dgbom.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                                bo1.RM_UOM_ID = d1[0].Prod_Primary_UOM_Id; //(dgbom.Rows[i].Cells["UOM"].Value == null) ? "" : dgbom.Rows[i].Cells["UOM"].Value.ToString();

                            }
                            bo1.QtyReq = (dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                            bo1.Remarks = (dgbom.Rows[i].Cells["Remarks"].Value == null) ? "" : dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                            bo1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                            bo1.Company_ID = logIn.company;
                            bo1.Created_By = logIn.username + "-" + DateTime.Now;
                            bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                            db.BOM_Projects.InsertOnSubmit(bo1);
                        }
                    }


                    db.SubmitChanges();
                    //MessageBox.Show("Recored Saved Successfully");
                    //clear();
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void GetMethod()
        {

            var sa = (from a in db.BOM_Projects
                      where  a.Company_ID == logIn.company && a.Project_Id == Convert.ToInt32(txtProjectID.Text) && a.MO_No == cmbMONo.Text && a.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && a.Module_No == txtModule.Text
                      select new
                      {
                          a.Project_Id,
                          a.Project_Code,
                          a.MO_No,
                          a.Material_Group,
                          a.Module_No,
                          a.Status,
                          a.Created_By,
                          a.Modified_By,
                      }).ToList();
            if (sa.Count > 0)
            {
                //cmbpname.SelectedValue = sa[0].Bom_Item_ID;
                cmbProdGroup.SelectedValue = sa[0].Material_Group;
                txtModule.Text = sa[0].Module_No;
                txtProjectCode.Text = sa[0].Project_Code;
                txtProjectID.Text = sa[0].Project_Id.ToString();

                if (sa[0].Status != null)
                {
                    cmbStatus.SelectedValue = (sa[0].Status);
                }
                //cmbStatus.SelectedValue = sa[0].Status;
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_By;
            }
            var ca = (from sq in db.BOM_Projects                      
                      join p in db.Products on sq.RM_Item_ID equals p.prod_ID
                      join b in db.Product_Groups on p.Prod_Group_Id equals b.ID
                      join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                      where sq.Company_ID == logIn.company && sq.Project_Id == Convert.ToInt32(txtProjectID.Text) && sq.MO_No == cmbMONo.Text && sq.Material_Group == Convert.ToInt32(cmbProdGroup.SelectedValue) && sq.Module_No == txtModule.Text
                      orderby sq.Item_Sno
                      select new
                      {
                          SNo = sq.Item_Sno,
                          Prod_Code = p.Prod_Code,
                          Item_Code = sq.RM_Item_ID,
                          Item_Name = p.Prod_Name,
                          Group_Name = b.Prod_Group_Name,
                          UOM = u.Uom_Descr,
                          sq.QtyReq,
                          sq.Remarks
                      });
            SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt1 = new DataTable();
            da3.Fill(dt1);
            if (dt1.Rows.Count > 0)
                dgbom.DataSource = dt1;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionManagement.Masters.Bom_Project_Search.frmName = "BOM-PROJECT";
                ProductionManagement.Masters.Bom_Project_Search obj = new ProductionManagement.Masters.Bom_Project_Search();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    cmbMONo.Text = ProductionManagement.Masters.Bom_Project_Search.MoNO;
                    txtProjectID.Text = ProductionManagement.Masters.Bom_Project_Search.Proj_Code;
                    cmbProdGroup.Text = ProductionManagement.Masters.Bom_Project_Search.Group_Name;
                    txtModule.Text = ProductionManagement.Masters.Bom_Project_Search.ModuleNo;
                    GetMethod();
                    //sumqty();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtTotalQuantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMONo.Text == "")
                {
                    MessageBox.Show("Select MO No");
                    cmbMONo.Focus();
                    return;


                }
                else
                if (txtProjectID.Text == "")
                {
                    MessageBox.Show("Project Code Cannot Be Blank");
                    //cmbMONo.Focus();
                    return;


                }
                else
                if (txtModule.Text == "")
                {
                    MessageBox.Show("Please Enter the Module No...!");
                    txtModule.Focus();
                    return;


                }
                else
                {
                    //decimal a = (txtbatchlot.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtbatchlot.Text);
                    //decimal b = (txtTotalQuantity.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtTotalQuantity.Text);
                    //if (a == b)
                    //{
                    //Save();
                    SaveBOM();
                    // }
                    //else
                    //{
                    //    MessageBox.Show("Sum Qty should be Equal to Batch/Lot...");
                    //    txtTotalQuantity.Focus();
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }



        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgbom.Rows.Count > 0)
                {
                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    // creating new WorkBook within Excel application  
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                    // creating new Excelsheet in workbook  

                    //Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    //Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                    var data = (from s in db.Company_Infos
                                where s.Id == logIn.company
                                select new
                                {
                                    s.Company_Name,
                                    Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                                }).ToList();
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet.Cells[1, 1] = data[0].Company_Name.ToString();
                    worksheet.Cells[2, 1] = "BOM";

                    worksheet.Cells[3, 1] = "Production Name : " + cmbMONo.Text;
                    worksheet.Cells[3, 4] = cmbProdGroup.Text;

                    worksheet.Cells[4, 1] = "Batch Size";
                    worksheet.Cells[4, 2] = txtModule.Text;

                    worksheet.Cells[4, 3] = "Receipe Code";
                    worksheet.Cells[4, 4] = txtModule.Text;


                    worksheet.Range["A1:d1"].MergeCells = true;
                    worksheet.Range["A2:d2"].MergeCells = true;
                    worksheet.Range["A3:c3"].MergeCells = true;
                    worksheet.Range["D3:F3"].MergeCells = true;



                    for (int j = 1; j < dgbom.Columns.Count; j++)
                    {
                        worksheet.Cells[5, j] = dgbom.Columns[j - 1].HeaderText;
                    }
                    ///*a*/pp.Visible = true;
                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < dgbom.Rows.Count; k++)
                    {
                        for (int l = 0; l < dgbom.Columns.Count - 1; l++)
                        {
                            //if (l == 1)
                            //{
                            //    string d = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(0, 2);
                            //    string m = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(3, 2);
                            //    string y = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(6, 4);
                            //    worksheet.Cells[k + 4, l + 1] = d + '/' + m + '/' + y;
                            //}
                            //else
                            //{
                            worksheet.Cells[k + 6, l + 1] = (dgbom.Rows[k].Cells[l].Value == "" || dgbom.Rows[k].Cells[l].Value == null ||
                                dgbom.Rows[k].Cells[l].Value == DBNull.Value) ? "" : dgbom.Rows[k].Cells[l].Value.ToString();
                            //}
                            //app.Range[worksheet.Cells[k + 4, 2], worksheet.Cells[k + 4, 2]].NumberFormat
                            //   = "dd-MM-yyyy";
                        }
                    }



                    worksheet.PageSetup.PrintGridlines = true;
                    worksheet.PageSetup.LeftMargin = 1.00;
                    worksheet.PageSetup.RightMargin = 0.50;
                    worksheet.Columns.AutoFit();
                    app.Visible = true;

                    //ExportToExcel(dataGridView1, "Invoice_Report");
                }



                //if (dgbom.Rows.Count > 0)
                //{
                //    ExportToExcel(dgbom, "Invoice_Report");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgbom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) //Remove Rows
            {
                if (dgbom.Rows.Count > 0)
                {

                    foreach (DataGridViewCell oneCell in dgbom.SelectedCells)
                    {
                        if (oneCell.Selected)
                            dgbom.Rows.RemoveAt(oneCell.RowIndex);
                    }
                }
            }
            int Sno = 0;
            DataTable dtexisting = new DataTable();
            if (e.KeyCode == Keys.F2)
            {
                ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                ioneNet.Masters.ProdSearch.frmName = "BOM-PROJECT";
                form.ShowDialog();
                if (dgbom.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("SNo", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Name", typeof(string));
                    dtexisting.Columns.Add("Group_Name", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("QtyReq", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["SNo"] = dgbom.Rows[i].Cells["SNo"].Value.ToString();
                        dr["Item_Code"] = dgbom.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Prod_Code"] = dgbom.Rows[i].Cells["Prod_Code"].Value.ToString();
                        dr["Item_Name"] = dgbom.Rows[i].Cells["Item_Name"].Value.ToString();
                        dr["Group_Name"] = dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                        dr["UOM"] = dgbom.Rows[i].Cells["UOM"].Value.ToString();
                        dr["QtyReq"] = dgbom.Rows[i].Cells["QtyReq"].Value.ToString();
                        dr["Remarks"] = dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                        Sno = Sno + 1;
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }

                if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SNo", typeof(string));
                    dt.Columns.Add("Item_Code", typeof(string));
                    dt.Columns.Add("Prod_Code", typeof(string));
                    dt.Columns.Add("Item_Name", typeof(string));
                    dt.Columns.Add("Group_Name", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("QtyReq", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));

                    Sno = Sno + 1;
                    //dt.Rows.Add();
                    for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                    {
                        string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                        var getproducts = (from obj in db.Products
                                           join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                           join pg in db.Product_Groups on obj.Prod_Group_Id equals pg.ID
                                           join tm in db.Tax_Class_Masters on obj.Prod_Tax_Class equals tm.ID
                                           where obj.prod_ID == Convert.ToInt32(prodcode)
                                           select new
                                           {

                                               SNo = Sno,
                                               Item_Code = obj.prod_ID,
                                               Prod_Code = obj.Prod_Code,
                                               Item_Name = obj.Prod_Name,
                                               Group_Name = pg.Prod_Group_Name,
                                               UOM = uom.Uom_Descr,
                                               QtyReq = 0,
                                               Remarks = "",

                                           }).ToList();
                        dt.Rows.Add(getproducts[0].SNo, getproducts[0].Item_Code, getproducts[0].Prod_Code, getproducts[0].Item_Name, getproducts[0].Group_Name, getproducts[0].UOM, getproducts[0].QtyReq, getproducts[0].Remarks);
                        Sno = Sno + 1;
                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    dgbom.DataSource = dtexisting;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Bind Project
            var bindGroups = (from m in db.BOM_Projects
                              where m.Company_ID == logIn.company
                              select new
                              {
                                  m.Project_Code,
                                  m.Project_Id,
                              }).Distinct().ToList();

            if (bindGroups.Count > 0)
            {
                cmbOldProjCode.DataSource = bindGroups;
                cmbOldProjCode.DisplayMember = "Project_Code";
                cmbOldProjCode.ValueMember = "Project_Id";
                cmbOldProjCode.SelectedIndex = -1;

            }
            groupBox2.Visible = true;
            cmbOldProjCode.Focus();
        }

        private void MigrateOLDBOM()
        {
            SqlConnection conOld = new SqlConnection();
         
            conOld.ConnectionString = "Data Source=113.193.191.187\\sqlexpress,1433; Initial Catalog = VPackERP; User ID = sa; Password = vpack@123456";
            conOld.Open();

            SqlDataAdapter adpt = new SqlDataAdapter("Select * from BOM where compname like '%V-Pack%'", conOld);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
           




        }

        private void txtModule_Enter(object sender, EventArgs e)
        {
            try
            {
                txtModule.AutoCompleteCustomSource = null;
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddProject(DataColl);
                txtModule.AutoCompleteCustomSource = DataColl;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddProject(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Project_Module_Masters
                                where d.Project_ID == Convert.ToInt32(txtProjectID.Text)
                                select new { d.Module_Name }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Module_Name");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Module_Name);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cmbOldProjCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            { 
            //Bind Project
            var bindGroups = (from m in db.BOM_Projects
                              where m.Company_ID == logIn.company && m.Project_Id == Convert.ToInt32(cmbOldProjCode.SelectedValue)
                              select new
                              {
                                  m.Module_No
                                
                              }).Distinct().ToList();

            if (bindGroups.Count > 0)
            {
                cmbOldModule.DataSource = bindGroups;
                cmbOldModule.DisplayMember = "Module_No";
                cmbOldModule.ValueMember = "Module_No";
                cmbOldModule.SelectedIndex = -1;

            }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnCancelDialog_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void btnImportBOM_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                {
                    var bindGroups = (from m in db.CopyAnalogBOM(logIn.company, Convert.ToInt32(cmbOldProjCode.SelectedValue))
                                   

                                      select new
                                      {
                                          SNo = m.Sno,
                                          Item_Code = m.Item_Code,
                                          Prod_Code = m.Prod_Code,
                                          Item_Name = m.Item_Name,
                                          Group_Name = m.Prod_Group_Name,
                                          UOM = m.Uom_Descr,
                                          QtyReq = m.QtyReq

                                      }).Distinct().ToList();

                    if (bindGroups.Count >= 0)
                        dgbom.DataSource = bindGroups;
                    else
                    {
                        MessageBox.Show("Invalid Project Parameters Entered, No Data Found");
                    }
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("GeBOM_Analog ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@project", Convert.ToInt32(cmbOldProjCode.SelectedValue));
                    cmd2.Parameters.AddWithValue("@module", cmbOldModule.Text);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgbom.DataSource = ds2;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtModule_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtModule.Text != "")
                {
                    if ((from u in db.Project_Module_Masters where u.Project_ID == Convert.ToInt32(txtProjectID.Text) &&  u.Module_Name == txtModule.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid Module Name Entered");
                        txtModule.Focus();
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
