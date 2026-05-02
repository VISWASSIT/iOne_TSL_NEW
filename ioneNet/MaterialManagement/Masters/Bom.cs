using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ione_DAL;
using System.Data.OleDb;

namespace ioneNet.MaterialManagement.Masters
{
   
    public partial class Bom : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public Bom()
        {
            InitializeComponent();
        }

        private void Bom_Load(object sender, EventArgs e)
        {
           
            BindMasters();
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            if (ProductionManagement.ProductionReport.var == "0")
            {
                cmbpname.SelectedValue = ProductionManagement.ProductionReport.iTem_Code;
                txtReceipeCode.Text = ProductionManagement.ProductionReport.RCode;
                GetMethod();                    
                
            }

        }

        public void BindMasters()
        {
            try
            {

                //Bind Products
                var sa = (from a in db.Products
                          where a.Prod_IsBOM_Item == true && a.Company_ID == logIn.company
                          select new { a.Prod_Name, a.prod_ID }).ToList();
                if (sa.Count > 0)
                {
                    cmbpname.DataSource = sa;
                    cmbpname.DisplayMember = "Prod_Name";
                    cmbpname.ValueMember = "prod_ID";
                    if (cmbpname.Items.Count > 0)
                    {
                        cmbpname.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbpname.SelectedIndex = -1;
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

                //Bind UOM
                var bindUOM = (from m in db.UoM_Masters
                               //where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Uom_Descr,
                                   m.UOM_ID,
                               }).Distinct().ToList();

                if (bindUOM.Count > 0)
                {
                    cmbUom.DataSource = bindUOM;
                    cmbUom.DisplayMember = "Uom_Descr";
                    cmbUom.ValueMember = "UOM_ID";
                    cmbUom.SelectedIndex = -1;
                }
              
                //Bind Tax Calss
               
               
                

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
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
            cmbpname.Text = "";
            //txtuom.Text = "";
            //txtcategory.Text = "";
            txtbatchlot.Text = "";
            txtReceipeCode.Text = "";
            cmbUom.Text = "";
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

                var sa = (from s in db.Products
                                      join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                      join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                      where s.prod_ID == Convert.ToInt32(cmbpname.SelectedValue)
                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name,s.Prod_Unit_Wt ,s.Prod_Group_Id,s.Prod_Primary_UOM_Id}).ToList();
                //var sa = (from a in db.Products where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) select new { a.Prod_Primary_UOM_Id, a.Prod_Group_Id,a.Prod_Unit_Wt }).ToList();
                if(sa.Count>0)
                {
                    cmbProdGroup.SelectedValue =sa[0].Prod_Group_Id;
                    cmbUom.SelectedValue = sa[0].Prod_Primary_UOM_Id;
                    txtUnitWt.Text = sa[0].Prod_Unit_Wt.ToString();
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

                if (columnName == "Receipe_Code")
                {
                    var RCode = (from d in db.BOMs where d.Bom_Item_ID == Convert.ToInt32(R1.Cells["Item_Code"].Value) &&  d.Company_ID == logIn.company select new { d.Bom_ReceipeCode }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Receipe_Code");
                    foreach (var item in RCode)
                    {
                        dt.Rows.Add(item.Bom_ReceipeCode);
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
                     if (columnName == "Receipe_Code")
                    {
                        if (tb3 != null && columnName == "Receipe_Code")
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
                txtTotalQuantity.Text = Convert.ToString(Math.Round((sum),2));

                txtTotalComp.Text = "";
                sum = 0;
                for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                {
                    sum += (dgbom.Rows[i].Cells["CompPer"].Value == "" || dgbom.Rows[i].Cells["CompPer"].Value == null || dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);

                    //sum += Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                }
                txtTotalComp.Text = Convert.ToString(Math.Round((sum),2));
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
                if(txtbatchlot.Text =="")
                {
                    MessageBox.Show("Enter Batch Size");
                    txtbatchlot.Focus();
                    return;
                }
                if (Convert.ToDecimal(txtUnitWt.Text)==0)
                {
                    MessageBox.Show("Unit Wt Should Be Greater Than 0");
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
                                              select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name }).FirstOrDefault();

                        if (getProductName != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            R1.Cells["Group_Name"].Value = getProductName.Prod_Group_Name.ToString();
                            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                            //{
                            //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                            //}
                        }
                    }
                }
                else
                if (columnName == "RM_Receipe_Code")
                {
                    var getProductName = (from s in db.BOMs                                         
                                          where s.Bom_Item_ID == Convert.ToInt32(R1.Cells["Item_Code"].Value) && s.Bom_ReceipeCode == R1.Cells["RM_Receipe_Code"].Value && s.Company_ID == logIn.company
                                          select new { s.Bom_ReceipeCode }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        //R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        //R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        //R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                        //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                        //{
                        //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Invalid Receipe Code Entered");
                        R1.Cells["RM_Receipe_Code"].Value = "";
                        return;
                    }
                }
                else
                if (columnName == "CompPer")
                {


                    decimal c = 0;
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        c += (dgbom.Rows[i].Cells["CompPer"].Value == "" || dgbom.Rows[i].Cells["CompPer"].Value == null || dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);

                        //c += Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                    }
                    txtTotalComp.Text = c.ToString();
                    TotalComposition = c;
                    Batchsize = Convert.ToDecimal(txtbatchlot.Text);
                    UnitWt = Convert.ToDecimal(txtUnitWt.Text);
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        decimal Composition1 = 0;
                        Composition1 = (dgbom.Rows[i].Cells["CompPer"].Value == "" || dgbom.Rows[i].Cells["CompPer"].Value == null || dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                        if (Composition1 > 0)
                        {


                            //Composition1 = Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);

                            QtyRequiredBatch = (Batchsize * UnitWt * Composition1) / c;
                            dgbom.Rows[i].Cells["QtyReq"].Value = QtyRequiredBatch;
                        }
                    }
                    sumqty();
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
                    Batchsize = Convert.ToDecimal(txtbatchlot.Text);
                    UnitWt = Convert.ToDecimal(txtUnitWt.Text);
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        decimal Composition1 = 0;
                        if (dgbom.Rows[i].Cells["QtyReq"].Value != null)
                        {

                            Composition1 = Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);

                            QtyRequiredBatch = ((Composition1 / Batchsize) * 100);
                            //dgbom.Rows[i].Cells["CompPer"].Value = QtyRequiredBatch;

                        }
                    }
                    sumqty();
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
                db.Sp_delete_Bom(logIn.company,Convert.ToInt32(cmbpname.SelectedValue), txtReceipeCode.Text);
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
        private void Save()
        {
            try
            {
                if ((from a in db.BOMs where a.Company_ID == logIn.company && a.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && a.Bom_ReceipeCode == txtReceipeCode.Text  select a).Count() > 0)
                {
                    db.Sp_delete_Bom(logIn.company, Convert.ToInt32(cmbpname.SelectedValue),txtReceipeCode.Text);
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        BOM bo1 = new BOM();
                        bo1.Bom_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                        bo1.Bom_Item_UOM_ID = Convert.ToInt32(cmbUom.SelectedValue);
                        bo1.Bom_Item_Group_Name = Convert.ToInt32(cmbProdGroup.SelectedValue);
                        bo1.Bom_Item_UnitWt = (txtUnitWt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtUnitWt.Text);
                        
                        bo1.Bom_Batchsize = (txtbatchlot.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtbatchlot.Text);
                        bo1.Bom_ReceipeCode = (txtReceipeCode.Text == null) ? "" : txtReceipeCode.Text;
                        bo1.Default_Receipe = chkDefaultReceipe.Checked;
                        bo1.RM_Item_Name = (dgbom.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Item_Name"].Value.ToString();

                        bo1.RM_Item_ID = Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value);
                        var d1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value) select new { a.Prod_Group_Id,a.Prod_Primary_UOM_Id }).ToList();
                        if (d1.Count > 0)
                        {
                            bo1.RM_Group_Name = d1[0].Prod_Group_Id; //(dgbom.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                            bo1.RM_UOM_ID = d1[0].Prod_Primary_UOM_Id; //(dgbom.Rows[i].Cells["UOM"].Value == null) ? "" : dgbom.Rows[i].Cells["UOM"].Value.ToString();

                        }
                        bo1.RM_Receipe_Code = (dgbom.Rows[i].Cells["RM_Receipe_Code"].Value == null) ? "" : dgbom.Rows[i].Cells["RM_Receipe_Code"].Value.ToString();

                        bo1.Comp_Per = (dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                        bo1.QtyReq = (dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                        bo1.Remarks = (dgbom.Rows[i].Cells["Remarks"].Value == null) ? "" : dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                        bo1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        bo1.Tot_Comp_Per = (txtTotalComp.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotalComp.Text);
                        bo1.Tot_Qty_Req = (txtTotalQuantity.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotalQuantity.Text);
                        bo1.Company_ID = logIn.company;
                        bo1.Created_By = lblCreatedBy.Text;
                        bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.BOMs.InsertOnSubmit(bo1);
                    }

                    db.SubmitChanges();
                    MessageBox.Show("Recored Updated Succesfully");
                    clear();
                    return;
                }
                else
                {
                    for (int i = 0; i < dgbom.Rows.Count - 1; i++)
                    {
                        BOM bo1 = new BOM();
                        bo1.Bom_Item_ID = Convert.ToInt32(cmbpname.SelectedValue);
                        bo1.Bom_Item_UOM_ID = Convert.ToInt32(cmbUom.SelectedValue);
                        bo1.Bom_Item_Group_Name = Convert.ToInt32(cmbProdGroup.SelectedValue);
                        bo1.Bom_Item_UnitWt = (txtUnitWt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtUnitWt.Text);

                        bo1.Bom_Batchsize = (txtbatchlot.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtbatchlot.Text);
                        bo1.Bom_ReceipeCode = (txtReceipeCode.Text == null) ? "" : txtReceipeCode.Text;
                        bo1.Default_Receipe = chkDefaultReceipe.Checked;
                        bo1.RM_Item_Name = (dgbom.Rows[i].Cells["Item_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Item_Name"].Value.ToString();

                        bo1.RM_Item_ID = Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value);
                        var d1 = (from a in db.Products where a.prod_ID == Convert.ToInt32(dgbom.Rows[i].Cells["Item_Code"].Value) select new { a.Prod_Group_Id, a.Prod_Primary_UOM_Id }).ToList();
                        if (d1.Count > 0)
                        {
                            bo1.RM_Group_Name = d1[0].Prod_Group_Id; //(dgbom.Rows[i].Cells["Group_Name"].Value == null) ? "" : dgbom.Rows[i].Cells["Group_Name"].Value.ToString();
                            bo1.RM_UOM_ID = d1[0].Prod_Primary_UOM_Id; //(dgbom.Rows[i].Cells["UOM"].Value == null) ? "" : dgbom.Rows[i].Cells["UOM"].Value.ToString();

                        }
                        bo1.Comp_Per = (dgbom.Rows[i].Cells["CompPer"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["CompPer"].Value);
                        bo1.QtyReq = (dgbom.Rows[i].Cells["QtyReq"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgbom.Rows[i].Cells["QtyReq"].Value);
                        bo1.Remarks = (dgbom.Rows[i].Cells["Remarks"].Value == null) ? "" : dgbom.Rows[i].Cells["Remarks"].Value.ToString();
                        bo1.Company_ID = logIn.company;
                        bo1.Tot_Comp_Per = (txtTotalComp.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotalComp.Text);
                        bo1.Tot_Qty_Req = (txtTotalQuantity.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotalQuantity.Text);
                        
                        bo1.Created_By = logIn.username + "-" + DateTime.Now;
                        bo1.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.BOMs.InsertOnSubmit(bo1);
                    }


                    db.SubmitChanges();
                    MessageBox.Show("Recored Saved Succesfully");
                    clear();
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

            var sa = (from a in db.BOMs
                      where a.Company_ID == logIn.company && a.Bom_Item_ID  == Convert.ToInt32(cmbpname.SelectedValue) && a.Bom_ReceipeCode == txtReceipeCode.Text
                      select new
                      {
                          a.Bom_Item_ID,
                          a.Bom_Item_Group_Name,
                          a.Bom_Item_UOM_ID,
                          a.Bom_ReceipeCode,
                          a.Bom_Batchsize,
                          a.Bom_Item_UnitWt,
                          a.Default_Receipe,
                          a.Status,
                          a.Created_By,
                          a.Modified_By,
                      }).ToList();
            if(sa.Count>0)
            {
                //cmbpname.SelectedValue = sa[0].Bom_Item_ID;
                cmbProdGroup.SelectedValue = sa[0].Bom_Item_Group_Name;
                cmbUom.SelectedValue = sa[0].Bom_Item_UOM_ID;
                txtbatchlot.Text = Convert.ToDecimal(sa[0].Bom_Batchsize).ToString();
                txtUnitWt.Text = Convert.ToDecimal(sa[0].Bom_Item_UnitWt).ToString();
                txtReceipeCode.Text = sa[0].Bom_ReceipeCode;
                chkDefaultReceipe.Checked = sa[0].Default_Receipe.Value;
                if (sa[0].Status != null)
                {
                    cmbStatus.SelectedValue = (sa[0].Status);
                }
                //cmbStatus.SelectedValue = sa[0].Status;
                lblCreatedBy.Text = sa[0].Created_By;
                lblModified.Text = sa[0].Modified_By;
            }
            var ca = (from sq in db.BOMLists
                      where sq.Company_ID == logIn.company && sq.Bom_Item_ID == Convert.ToInt32(cmbpname.SelectedValue) && sq.Bom_ReceipeCode == txtReceipeCode.Text
                      select new
                      {
                          Item_Code= sq.RM_Item_ID,
                          Item_Name= sq.RM_Item_Name,
                          Group_Name= sq.Prod_Group_Name,
                          BOM_Item = sq.Prod_IsBOM_Item,
                          sq.RM_Receipe_Code,
                          UOM =sq.Uom_Descr,
                          CompPer =sq.Comp_Per,
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
                MaterialManagement.Masters.BomSearch obj = new BomSearch();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    cmbpname.Text = MaterialManagement.Masters.BomSearch.FInname;
                    txtReceipeCode.Text = MaterialManagement.Masters.BomSearch.receipcode;
                    GetMethod();
                    sumqty();
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
                if (cmbpname.Text == "")
                {
                    MessageBox.Show("BOM Cannot Be Save Without Product Name...!");
                    txtbatchlot.Focus();
                    return;


                }
                else
                if (txtbatchlot.Text == "")
                {
                    MessageBox.Show("Please Enter the Batch size...!");
                    txtbatchlot.Focus();
                    return;


                }
                else
                 if (txtReceipeCode.Text == "")
                {
                    MessageBox.Show("Please Enter the Receipe Code...!");
                    txtReceipeCode.Focus();
                    return;


                }
                else
                {
                    //decimal a = (txtbatchlot.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtbatchlot.Text);
                    //decimal b = (txtTotalQuantity.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtTotalQuantity.Text);
                    //if (a == b)
                    //{
                    Save();
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
                   
                    worksheet.Cells[3, 1] = "Production Name : " + cmbpname.Text;
                    worksheet.Cells[3, 4] = cmbProdGroup.Text;
                    worksheet.Cells[3, 7] = cmbUom.Text;

                    worksheet.Cells[4, 1] = "Batch Size";
                    worksheet.Cells[4, 2] = txtbatchlot.Text;

                    worksheet.Cells[4, 3] = "Receipe Code";
                    worksheet.Cells[4, 4] = txtbatchlot.Text;


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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    Application.DoEvents();
                }


                Cursor.Current = Cursors.WaitCursor;
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataTable DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                string SheetName = "Sheet1";
                // string ExcellSheet = ;

                string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'");

                MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + SheetName + "$] ", MyConnection);
                //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
                MyCommand.TableMappings.Add("Table", filename);
                DtSet = new System.Data.DataTable();
                MyCommand.Fill(DtSet);
                int count = DtSet.Rows.Count;
                DataTable dt = new DataTable();
                System.Data.DataRow dr = null; //dataGrdView.Visible = true;
                                               //dataGrdView.DataSource = dtExcel;
                dt.Columns.Add(new DataColumn("Item_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Item_Name", typeof(string)));              
                dt.Columns.Add(new DataColumn("UOM", typeof(string)));
                dt.Columns.Add(new DataColumn("QtyReq", typeof(string)));
                for (int i = 0; i < count; i++)
                {
                    dr = dt.NewRow();
                    //txtProdID.Text = result.FirstOrDefault().Product_Code;
                    //dr["Prod_Code"] = result.FirstOrDefault().Product_Code; ;
                    dr["Item_Code"] = DtSet.Rows[i]["Prod_Name"].ToString();
                    dr["Item_Name"] = DtSet.Rows[i]["Prod_Type_Id"].ToString();                 
                    dr["UOM"] = DtSet.Rows[i]["Prod_Primary_UOM_Id"].ToString();
                    dr["QtyReq"] = DtSet.Rows[i]["Prod_Group_Id"].ToString();

                    dt.Rows.Add(dr);
                }

                dgbom.DataSource = dt;
                


                

                MessageBox.Show("Data Uploaded Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
