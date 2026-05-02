using Ione_DAL;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
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
    public partial class frmforge_Production_Scheduler : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SO_No, SO_ID, SO_Item_No, SO_Item_Code, SO_Item_Name, SO_Qty, del_date;
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public frmforge_Production_Scheduler()
        {
            InitializeComponent();
        }
        private DateTimePicker dtp { get; set; }

        private class User
        {
            public string Name { get; set; }
            public DateTime Available_Date { get; set; }
        }
        private void frmforge_Production_Scheduler_Load(object sender, EventArgs e)
        {

            //Add Machines
            using (SqlCommand cmd = new SqlCommand("SELECT distinct [Machine_Name],Machine_Type FROM [Forging_MachineMaster] where [company_id] = @CompID order by Machine_Type", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            checkedListBox1.Items.Add(dt.Rows[i]["Machine_Name"].ToString());
                        }

                    }
                }
            }


            var d = (from data in db.SP_Get_ForgeSOrdersToSchedule(logIn.company) select data).ToList();

            if (d.Count > 0)
            {
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Ord_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Ord_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Ord_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Ord_No"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["Cust_PO_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Cust_PO_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Cust_PO_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Cust_PO_No"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                //string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Qty_Alloted"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    // DateTime deldate = Convert.ToDateTime(cellVaue.ToString());

                    if (Convert.ToDecimal(cellVaue) > 0)
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 1), Color.Blue);
                    }
                    if (Convert.ToDecimal(cellVaue) <= 0)
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 1), Color.Orange);
                    }
               
                }


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void scheduleTheOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
            //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
            //string columnName = dgProducts.Columns[columnIndex].Name;

            int i = sfDataGrid1.CurrentCell.RowIndex;
            // int k = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var AllotedStock = sfDataGrid1.Columns["Qty_Alloted"].MappingName;
            var AStock = (rowData.GetType().GetProperty(AllotedStock).GetValue(rowData, null).ToString());
            if(Convert.ToDecimal(AStock)<=0)
            {
                MessageBox.Show("Order Cannot Be Scheduled unless Raw Material is Alloted");
                return;
            }

            var mappingName = sfDataGrid1.Columns["Ord_No"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
            var mappingName2 = sfDataGrid1.Columns["S_No"].MappingName;
            var mappingName3 = sfDataGrid1.Columns["Prod_Code"].MappingName;
            var mappingName4 = sfDataGrid1.Columns["Item_Description"].MappingName;
            var mappingName5 = sfDataGrid1.Columns["Ord_Qty"].MappingName;
            var mappingName6 = sfDataGrid1.Columns["Delivery_Date"].MappingName;
            var cellvalue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            txtOrdNo.Text =cellvalue.ToString();
            var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            txtOrdID.Text = cellvalue1.ToString();
            var cellvalue2 =  (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            txtItemSno.Text = cellvalue2.ToString();

             var cellvalue3 =  (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            txtItemCode.Text = cellvalue3.ToString();
            //txtOrdNo.Text = R1.Cells["Ord_No"].Value.ToString();
            //txtOrdID.Text = R1.Cells["SO_Master_ID"].Value.ToString();
            //txtItemSno.Text = R1.Cells["S_No"].Value.ToString();
            //txtItemCode.Text = R1.Cells["Prod_Code"].Value.ToString();
            //SO_Item_Name = R1.Cells["Item_Description"].Value.ToString();
            //SO_Qty = R1.Cells["Ord_Qty"].Value.ToString();
            //del_date = R1.Cells["Delivery_Date"].Value.ToString();

            var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Forging" && d.Company_ID == logIn.company select new {d.ID, d.Machine_Name }).ToList();
            if (Prodname.Count > 0)
            {
                cmbMachine.DataSource = Prodname;
                cmbMachine.DisplayMember = "Machine_Name";
                cmbMachine.ValueMember = "ID";
                if (cmbMachine.Items.Count > 0)
                {
                    cmbMachine.SelectedIndex = -1;
                }
                else
                {
                    cmbMachine.SelectedIndex = -1;
                }
            }


            groupBox1.Visible = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
               
                forge_Scheduled_Order ci = new forge_Scheduled_Order();
                ci.SO_Master_ID = Convert.ToInt32(txtOrdID.Text);
                ci.So_item_No = Convert.ToInt32(txtItemSno.Text);
                ci.Start_Date = dtStartDate.Value;
                ci.End_Date = dtEndDate.Value;
                ci.Machine = Convert.ToInt32(cmbMachine.SelectedValue);
                ci.Prod_Shift = (cmbShift.Text == "") ? "" : cmbShift.Text;          
                ci.Company_ID = logIn.company;
                ci.Created_By = logIn.username;
                ci.Modified_BY = logIn.username;
                db.forge_Scheduled_Orders.InsertOnSubmit(ci);
                db.SubmitChanges();
                MessageBox.Show("Schedule Updated Successfully");
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void cmbMachine_Leave(object sender, EventArgs e)
        {
            //if (cmbMachine.Text != "")
            //{
            //    DateTime dt = DateTime.Now;
            //    DateTime dt1 = dt.AddDays(30);
            //    //string dt1 = dt.ToString("yyyy/MM/dd");

            //    var getAvblDate = (from s in db.Get_MachineAvailabilityDate(logIn.company,dt,dt1,Convert.ToInt32(cmbMachine.SelectedValue),cmbShift.Text)
            //                          select new { s.Availble_Date}).FirstOrDefault();

            //    if (getAvblDate != null)
            //    {
                   
            //         dateTimePicker1.Text = getAvblDate.Availble_Date.ToString();
                   
            //    }
            //}
        }

        private void dtStartDate_Leave(object sender, EventArgs e)
        {
            if(dtStartDate.Value < dateTimePicker1.Value)
            {
                MessageBox.Show("The Start Date Should Be Greater than Machine Available Date");
                dtStartDate.Focus();
            }
            else
            {
                if (cmbMachine.Text != "")
                {
                    if (cmbShift.Text != "")
                    {
                        //DateTime dt = DateTime.Now;
                        //DateTime dt1 = dt.AddDays(30);
                        //string dt2 = dt.ToString("yyyy/MM/dd");
                        //string dt3 = dt1.ToString("yyyy/MM/dd");


                        //var getAvblDate = (from s in db.Get_MachineAvailabilityDate(logIn.company, Convert.ToDateTime(dt2), Convert.ToDateTime(dt3), Convert.ToInt32(cmbMachine.SelectedValue), cmbShift.Text)
                        //                   select new { s.Avbl_Date, s.AvblHrs }).FirstOrDefault();

                        //if (getAvblDate != null)
                        //{

                        //    dateTimePicker1.Text = getAvblDate.Avbl_Date.ToString();
                        //    textBox1.Text = getAvblDate.AvblHrs.ToString();

                        //}
                    }
                }
            }

        }

        private void dtEndDate_Leave(object sender, EventArgs e)
        {
            try
            {
                if (dtEndDate.Value < dtStartDate.Value)
                {
                    MessageBox.Show("The End Date Should Be Greater than Start Date");
                    dtEndDate.Focus();
                }

                DateTime dob = dtStartDate.Value;
                DateTime dtToday = dtEndDate.Value;
                TimeSpan diffResult = dtToday.Subtract(dob); // - dob;
                Decimal diffhrs = ((diffResult.Hours * 60 + (diffResult.Minutes)));
                txtDuration.Text = Math.Round((diffhrs / 60),2).ToString() ;
                //if (Convert.ToInt32(txtAgeinYears.Text) < 18)
                //{
                //    MessageBox.Show("Employee Age Must Be Above 18 Years");
                //    dtDOB.Focus();
                //    return;
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = tabControl1.SelectedTab;
            int selectedIndex = tabControl1.SelectedIndex;
            if (selectedIndex == 0)
            {
                //Get Orders To Schedule
                //Get Orders To Schedule
                var d = (from data in db.SP_Get_ForgeSOrdersToSchedule(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Ord_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Ord_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Ord_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Ord_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Cust_PO_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Cust_PO_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Cust_PO_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Cust_PO_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                    //string cellValue;
                    for (int i = 2; i < sfDataGrid1.RowCount; i++)
                    {
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Qty_Alloted"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        // DateTime deldate = Convert.ToDateTime(cellVaue.ToString());

                        if (Convert.ToDecimal(cellVaue) > 0)
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 1), Color.Blue);
                        }
                        if (Convert.ToDecimal(cellVaue) <= 0)
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 1), Color.Orange);
                        }

                    }

                }            
            }
            else
            {
                //Get Scheduled Orders
                var d = (from data in db.SP_Get_ForgeSOrdersScheduled(logIn.company) select data).ToList();

                if (d.Count > 0)
                {
                    sfDataGrid2.DataSource = d;
                    this.sfDataGrid2.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid2.Columns["Ord_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid2.Columns["Ord_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid2.Columns["Ord_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid2.Columns["Ord_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid2.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid2.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid2.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid2.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid2.Columns["Cust_PO_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid2.Columns["Cust_PO_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid2.Columns["Cust_PO_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid2.Columns["Cust_PO_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid2.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid2.Columns["Prod_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid2.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid2.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid2.QueryCellStyle += sfDataGrid_QueryCellStyle;
                    //string cellValue;
                    for (int i = 2; i < sfDataGrid2.RowCount; i++)
                    {
                        var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid2.Columns["CDD"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                         DateTime deldate = Convert.ToDateTime(cellVaue.ToString());

                        var mappingName1 = sfDataGrid2.Columns["EDD"].MappingName;
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        DateTime EDD = Convert.ToDateTime(cellVaue1.ToString());

                        if (EDD > deldate)
                        {
                            SetCellBackgroundColorGRID2(new RowColumnIndex(i, 1), Color.Red);
                        }
                       else
                        {
                            SetCellBackgroundColorGRID2(new RowColumnIndex(i, 1), Color.Green);
                        }

                    }

                }
            }
        }
         void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }

        private void cmbShift_Leave(object sender, EventArgs e)
        {
            if (cmbMachine.Text != "")
            {
                if (cmbShift.Text != "")
                {
                    DateTime dt = DateTime.Now;
                    DateTime dt1 = dt.AddDays(30);
                    string dt2 = dt.ToString("yyyy/MM/dd");
                    string dt3 = dt1.ToString("yyyy/MM/dd");


                    var getAvblDate = (from s in db.Get_MachineAvailabilityDate(logIn.company,Convert.ToDateTime(dt2), Convert.ToDateTime(dt3), Convert.ToInt32(cmbMachine.SelectedValue), cmbShift.Text)
                                       select new { s.Avbl_Date, s.AvblHrs }).FirstOrDefault();

                    if (getAvblDate != null)
                    {

                        dateTimePicker1.Text = getAvblDate.Avbl_Date.ToString();
                        textBox1.Text = getAvblDate.AvblHrs.ToString();

                    }
                }
            }
        }

        private void sfDataGrid1_CellClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            //try
            //{
            //    //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
            //    //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
            //    //string columnName = dgProducts.Columns[columnIndex].Name;
            //    DataTable dt = new DataTable();
            //int i = sfDataGrid1.CurrentCell.RowIndex;
            //// int k = sfDataGrid1.CurrentCell.RowIndex;
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //var SONo = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
            //var SO_ID = (rowData.GetType().GetProperty(SONo).GetValue(rowData, null).ToString());

            //var Itemno = sfDataGrid1.Columns["S_No"].MappingName;
            //var itemID = (rowData.GetType().GetProperty(Itemno).GetValue(rowData, null).ToString());
            //SqlCommand com = new SqlCommand("SP_Get_Process_SOItemWise", con);
            //com.Parameters.AddWithValue("@compname", logIn.company);
            //com.Parameters.AddWithValue("@so_master_Id",Convert.ToInt32(SO_ID));
            //com.Parameters.AddWithValue("@so_S_no", Convert.ToInt32(itemID));
           
            //com.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(com);

            //con.Open();
            //da.Fill(dt);
            //if (dt.Rows.Count > 0)
            //{
            //    dataGridView1.DataSource = dt;

            //}
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void sfDataGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void sfDataGrid1_SelectionChanged(object sender, Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs e)
        {
            try
            {
                //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                //string columnName = dgProducts.Columns[columnIndex].Name;
                DataTable dt = new DataTable();
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i > 1)
                {
                    // int k = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var SONo = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
                    var SO_ID = (rowData.GetType().GetProperty(SONo).GetValue(rowData, null).ToString());

                    var Itemno = sfDataGrid1.Columns["S_No"].MappingName;
                    var itemID = (rowData.GetType().GetProperty(Itemno).GetValue(rowData, null).ToString());
                    SqlCommand com = new SqlCommand("SP_Get_Process_Sheduled_Orders", con);
                    com.Parameters.AddWithValue("@compname", logIn.company);
                    com.Parameters.AddWithValue("@so_master_Id", Convert.ToInt32(SO_ID));
                    com.Parameters.AddWithValue("@so_S_no", Convert.ToInt32(itemID));

                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(com);

                    con.Open();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;

                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                if (tb3 != null && columnName == "Machine")
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
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;

                if (columnName == "Machine")
                {
                    var Prodname = (from d in db.Forging_MachineMasters select new { d.Machine_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Machine_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Machine_Name);
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

        private void updateProcurementStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
            //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
            //string columnName = dgProducts.Columns[columnIndex].Name;

            int i = sfDataGrid1.CurrentCell.RowIndex;
            // int k = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var AllotedStock = sfDataGrid1.Columns["Qty_To_Allot_Procure"].MappingName;
            var AStock = (rowData.GetType().GetProperty(AllotedStock).GetValue(rowData, null).ToString());
            if (Convert.ToDecimal(AStock) == 0)
            {
                MessageBox.Show("When Qty To Procure is Zero, No Procurement Data to update");
                return;
            }

            var mappingName = sfDataGrid1.Columns["Ord_No"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
            var mappingName2 = sfDataGrid1.Columns["S_No"].MappingName;
            var mappingName3 = sfDataGrid1.Columns["Prod_Code"].MappingName;
            var mappingName4 = sfDataGrid1.Columns["Item_Description"].MappingName;
            var mappingName5 = sfDataGrid1.Columns["Ord_Qty"].MappingName;
            var mappingName6 = sfDataGrid1.Columns["Delivery_Date"].MappingName;
            var cellvalue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            textBox5.Text = cellvalue.ToString();
            var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            textBox2.Text = cellvalue1.ToString();
            var cellvalue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            textBox4.Text = cellvalue2.ToString();

            var cellvalue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            textBox3.Text = cellvalue3.ToString();
            textBox6.Text = AStock.ToString();
            //txtOrdNo.Text = R1.Cells["Ord_No"].Value.ToString();
            //txtOrdID.Text = R1.Cells["SO_Master_ID"].Value.ToString();
            //txtItemSno.Text = R1.Cells["S_No"].Value.ToString();
            //txtItemCode.Text = R1.Cells["Prod_Code"].Value.ToString();
            //SO_Item_Name = R1.Cells["Item_Description"].Value.ToString();
            //SO_Qty = R1.Cells["Ord_Qty"].Value.ToString();
            //del_date = R1.Cells["Delivery_Date"].Value.ToString();

            var Prodname = (from d in db.Forging_FinishedGoods_RMs
                            join f in db.Forging_Finished_Goods on d.FG_Item_ID equals f.prod_ID
                            join p in db.Products on d.RM_Prod_ID equals p.prod_ID
                            where f.Prod_Customer_Code == textBox3.Text
                            select new { p.Prod_Name,p.prod_ID }).ToList();

            if (Prodname.Count > 0)
            {
                comboBox2.DataSource = Prodname;
                comboBox2.DisplayMember = "Prod_Name";
                comboBox2.ValueMember = "prod_ID";
                if (comboBox2.Items.Count > 0)
                {
                    comboBox2.SelectedIndex = -1;
                }
                else
                {
                    comboBox2.SelectedIndex = -1;
                }
            }
            groupBox2.Visible = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if(comboBox2.Text!="")
            {
                var Prodname = (from d in db.Get_PO_Qty_ProdWise(logIn.company,logIn.BU_ID,Convert.ToInt32(comboBox2.SelectedValue)) select new { d.PO_NO, d.PO_Qty,PO_Price }).ToList();
                if (Prodname.Count > 0)
                {

                    dataGridView2.DataSource = Prodname;
                }
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // determine if click was on our date column
                if (dataGridView2.Columns[e.ColumnIndex].DataPropertyName == nameof(User.Available_Date))
                {
                    // initialize DateTimePicker

                    dtp = new DateTimePicker();
                    dtp.Format = DateTimePickerFormat.Short;
                    dtp.Visible = true;
                    if (dataGridView2.CurrentCell.Value != null)
                    {
                        dtp.Value = DateTime.Parse(dataGridView2.CurrentCell.Value.ToString());
                    }
                    // set size and location
                    var rect = dataGridView2.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                    dtp.Size = new Size(rect.Width, rect.Height);
                    dtp.Location = new Point(rect.X, rect.Y);

                    // attach events
                    dtp.CloseUp += new EventHandler(dtp_CloseUp);
                    dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                    dataGridView2.Controls.Add(dtp);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            dataGridView2.CurrentCell.Value = dtp.Text.ToString();
        }
        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {

                    forge_PO_Reserved_Qty ci = new forge_PO_Reserved_Qty();
                    ci.So_Master_ID = Convert.ToInt32(textBox2.Text);
                    ci.So_S_No = Convert.ToInt32(textBox4.Text);                   
                    ci.RM_Prod_ID = Convert.ToInt32(comboBox2.SelectedValue);
                    ci.PO_No = (dataGridView2.Rows[i].Cells["PO_No"].Value == null) ? "" : (dataGridView2.Rows[i].Cells["PO_No"].Value).ToString();

                    ci.PO_Price = (dataGridView2.Rows[i].Cells["PO_Price"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToDecimal(dataGridView2.Rows[i].Cells["PO_Price"].Value);
                    ci.PO_Qty = (dataGridView2.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToDecimal(dataGridView2.Rows[i].Cells["PO_Qty"].Value);
                    ci.Available_Date = Convert.ToDateTime(dataGridView2.Rows[i].Cells["Available_Date"].Value);
                    ci.Qty_Reserved = (dataGridView2.Rows[i].Cells["Qty_Reserved"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToDecimal(dataGridView2.Rows[i].Cells["Qty_Reserved"].Value);


                    ci.Company_Id = logIn.company;
                    ci.Created_By = logIn.username;
                    ci.Modified_By = logIn.username;
                    db.forge_PO_Reserved_Qties.InsertOnSubmit(ci);
                }
                db.SubmitChanges();
                MessageBox.Show("Record Updated Sucessfully");
                //clear();
                return;
                

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Update Selectedf Machines in Temp Table
            string MachineProcess = "";
            SqlCommand cmd1 = new SqlCommand();
            //inv_No1 = row.Cells["Invoice_No"].Value.ToString();
            cmd1.CommandText = "delete from tmp_Machines_Selected";            
            cmd1.Connection = con;
            con.Open();
            cmd1.ExecuteNonQuery();

            cmd1.CommandText = "delete from forge_Scheduled_Orders";
            cmd1.Connection = con;           
            cmd1.ExecuteNonQuery();
            con.Close();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))

                {
                    SqlCommand cmd = new SqlCommand();
                    //inv_No1 = row.Cells["Invoice_No"].Value.ToString();
                    cmd.CommandText = "INSERT INTO tmp_Machines_Selected ([Machine_Name]) Values (@param1)";
                    cmd.Parameters.AddWithValue("@param1", checkedListBox1.Items[i].ToString());                   
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }


            //Get Tentative Schedule Dates
            for (int i = 2; i < sfDataGrid1.RowCount; i++)
            {
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Ord_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
                var mappingName2 = sfDataGrid1.Columns["S_No"].MappingName;
                var mappingName3 = sfDataGrid1.Columns["Prod_Code"].MappingName;
                var mappingName4 = sfDataGrid1.Columns["Item_Description"].MappingName;
                var mappingName5 = sfDataGrid1.Columns["Ord_Qty"].MappingName;
                var mappingName6 = sfDataGrid1.Columns["CDD"].MappingName;
                var AllotedStock = sfDataGrid1.Columns["Qty_Alloted"].MappingName;
                var AStock = (rowData.GetType().GetProperty(AllotedStock).GetValue(rowData, null).ToString());
                
                var cellvalue = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                var cellvalue2 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
                var cellvalue3 = (rowData.GetType().GetProperty(mappingName6).GetValue(rowData, null).ToString());
                //if (Convert.ToDecimal(AStock) > 0)
                //{ 

                SqlCommand com = new SqlCommand("SP_Get_Process_SOItemWise", con);
                com.Parameters.AddWithValue("@compname", logIn.company);
                com.Parameters.AddWithValue("@so_master_Id", Convert.ToInt32(cellvalue));
                com.Parameters.AddWithValue("@so_S_no", Convert.ToInt32(cellvalue1));

                com.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    string process_ID = dt.Rows[k]["Process_ID"].ToString();
                    string process_name = dt.Rows[k]["process_name"].ToString();
                    string Process_Time = dt.Rows[k]["Process_Time"].ToString();
                    int MID = Convert.ToInt32(dt.Rows[k]["default_machine_id"].ToString());
                    int Machine_Id = 0;
                    DateTime endDate = DateTime.Now;
                    if (MID != 0)
                    {
                        Machine_Id = Convert.ToInt32(dt.Rows[k]["default_machine_id"].ToString());
                        var q = from n in db.forge_Scheduled_Orders
                                where n.Machine == Machine_Id
                                group n by n.Machine into g
                                select new { Machine = g.Key, Date = g.Max(t => t.End_Date) };
                       

                        q.ToList().ForEach(j =>
                        {
                            endDate = Convert.ToDateTime(j.Date.ToString());
                        });
                    }

                    else
                    {
                        var Buyerblind = (from m in db.sp_Forge_Get_Machines_Process_WIse(logIn.company, process_name)
                                          select new { m.Machine_Name, m.Avbl_Date , m.Mach_ID}).Distinct().ToList();
                        if (Buyerblind.Count > 0)
                        {
                            //txt_fg_Item_Name.Text = Buyerblind[0].Product_Description;
                            endDate = Convert.ToDateTime(Buyerblind[0].Avbl_Date.ToString());
                            Machine_Id = Convert.ToInt32(Buyerblind[0].Mach_ID.ToString());
                            //dtDelDate.Text = Buyerblind[0].Del_Date.ToString();
                            //CustName = Convert.ToInt32(Buyerblind[0].BuyerName);
                        }
                    }
                    
                    if (endDate < DateTime.Now)
                    {
                        endDate = DateTime.Now;
                    }
                    double PDays = Convert.ToDouble(Process_Time);
                    //if(PDays>4)
                    //{
                    //    PDays = PDays + 1;
                    //}
                    DateTime sDate = endDate.AddHours(PDays);
                    DateTime eDate = endDate;
                    double wHrs = Convert.ToDouble(textBox7.Text);
                    if (PDays > wHrs)
                    {
                        while (PDays>wHrs)
                        {
                            double h = wHrs;
                          //  eDate = eDate.AddDays(1);
                            eDate = endDate.AddHours(h);
                            PDays = PDays - h;
                        }
                        //for (double h = 8; h < PDays; h++)
                        //{

                        //    eDate = endDate.AddHours(h);
                        //}
                        sDate = eDate.AddHours(PDays);
                    }
                    else
                    {
                        sDate = endDate.AddHours(PDays);
                    }
                    if (Machine_Id != 0)
                    {

                        forge_Scheduled_Order ci = new forge_Scheduled_Order();
                        ci.SO_Master_ID = Convert.ToInt32(cellvalue);
                        ci.So_item_No = Convert.ToInt32(cellvalue1);
                        ci.Start_Date = endDate;
                        ci.End_Date = sDate;
                        ci.Machine = Machine_Id;
                        ci.Prod_Code = Convert.ToInt32(process_ID); //Saving Process ID
                        ci.Process_Time = Convert.ToDecimal(Process_Time);  
                        ci.Prod_Shift = "G";
                        ci.Delivery_Date =Convert.ToDateTime(cellvalue3);                        
                        ci.Company_ID = logIn.company;
                        ci.Created_By = logIn.username;
                        ci.Modified_BY = logIn.username;
                        db.forge_Scheduled_Orders.InsertOnSubmit(ci);
                        db.SubmitChanges();
                    }
                    con.Close();
                    //MessageBox.Show("Schedule Updated Successfully");
                    
                }
            }
        }

        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void sfDataGrid2_SelectionChanged(object sender, Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs e)
        {
            try
            {
                //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                //string columnName = dgProducts.Columns[columnIndex].Name;
                DataTable dt = new DataTable();
                int i = sfDataGrid2.CurrentCell.RowIndex;
                if (i > 1)
                {
                    // int k = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                    var SONo = sfDataGrid2.Columns["SO_Master_ID"].MappingName;
                    var SO_ID = (rowData.GetType().GetProperty(SONo).GetValue(rowData, null).ToString());

                    var Itemno = sfDataGrid2.Columns["S_No"].MappingName;
                    var itemID = (rowData.GetType().GetProperty(Itemno).GetValue(rowData, null).ToString());
                    SqlCommand com = new SqlCommand("SP_Get_Process_Sheduled_Orders", con);
                    com.Parameters.AddWithValue("@compname", logIn.company);
                    com.Parameters.AddWithValue("@so_master_Id", Convert.ToInt32(SO_ID));
                    com.Parameters.AddWithValue("@so_S_no", Convert.ToInt32(itemID));

                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(com);

                    con.Open();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void modifyTheScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid2.CurrentCell.RowIndex;
            // int k = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
            //var AllotedStock = sfDataGrid1.Columns["Qty_Alloted"].MappingName;
            //var AStock = (rowData.GetType().GetProperty(AllotedStock).GetValue(rowData, null).ToString());
            //if (Convert.ToDecimal(AStock) <= 0)
            //{
            //    MessageBox.Show("Order Cannot Be Scheduled unless Raw Material is Alloted");
            //    return;
            //}

            var mappingName = sfDataGrid2.Columns["Ord_No"].MappingName;
            var mappingName1 = sfDataGrid2.Columns["SO_Master_ID"].MappingName;
            var mappingName2 = sfDataGrid2.Columns["S_No"].MappingName;
            var mappingName3 = sfDataGrid2.Columns["Prod_Code"].MappingName;
            var mappingName4 = sfDataGrid2.Columns["Item_Description"].MappingName;
            var mappingName5 = sfDataGrid2.Columns["Ord_Qty"].MappingName;
            var mappingName6 = sfDataGrid2.Columns["CDD"].MappingName;
            var cellvalue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            txtOrdNo.Text = cellvalue.ToString();
            var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            txtOrdID.Text = cellvalue1.ToString();
            var cellvalue2 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            txtItemSno.Text = cellvalue2.ToString();

            var cellvalue3 = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            txtItemCode.Text = cellvalue3.ToString();
            //txtOrdNo.Text = R1.Cells["Ord_No"].Value.ToString();
            //txtOrdID.Text = R1.Cells["SO_Master_ID"].Value.ToString();
            //txtItemSno.Text = R1.Cells["S_No"].Value.ToString();
            //txtItemCode.Text = R1.Cells["Prod_Code"].Value.ToString();
            //SO_Item_Name = R1.Cells["Item_Description"].Value.ToString();
            //SO_Qty = R1.Cells["Ord_Qty"].Value.ToString();
            //del_date = R1.Cells["Delivery_Date"].Value.ToString();

            var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Forging" && d.Company_ID == logIn.company select new { d.ID, d.Machine_Name }).ToList();
            if (Prodname.Count > 0)
            {
                cmbMachine.DataSource = Prodname;
                cmbMachine.DisplayMember = "Machine_Name";
                cmbMachine.ValueMember = "ID";
                if (cmbMachine.Items.Count > 0)
                {
                    cmbMachine.SelectedIndex = -1;
                }
                else
                {
                    cmbMachine.SelectedIndex = -1;
                }
            }


            groupBox1.Visible = true;
        }

        void SetCellBackgroundColorGRID2(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid2.TableControl.Invalidate(this.sfDataGrid2.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ////Get Orders To Schedule
            //SqlCommand cmd2 = new SqlCommand("SP_Get_ForgeSOrdersToSchedule", con);
            //cmd2.CommandType = CommandType.StoredProcedure;
            //cmd2.Parameters.AddWithValue("@compname", logIn.company);            
            //            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            ////DataSet ds2 = new DataSet();
            //DataTable ds2 = new DataTable();
            //// da2.Fill(ds2, "x");
            //da2.Fill(ds2);
            //sfDataGrid1.DataSource = ds2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Get Scheduled Orders
            //SqlCommand cmd2 = new SqlCommand("SP_Get_ForgeSOrdersToScheduled", con);
            //cmd2.CommandType = CommandType.StoredProcedure;
            //cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            ////DataSet ds2 = new DataSet();
            //DataTable ds2 = new DataTable();
            //// da2.Fill(ds2, "x");
            //da2.Fill(ds2);
            //dataGridView1.DataSource = ds2;
        }

        private void scheduleTheOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void allotStocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
            //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
            //string columnName = dgProducts.Columns[columnIndex].Name;

            int i = sfDataGrid1.CurrentCell.RowIndex;
            // int k = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["Ord_No"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["SO_Master_ID"].MappingName;
            var mappingName2 = sfDataGrid1.Columns["S_No"].MappingName;
            var mappingName3 = sfDataGrid1.Columns["Prod_Code"].MappingName;
            var mappingName4 = sfDataGrid1.Columns["Item_Description"].MappingName;
            var mappingName5 = sfDataGrid1.Columns["Ord_Qty"].MappingName;
            var mappingName6 = sfDataGrid1.Columns["Delivery_Date"].MappingName;
            SO_No = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            SO_ID = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            SO_Item_No = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            SO_Item_Code = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
            SO_Item_Name = (rowData.GetType().GetProperty(mappingName4).GetValue(rowData, null).ToString());
            SO_Qty = (rowData.GetType().GetProperty(mappingName5).GetValue(rowData, null).ToString());
            del_date = (rowData.GetType().GetProperty(mappingName6).GetValue(rowData, null).ToString());

            //SO_No = R1.Cells["Ord_No"].Value.ToString();
            //SO_ID = R1.Cells["SO_Master_ID"].Value.ToString();
            //SO_Item_No = R1.Cells["S_No"].Value.ToString();
            //SO_Item_Code = R1.Cells["Prod_Code"].Value.ToString();
            //SO_Item_Name = R1.Cells["Item_Description"].Value.ToString();
            //SO_Qty = R1.Cells["Ord_Qty"].Value.ToString();
            //del_date = R1.Cells["Delivery_Date"].Value.ToString();           

            ProductionManagement.Transactions.frmForge_RM_Reservation frm = new ProductionManagement.Transactions.frmForge_RM_Reservation();
            //OrderManagement.Transactions.            
            frm.ShowDialog();
            //Get Orders To Schedule
            //SqlCommand cmd2 = new SqlCommand("SP_Get_ForgeSOrdersToSchedule", con);
            //cmd2.CommandType = CommandType.StoredProcedure;
            //cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            ////DataSet ds2 = new DataSet();
            //DataTable ds2 = new DataTable();
            //// da2.Fill(ds2, "x");
            //da2.Fill(ds2);
            //dgProducts.DataSource = ds2;
        }
    }
}
