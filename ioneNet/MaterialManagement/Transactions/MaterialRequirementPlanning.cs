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
using System.Configuration;

namespace ioneNet
{
    public partial class MaterialRequirementPlanning : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        SqlCommand cmd;
        public static DataTable dtpr;
        public static DataTable dtpo;
        public static string RefNO = "";

        public static DateTime dt;
        public static string dt1;
        public static string PRName = "";
        public static string POName = "";
        public static DataTable GDT, SUMPODT, SUMMFGDT;
        public static int r = 0;
        public static int r1 = 0;
        public MaterialRequirementPlanning()
        {
            InitializeComponent();
        }

        public void AutoincrementId()
        {
            try
            {
                var auto = db.sp_autoincrement_MRP(AppCode.GlobalAccess.companyName);
                txtIndentNo.Text = auto.FirstOrDefault().MRP;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void clear()
        {
            try
            {
                foreach (Control x in this.Controls)
                {
                    foreach (Control d in GroupBox1.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is CheckBox)
                            (d as CheckBox).Checked = false;
                    }
                }

                {
                    for (int i = 0; i < dgIndent.Rows.Count - 1; i++)
                    {
                        dgIndent.Rows.RemoveAt(i);
                        i--;
                        while (dgIndent.Rows.Count == 0)
                            continue;
                    }

                    for (int i = 0; i < dgPO.Rows.Count - 1; i++)
                    {
                        dgPO.Rows.RemoveAt(i);
                        i--;
                        while (dgPO.Rows.Count == 0)
                            continue;
                    }
                    for (int i = 0; i < dgMFg.Rows.Count - 1; i++)
                    {
                        dgMFg.Rows.RemoveAt(i);
                        i--;
                        while (dgMFg.Rows.Count == 0)
                            continue;
                    }
                    //  dgProdOrd.Rows.Clear();
                    //dgProdOrd.DataSource = null;
                }
                txttotalQty.Text = "";
                AutoincrementId();
                //loadProductionOrders();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While  clear Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string GetmaterialcodeBymaterialName(string p_Name)
        {



            var prodid = (from s in db.ProdMasters where s.Product_Name == p_Name && s.Comp_Name == AppCode.GlobalAccess.companyName select s.Product_code).FirstOrDefault();
            string returnvalue = prodid.ToString();
            return returnvalue;

        }

        private void Save()

        {
            if ((from u in db.MRPs where u.RefNo == txtIndentNo.Text && u.CompName == AppCode.GlobalAccess.companyName select u).Count() > 0)
            {

                try
                {
                    if (AppCode.GlobalAccess.Edit == "Yes")
                    {


                        db.sp_delete_MRP(txtIndentNo.Text, AppCode.GlobalAccess.companyName);

                        for (int i = 0; i < dgIndent.Rows.Count - 1; i++)
                        {


                            MRP I = new MRP();

                            I.RefNo = txtIndentNo.Text;
                            I.Date = dpIndentDate.Value;
                            I.FinishedProductName = (txtProductName.Text == null) ? "" : txtProductName.Text;
                            I.Qty = (TxtQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(TxtQty.Text);
                            I.ProdCode = (dgIndent.Rows[i].Cells["ProdCode"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProdCode"].Value).ToString();
                            I.ProductName = (dgIndent.Rows[i].Cells["ProductName"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProductName"].Value).ToString();
                            I.Product_Type = (dgIndent.Rows[i].Cells["Product_Type"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Product_Type"].Value).ToString();
                         //   I.Grade = (dgIndent.Rows[i].Cells["Grade"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Grade"].Value).ToString();
                            I.Unit = (dgIndent.Rows[i].Cells["Unit"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Unit"].Value).ToString();

                            I.RequiredQty = (dgIndent.Rows[i].Cells["RequiredQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["RequiredQty"].Value);
                            I.StockInQty = (dgIndent.Rows[i].Cells["StockInQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["StockInQty"].Value);
                            I.ProcureQty = (dgIndent.Rows[i].Cells["ProcureQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["ProcureQty"].Value);
                            I.MfgQty = (dgIndent.Rows[i].Cells["MfgQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["MfgQty"].Value);
                            I.Min_Stock = (dgIndent.Rows[i].Cells["Min_Stock"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["Min_Stock"].Value);
                            I.TotalQty = (txttotalQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txttotalQty.Text);
                            //I.Remarks = (dgIndent.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Remarks"].Value).ToString();


                            I.ModifiedBy = AppCode.GlobalAccess.UserName;
                            I.ModifiedOn = Convert.ToDateTime(DateTime.Now.ToString());
                            I.CompName = AppCode.GlobalAccess.companyName;
                            db.MRPs.InsertOnSubmit(I);
                        }

                        db.SubmitChanges();
                        MessageBox.Show("Record Updated Successfully");
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
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
            else
            {
                try
                {

                    if (AppCode.GlobalAccess.Add == "Yes")
                    {


                        //if (dgProdOrdRpt.Rows.Count > 0)
                        //{
                        for (int i = 0; i < dgIndent.Rows.Count - 1; i++)
                        {


                            MRP I = new MRP();

                            I.RefNo = txtIndentNo.Text;
                            I.Date = dpIndentDate.Value;
                            I.FinishedProductName = (txtProductName.Text == null) ? "" : txtProductName.Text;
                            I.Qty = (TxtQty.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(TxtQty.Text);
                            I.ProdCode = (dgIndent.Rows[i].Cells["ProdCode"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProdCode"].Value).ToString();
                            I.ProductName = (dgIndent.Rows[i].Cells["ProductName"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProductName"].Value).ToString();
                            I.Product_Type = (dgIndent.Rows[i].Cells["Product_Type"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Product_Type"].Value).ToString();
                          //  I.Grade = (dgIndent.Rows[i].Cells["Grade"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Grade"].Value).ToString();
                            I.Unit = (dgIndent.Rows[i].Cells["Unit"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Unit"].Value).ToString();

                            I.RequiredQty = (dgIndent.Rows[i].Cells["RequiredQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["RequiredQty"].Value);
                            I.StockInQty = (dgIndent.Rows[i].Cells["StockInQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["StockInQty"].Value);
                            I.ProcureQty = (dgIndent.Rows[i].Cells["ProcureQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["ProcureQty"].Value);
                            I.MfgQty = (dgIndent.Rows[i].Cells["MfgQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["MfgQty"].Value);
                            I.Min_Stock = (dgIndent.Rows[i].Cells["Min_Stock"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["Min_Stock"].Value);
                           // I.TotalQty = (txttotalQty.Text == null) ? Convert.ToDecimal("0") : Convert.ToDecimal(txttotalQty.Text);


                            I.CreatedBy = AppCode.GlobalAccess.UserName;
                            I.CreatedOn = Convert.ToDateTime(DateTime.Now.ToString());
                            I.CompName = AppCode.GlobalAccess.companyName;
                            db.MRPs.InsertOnSubmit(I);

                        }
                        // }
                        //else
                        //{
                        //    MessageBox.Show("Test");
                        //}

                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");

                        clear();
                    }
                    else
                    {
                        MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
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
        }

        //private void loadProductionOrders()
        //{
        //     //var query = (from s in db.sp_ProductionOrderNoLoad(AppCode.GlobalAccess.companyName) select s).ToList();
        //    var f = (from s in db.ProOrders where s.CompName == AppCode.GlobalAccess.companyName select s.POrdNo).Distinct().ToList();
        //    if (f != null)
        //    {
        //        cmbProductionOrdNo.DataSource = f;
        //        cmbProductionOrdNo.DisplayMember = "pordno";
        //        if(cmbProductionOrdNo.Items.Count>0)
        //        cmbProductionOrdNo.SelectedIndex = -1;

        //    }

        //}

        private void ProductionReport_Load(object sender, EventArgs e)
        {
            //// TODO: This line of code loads data into the 'laksanaIndSysDataSet1.TempBomProcure' table. You can move, or remove it, as needed.
            //this.tempBomProcureTableAdapter1.Fill(this.laksanaIndSysDataSet1.TempBomProcure);
            //// TODO: This line of code loads data into the 'laksanaIndSysDataSet.TempBomProcure' table. You can move, or remove it, as needed.
            //this.tempBomProcureTableAdapter.Fill(this.laksanaIndSysDataSet.TempBomProcure);
            try
            {

                //var data = db.ProdMasters.Select(c => c.Product_Name).Distinct().ToArray();            
                //AutoCompleteStringCollection instcol = new AutoCompleteStringCollection();               
                //instcol.AddRange(data);
                //txtProductName.AutoCompleteCustomSource = instcol;


                txtProductName.AutoCompleteMode = AutoCompleteMode.Suggest;
                txtProductName.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
                getData(DataCollection);
                txtProductName.AutoCompleteCustomSource = DataCollection;




                /// Data Table For Main Grid

                GDT = new DataTable();
                GDT.Columns.Add("ProdCode");
                GDT.Columns.Add("ProductName");
                GDT.Columns.Add("Product_Type");
                GDT.Columns.Add("Unit");
                GDT.Columns.Add("RequiredQty");
                //GDT.Columns.Add("Grade");
                GDT.Columns.Add("StockInQty");
                GDT.Columns.Add("ProcureQty");
                GDT.Columns.Add("MfgQty");
                GDT.Columns.Add("Min_Stock");

                ///  Data Table For Summery of General Items

                SUMPODT = new DataTable();
                SUMPODT.Columns.Add("ProdCode");
                SUMPODT.Columns.Add("ProductName");
                SUMPODT.Columns.Add("Product_Type");
                SUMPODT.Columns.Add("Unit");
                SUMPODT.Columns.Add("RequiredQty");
                //GDT.Columns.Add("Grade");
                SUMPODT.Columns.Add("StockInQty");
                SUMPODT.Columns.Add("ProcureQty");
                SUMPODT.Columns.Add("MfgQty");
                SUMPODT.Columns.Add("Min_Stock");

                ///  Data Table For Summery of Semifinisged Goods

                SUMMFGDT = new DataTable();
                SUMMFGDT.Columns.Add("ProdCode");
                SUMMFGDT.Columns.Add("ProductName");
                SUMMFGDT.Columns.Add("Product_Type");
                SUMMFGDT.Columns.Add("Unit");
                SUMMFGDT.Columns.Add("RequiredQty");
                //GDT.Columns.Add("Grade");
                SUMMFGDT.Columns.Add("StockInQty");
                SUMMFGDT.Columns.Add("ProcureQty");
                SUMMFGDT.Columns.Add("MfgQty");
                SUMMFGDT.Columns.Add("Min_Stock");


                pictureBox1.Image = AppCode.GlobalAccess.comylogo;
                if (RawMaterialIndentReportExcel.name == "IndentReport")
                {
                    Search();
                }
                else
                {
                    AutoincrementId();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        string comp, Type;
        private void getData(AutoCompleteStringCollection dataCollection)
        {
            try
            {
                SqlCommand cmd;

               
                DataSet ds = new DataSet();
                comp = AppCode.GlobalAccess.companyName;
                Type = "Finished Goods";
               // string query = string.Format("SELECT DISTINCT Product_Name FROM ProdMaster where Comp_Name= '{0}'", comp);
                string query = string.Format("SELECT DISTINCT Product_Name FROM ProdMaster");
                con.Open();
                cmd = new SqlCommand(query, con);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);                
                dap.Fill(ds);
                
                con.Close();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dataCollection.Add(row[0].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void cmbProductionOrdNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProdOrdRpt_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIndentNo.Text == "")
                {
                    MessageBox.Show("Please Enter Indent", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtIndentNo.Focus();
                    return;
                }
                else if (txtProductName.Text == "")
                {
                    MessageBox.Show("Please Select ProductName", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtProductName.Focus();
                    return;
                }
                else if (TxtQty.Text == "")
                {
                    MessageBox.Show("Please Select Quantity", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TxtQty.Focus();
                    return;
                }
                else if (dgIndent.Rows.Count == 1)
                {
                    MessageBox.Show("Please Enter Atleast one record in Grid");
                    btnGenerate.Focus();
                    return;
                }

                Save();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }


        }

        private void Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && Convert.ToInt32(e.KeyChar) != 13 && Convert.ToInt32(e.KeyChar) != 8 && Convert.ToInt32(e.KeyChar) != 46)
            {
                e.Handled = true;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManagement.MRPSearch obj = new OrderManagement.MRPSearch();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtIndentNo.Text = OrderManagement.MRPSearch.Indentno;
                    dpIndentDate.Text = OrderManagement.MRPSearch.Date;
                  
                    if (!string.IsNullOrEmpty(txtIndentNo.Text))
                    {
                        var dm1 = (from s in db.MRPs
                                   where s.RefNo == txtIndentNo.Text && s.CompName == AppCode.GlobalAccess.companyName
                                   select new
                                   { s.ProdCode, s.ProductName, s.Grade, s.Unit, s.RequiredQty, s.StockInQty, s.ProcureQty,s.MfgQty, s.Remarks });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgIndent.DataSource = dtr;
                        gridtotal();

                        var aa=(from sa in db.MRPs
                                where sa.RefNo == txtIndentNo.Text && sa.CompName == AppCode.GlobalAccess.companyName
                                select new
                                {sa.FinishedProductName,sa.Qty }).ToList();
                        if(aa.Count>0)
                        {
                            txtProductName.Text = aa[0].FinishedProductName;
                            TxtQty.Text = Convert.ToDecimal(aa[0].Qty).ToString();
                        }

                    }
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Search()
        {

            try
            {
                txtIndentNo.Text = OrderManagement.MaterialIndentSearch.Indentno;


                if (!string.IsNullOrEmpty(txtIndentNo.Text))
                {
                    var dm1 = (from s in db.Indents
                               where s.IndentNo == txtIndentNo.Text && s.CompName == AppCode.GlobalAccess.companyName
                               select new
                               { s.ProductName, s.RequiredQty, s.StockInQty, s.ProcureQty, s.Remarks });

                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgIndent.DataSource = dtr;



                    var f = (from p in db.Indents where p.IndentNo == txtIndentNo.Text && p.CompName == AppCode.GlobalAccess.companyName select p).FirstOrDefault();
                    if (f != null)
                    {

                        dpIndentDate.Text = f.IndentDate.ToString();

                        txtPordNo.Text = f.OrdNo;


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (AppCode.GlobalAccess.Edit == "Yes")
                {
                    if (txtIndentNo.Text != "")
                    {
                        if ((from u in db.MRPs where u.RefNo == txtIndentNo.Text && u.CompName == AppCode.GlobalAccess.companyName select u).Count() > 0)
                        {
                            if ((from u in db.sp_delete_MRP(AppCode.GlobalAccess.companyName, txtIndentNo.Text) select u).Count() == 0)
                            {

                                var result = MessageBox.Show("Are You Sure Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (result == DialogResult.Yes)
                                {

                                    db.sp_delete_MRPData(AppCode.GlobalAccess.companyName, txtIndentNo.Text);
                                    MessageBox.Show("Record Deleted Successfully");

                                    clear();


                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("The Record Cannot Be deleted because Indent No is Already In Use");


                                clear();

                            }
                        }
                        else
                        {
                            MessageBox.Show("This Record Not Exist,Please Try Another Record");
                            clear();

                        }
                    }


                    else
                    {
                        MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgIndent_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                string title = dgIndent.Columns["Grade"].HeaderText;
                if (title.Equals("Grade"))
                {
                    TextBox tb = e.Control as TextBox;
                    if (tb != null)
                    {
                        tb.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb.AutoCompleteCustomSource = DataColl;
                    }
                }

                //e.Control.KeyPress -= new KeyPressEventHandler(Column4_KeyPress);

                //if (dgProductData.Rows[dgProductData.CurrentRow.Index].Cells["Qty"].Value != "" && dgProductData.Rows[dgProductData.CurrentRow.Index].Cells["Qty"].Value != DBNull.Value) //Desired Column
                //{
                //    TextBox tb = e.Control as TextBox;
                //    if (tb != null)
                //    {
                //        tb.KeyPress += new KeyPressEventHandler(Column4_KeyPress);
                //    }
                //}




                //DataGridViewRow R1 = dgIndent.Rows[dgIndent.CurrentRow.Index];
                //e.Control.KeyPress -= new KeyPressEventHandler(Quantity_KeyPress);
                //if (dgIndent.Rows[dgIndent.CurrentRow.Index].Cells["Qty"].Value != "" && dgIndent.Rows[dgIndent.CurrentRow.Index].Cells["Qty"].Value != DBNull.Value) //Desired Column
                //    if (dgIndent.CurrentCell.ColumnIndex == 0|| dgIndent.CurrentCell.ColumnIndex == 1 )
                //{
                //    DataGridViewRow R = dgIndent.Rows[dgIndent.CurrentRow.Index];

                //    TextBox tb = e.Control as TextBox;
                //    if (tb != null)
                //    {
                //        tb.KeyPress += new KeyPressEventHandler(Quantity_KeyPress);
                //    }
                //}
                //e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
                //if (dgIndent.CurrentCell.ColumnIndex == 0 || dgIndent.CurrentCell.ColumnIndex == 1 ) //Desired Column
                //{
                //    TextBox tb = e.Control as TextBox;
                //    if (tb != null)
                //    {
                //        tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                //    }
                //}
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            }
        }


        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgIndent.Rows[dgIndent.CurrentRow.Index];
                //if (dgIndent.Rows[dgIndent.CurrentRow.Index].Cells["Grade"].Value.ToString() != "" && dgIndent.Rows[dgIndent.CurrentRow.Index].Cells["Grade"].Value != DBNull.Value) //Desired Column
                //if (dgIndent.CurrentCell.ColumnIndex == 0)
                string title = dgIndent.Columns["Grade"].HeaderText;
                if (title.Equals("Grade"))
                {
                    if (R1.Cells["ProdCode"].Value != null)
                    {
                        //var grade = (from d in db.OpeningStocks where d.Compname == AppCode.GlobalAccess.companyName && d.Prod_Code == R1.Cells["ProdCode"].Value.ToString() select new { d.Grade }).ToList();
                        var grade = (from d in db.Grade_Masters  select new { d.Grade }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Grade");
                        foreach (var item in grade)
                        {

                            dt.Rows.Add(item.Grade);
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


        public void clearbeforenew()
        {
            for (int i = 0; i < dgIndent.Rows.Count - 1; i++)
            {
                dgIndent.Rows.RemoveAt(i);
                i--;
                while (dgIndent.Rows.Count == 0)
                    continue;
            }

            for (int i = 0; i < dgPO.Rows.Count - 1; i++)
            {
                dgPO.Rows.RemoveAt(i);
                i--;
                while (dgPO.Rows.Count == 0)
                    continue;
            }
            for (int i = 0; i < dgMFg.Rows.Count - 1; i++)
            {
                dgMFg.Rows.RemoveAt(i);
                i--;
                while (dgMFg.Rows.Count == 0)
                    continue;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clearbeforenew();
                if (!string.IsNullOrEmpty(txtProductName.Text))
                {
                    DateTime f = dpIndentDate.Value;
                    string f1 = f.ToString("yyyy/MM/dd");
                   //var V1 = db.BOMs.Where(p => p.Prod_Code != null).Max(x => x.versionno);
                    var getProductName = db.Sp_MRPMainGeneration(AppCode.GlobalAccess.companyName, txtProductName.Text, Convert.ToDecimal(TxtQty.Text)).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("ProdCode");
                    dt.Columns.Add("ProductName");
                    dt.Columns.Add("Product_Type");
                    dt.Columns.Add("Unit");
                    dt.Columns.Add("RequiredQty");
                    //dt.Columns.Add("Grade");
                    dt.Columns.Add("StockInQty");
                    dt.Columns.Add("ProcureQty");
                    dt.Columns.Add("MfgQty");
                    dt.Columns.Add("Min_Stock");
                    //dt.Columns.Add("SubRMCode");
                    //dt.Columns.Add("Subitem");
                    //dt.Columns.Add("SubUnit");
                    //dt.Columns.Add("SubQtyReq");
                    //dt.Columns.Add("SubProduct_type");

                    foreach (var item in getProductName)
                    {

                        if (item.Product_type == "Semi Finished Goods")
                        {
                            var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores SFG") select data.OP).FirstOrDefault();
                            decimal de = s;
                            decimal qty = Convert.ToDecimal(item.QtyReq);
                            decimal mfg = (qty - de);
                            if (mfg < 0)
                            {
                                mfg = 0;
                            }

                            dt.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, "0.00", mfg, item.Min_Stock);
                        }

                        else if (item.Product_type == "General Items")
                        {
                            var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores RM") select data.OP).FirstOrDefault();
                            decimal de = s;
                            decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                            if (PRQ < 0)
                            {
                                PRQ = 0;
                            }
                            dt.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                        }

                        else if (item.Product_type == "Packing Material")
                        {
                            var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores RM") select data.OP).FirstOrDefault();
                            decimal de = s;
                            decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                            if (PRQ < 0)
                            {
                                PRQ = 0;
                            }
                            dt.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                        }

                        else if (item.Product_type == "Bought-Out Item")
                        {
                            var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores B/O") select data.OP).FirstOrDefault();
                            decimal de = s;
                            decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                            if (PRQ < 0)
                            {
                                PRQ = 0;
                            }
                            dt.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                        }
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GDT.Rows.Add(dt.Rows[i]["ProdCode"].ToString(), dt.Rows[i]["ProductName"].ToString(), dt.Rows[i]["Product_Type"].ToString(), dt.Rows[i]["Unit"].ToString(), dt.Rows[i]["RequiredQty"].ToString(), dt.Rows[i]["StockInQty"].ToString(), dt.Rows[i]["ProcureQty"].ToString(), dt.Rows[i]["MfgQty"].ToString(), dt.Rows[i]["Min_Stock"].ToString());

                        var getProductName1 = db.Sp_MRPMainGeneration(AppCode.GlobalAccess.companyName, dt.Rows[i]["ProductName"].ToString(), Convert.ToDecimal(dt.Rows[i]["RequiredQty"].ToString())).ToList();

                        foreach (var item in getProductName1)
                        {

                            if (item.Product_type == "Semi Finished Goods")
                            {
                                var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores SFG") select data.OP).FirstOrDefault();
                                decimal de = s;

                                decimal qty = Convert.ToDecimal(item.QtyReq);
                                decimal mfg = (qty - de);
                                if (mfg < 0)
                                {
                                    mfg = 0;
                                }
                                GDT.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, "0.00", mfg, item.Min_Stock);
                            }
                            else if (item.Product_type == "General Items")
                            {
                                var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores RM") select data.OP).FirstOrDefault();
                                decimal de = s;
                                decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                                if (PRQ < 0)
                                {
                                    PRQ = 0;
                                }
                                GDT.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                            }

                            else if (item.Product_type == "Packing Material")
                            {
                                var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores RM") select data.OP).FirstOrDefault();
                                decimal de = s;
                                decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                                if (PRQ < 0)
                                {
                                    PRQ = 0;
                                }
                                GDT.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                            }

                            else if (item.Product_type == "Bought-Out Item")
                            {
                                var s = (from data in db.sp_BinWise_Currentstock(AppCode.GlobalAccess.companyName, Convert.ToDateTime(f1), item.ProdName, "Stores B/O") select data.OP).FirstOrDefault();
                                decimal de = s;
                                decimal PRQ = Convert.ToDecimal(item.QtyReq - de);
                                if (PRQ < 0)
                                {
                                    PRQ = 0;
                                }
                                GDT.Rows.Add(item.RMCode, item.ProdName, item.Product_type, item.Unit, item.QtyReq, de, PRQ, "0.00", item.Min_Stock);
                            }
                        }
                    }

                    dgIndent.DataSource = GDT;
                    db.Sp_DeleteTempBOM();
                    for (int i = 0; i < dgIndent.Rows.Count - 1; i++)
                    {
                        TempBOM I = new TempBOM();
                        I.ProdCode = (dgIndent.Rows[i].Cells["ProdCode"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProdCode"].Value).ToString();
                        I.ProdName = (dgIndent.Rows[i].Cells["ProductName"].Value == null) ? "" : (dgIndent.Rows[i].Cells["ProductName"].Value).ToString();
                        I.ProdType = (dgIndent.Rows[i].Cells["Product_Type"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Product_Type"].Value).ToString();                      
                        I.Unit = (dgIndent.Rows[i].Cells["Unit"].Value == null) ? "" : (dgIndent.Rows[i].Cells["Unit"].Value).ToString();
                        I.RequiredQty = (dgIndent.Rows[i].Cells["RequiredQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["RequiredQty"].Value);                       
                        I.ProcureQty = (dgIndent.Rows[i].Cells["ProcureQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["ProcureQty"].Value);
                        I.MfgQty = (dgIndent.Rows[i].Cells["MfgQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgIndent.Rows[i].Cells["MfgQty"].Value);

                        db.TempBOMs.InsertOnSubmit(I);
                    }

                    db.SubmitChanges();

                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message);
            }
        }
        public void gridtotal()
        {
            try
            {
                double Qty = 0;

                for (int j = 0; j < dgIndent.Rows.Count; j++)
                {
                    if (dgIndent.Rows[j].Cells["RequiredQty"].Value != null)
                    {
                        Qty += (dgIndent.Rows[j].Cells["RequiredQty"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgIndent.Rows[j].Cells["RequiredQty"].Value);
                    }
                }
                txttotalQty.Text = Qty.ToString(".00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void dgIndent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgIndent.Rows[dgIndent.CurrentRow.Index];

                //if (R1.Cells["Grade"].Value != "" )
                //    if (R1.Cells["Grade"].Value != null && R1.Cells["Grade"].Value != DBNull.Value && R1.Cells["ProdCode"].Value != null && R1.Cells["ProdCode"].Value != DBNull.Value )
                //    {
                //    var ds = (from c in db.sp_QtyinAtock(AppCode.GlobalAccess.companyName, R1.Cells["Grade"].Value.ToString(), R1.Cells["ProdCode"].Value.ToString(), Convert.ToDecimal( R1.Cells["RequiredQty"].Value.ToString())) select c).ToList();

                //    if (ds.Count > 0)
                //    {
                //        R1.Cells["StockInQty"].Value = ds[0].QtyinStock;
                //        R1.Cells["ProcureQty"].Value = ds[0].QtytoProcure;
                //        //R1.Cells["LastThreeMonthsConsumption"].Value = ds[0].Issue_Qty_3Month;
                //    }
                //}
                //decimal Requiredqty, Stockqty, Procureqty = 0;

                //Requiredqty = (R1.Cells["RequiredQty"].Value == null || R1.Cells["RequiredQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["RequiredQty"].Value);
                //Stockqty = (R1.Cells["StockInQty"].Value == null || R1.Cells["StockInQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["StockInQty"].Value);

                //Procureqty = Requiredqty - Stockqty;
                //R1.Cells["ProcureQty"].Value = Procureqty;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dpIndentDate_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (Convert.ToDateTime(dpIndentDate.Value) > Convert.ToDateTime(DateTime.Now.ToString()))
                //{
                //    MessageBox.Show("Date Should not be Greater than Current Date");
                //    dpIndentDate.Focus();
                //    return;
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }


        private void BtnGetOrders_Click(object sender, EventArgs e)
        {
            try
            {
                dt = dpIndentDate.Value;
                dt1 = dt.ToString("yyyy/MM/dd");
                ProductionManagement.GetProductionOrder obj = new ProductionManagement.GetProductionOrder();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    DataTable dts = ProductionManagement.GetProductionOrder.dt;

                    // DataTable dt = new DataTable();
                    string str = string.Empty;
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        str = str + dts.Rows[i]["POrdNo"].ToString();
                        str += (i < dts.Rows.Count - 1) ? "," : string.Empty;
                    }

                    txtPordNo.Text = str.ToString();
                    //string StrModules;

                    //foreach (var p in dts.Columns)
                    //{

                    //        StrModules += li.Text + ",";
                    //        StrModules1 += li.Value + ",";



                    //}
                    //StrModules1 = StrModules1.Trim(",".ToCharArray());
                    //StrModules = StrModules.Trim(",".ToCharArray());
                    //    string str = "";
                    //    StringBuilder ProordNo = new StringBuilder();


                    //    if (dts.Rows.Count > 0)
                    //    {


                    //        if (string.IsNullOrEmpty(str))
                    //        {
                    //            str = Convert.ToString((ProordNo.Append(dts.Rows.ToString())));
                    //        }
                    //        else
                    //        {
                    //            str = Convert.ToString((ProordNo.Append(',').Append(dts.Rows[0]["POrdNo"].ToString())));
                    //        }
                    //    }

                    //    txtPordNo.Text = str.ToString();
                    //}
                    //else
                    //{
                    //    txtPordNo.Text = "";
                    //}
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            try
            {
                RefNO = txtIndentNo.Text;
                POName = "MRP";
                dtpo = new DataTable();

                dtpo.Columns.Add("ProdCode", typeof(string));
                dtpo.Columns.Add("ProductName", typeof(string));
                dtpo.Columns.Add("Product_Category", typeof(string));
                dtpo.Columns.Add("UOM", typeof(string));
                dtpo.Columns.Add("PlannedQty", typeof(decimal));
                //dtpr.Columns.Add("Requiredon", typeof(string));
                //dtpr.Columns.Add("PurchasePrice", typeof(decimal));
                //dtpr.Columns.Add("LastPurchaseDate", typeof(string));

                for (int i = 0; i < dgMFg.Rows.Count - 1; i++)
                {                   
                        string ProdCode = dgMFg.Rows[i].Cells["ProdCode"].Value.ToString();
                        string ProductName = dgMFg.Rows[i].Cells["prodname"].Value.ToString();
                        string Product_Category = dgMFg.Rows[i].Cells["prodtype"].Value.ToString();
                        string UOM = dgMFg.Rows[i].Cells["Unit"].Value.ToString();
                        decimal PlannedQty = Convert.ToDecimal(dgMFg.Rows[i].Cells["MfgQty"].Value);
                        dtpo.Rows.Add(ProdCode, ProductName, Product_Category, UOM, PlannedQty);                  
                }

                OrderManagement.ProductionOrder obj = new OrderManagement.ProductionOrder();
                obj.Show();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnPR_Click(object sender, EventArgs e)
         {
            try
            {
                PRName = "MRP";
                RefNO = txtIndentNo.Text;
                dtpr = new DataTable();

                dtpr.Columns.Add("Product_code", typeof(string));
                dtpr.Columns.Add("Product_Name", typeof(string));
                dtpr.Columns.Add("Product_Descr", typeof(string));
                dtpr.Columns.Add("unit_purchase", typeof(string));
                dtpr.Columns.Add("Qty", typeof(decimal));
                dtpr.Columns.Add("Requiredon", typeof(string));
                dtpr.Columns.Add("PurchasePrice", typeof(decimal));
                dtpr.Columns.Add("LastPurchaseDate", typeof(string));

                for (int i = 0; i < dgPO.Rows.Count - 1; i++)
                {
                       string Product_code = dgPO.Rows[i].Cells["ProdCode"].Value.ToString();
                        string Product_Name = dgPO.Rows[i].Cells["ProdName"].Value.ToString();
                        string Product_Descr = "";
                        string unit_purchase = dgPO.Rows[i].Cells["Unit"].Value.ToString();
                        decimal Qty = Convert.ToDecimal(dgPO.Rows[i].Cells["ProcureQty"].Value);
                        string Requiredon = "";
                        decimal PurchasePrice = Convert.ToDecimal("0.00");

                        string LastPurchaseDate = "";


                        dtpr.Rows.Add(Product_code, Product_Name, Product_Descr, unit_purchase, Qty, Requiredon, PurchasePrice, LastPurchaseDate);
                 


                }


                MaterialManagement.Purchase_Requisition obj = new MaterialManagement.Purchase_Requisition();
                obj.Show();


            }
            catch (Exception ex)
            {


            }
        } 

        private void GenerateMfgItems_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("select prodcode,prodname,unit,prodtype,MfgQty from TempBomProcure where MfgQty >0", con);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                dgMFg.DataSource = dt;

            }
            catch (Exception ex)
            {

              
            }
        }

        private void btnGenetePOItems_Click(object sender, EventArgs e)
        {
            try
            {


                SqlCommand cmd = new SqlCommand("select prodcode,prodname,unit,prodtype,procureQty from TempBomProcure where ProcureQty >0", con);
            
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                dgPO.DataSource = dt;


            }
            catch (Exception ex)
            {


            }
        }

    
      


    }
}
