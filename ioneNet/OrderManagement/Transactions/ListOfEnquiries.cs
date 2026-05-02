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
using System.Diagnostics;
using System.Data.OleDb;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid.Enums;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Events;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class ListOfEnquiries : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No,Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode =false;

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;

        public ListOfEnquiries()
        {
            InitializeComponent();
        }
        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
            //this.showSOListBindingSource.Filter = dgvRecordList.FilterString;
        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
            //this.showSOListBindingSource.Sort = dgvRecordList.SortString;
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("Enq_NO").GetValue(rowData, null).ToString());
                //var currentAmendValue = (rowData.GetType().GetProperty("Enq_Amend_No").GetValue(rowData, null).ToString());               
                
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        SO_No = currentCellValue.ToString();
                        //SO_Amend_No = currentAmendValue.ToString();
                        var = "1";
                        editMode = true;
                        OrderManagement.Transactions.frmNewEnquiry frm = new OrderManagement.Transactions.frmNewEnquiry();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();

                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Enquiry");
                        return;
                    }
                }                    
                  
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string cellValue;
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
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                MessageBox.Show("Selected Order(s) Are Pre-Closed Successfully");
                BindOrderslist();





                //foreach (i= 0; i <= dgvRecordList.Rows.Count;if++)
                //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
                //{
                ////DataGridViewRow row = i;

                //bool isSelected = Convert.ToBoolean(dgvRecordList.Rows[i].Cells["SelOrd"].Value);
                //if (isSelected)
                //{

                //    SqlCommand cmd = new SqlCommand();

                //    SO_No = dgvRecordList.Rows[i].Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //    cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);                       
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //    cmd.Parameters.Clear();
                //    string strT = logIn.username + "-" + DateTime.Now;
                //    cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@strT", strT);
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                ////else
                ////{
                ////    MessageBox.Show("Atlease One So No to be Selected to Approve");
                ////}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    string cellValue;
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
                            var mappingName = sfDataGrid1.Columns["Enq_NO"].MappingName;
                            var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                            //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                            if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                                var cellStatus = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                                if (cellStatus.ToString() != "Quot Submitted")
                                {
                                    SqlCommand cmd = new SqlCommand();

                                    SO_No = cellVaue.ToString();
                                    cmd.CommandText = "Update Sale_Enquiry_Master set status = '24' where Enq_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    cmd.Parameters.Clear();
                                    string strT = logIn.username + "-" + DateTime.Now;
                                    cmd.CommandText = "Update Sale_Enquiry_Master set Modified_By = @strT where Enq_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@strT", strT);
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Selected Enquiry Cannot Be Deleted As Already Quote Submitted");
                                    return;
                                }
                            }
                        }
                    }
                    MessageBox.Show("Selected Enquiry(s) Are Deleted Successfully");
                    BindOrderslist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvRecordList_DataSourceChanged(object sender, EventArgs e)
        {
            //for (int i = 0; i < dgvRecordList.Rows.Count; i++)
            //{
                
            //    if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Created")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.OrangeRed;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        // dgorders.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
            //    }
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Approved")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.DarkGreen;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        // dgorders.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
            //    }
               
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Pre-Closed")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.DarkViolet;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        //dgorders.Rows[i].DefaultCellStyle.BackColor = Color.Violet;
            //    }
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Shipped")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.SkyBlue;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        //dgorders.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
            //    }
               
            //}
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {

        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void generateCostingSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
        }

        private void submitQuoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            //if (i >= 0)
            //{
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var currentCellValue = (rowData.GetType().GetProperty("Enq_NO").GetValue(rowData, null).ToString());
           // var currentAmendValue = (rowData.GetType().GetProperty("Enq_Amend_No").GetValue(rowData, null).ToString());
            var currentCustName = (rowData.GetType().GetProperty("Customer_name").GetValue(rowData, null).ToString());
            SO_No = currentCellValue.ToString();
            //SO_Amend_No = currentAmendValue.ToString();
            Consignee = currentCustName.ToString();
            OrderManagement.Transactions.frmNewOrder_TSL frm = new frmNewOrder_TSL();
            //OrderManagement.Transactions.      
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void amendmentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'viswaSiOneDataSet.Order_Data_Monthly_for_Chart' table. You can move, or remove it, as needed.
            //showSOListTableAdapter.Fill(ioneDataSet.ShowSOList, logIn.company,null);
            ////dgvRecordList.DataSource = showSOListBindingSource;
            //sfDataGrid1.DataSource = showSOListBindingSource;

            //string pcode = "%" + txtSearch.Text + "%";
            BindOrderslist();
            sfDataGrid1.Width = tableLayoutPanel2.Width;
            //MessageBox.Show(tableLayoutPanel2.Width);



        }

        private void printReviewFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            rep = new OrderManagement.Transactions.EnquiryReviewForm();
            path = Path.Combine(Directory.GetCurrentDirectory(), "Enquiry_Review.pdf");
            //string path = @"D:\Invoice.pdf";
            FileInfo fi1 = new FileInfo(path);


            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns[1].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
            SqlCommand cmd1 = con.CreateCommand();
            SqlCommand cmd2 = con.CreateCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            //int i = sfDataGrid1.CurrentRow.Index;
            SO_No = cellVaue.ToString();
            SqlCommand cmd = new SqlCommand("sp_Rpt_EnquiryReviewForm", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Enq_No", SO_No);
            cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
            cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
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
                //Loop through all tables in the report and apply the connection information for each table.
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
                // cmd1.Parameters.AddWithValue("@Con_Address1", "Door No");

                
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                cmd.Parameters.Clear();
                Process.Start(path);
            }
            con.Close();
        }

        private void regretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("Enq_NO").GetValue(rowData, null).ToString());
                //var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


                if (cellVaue.ToString() != "Quot Submitted")
                {
                    groupBox1.Visible = true;
                    comboBox1.Focus();
                }
                else
                {
                    MessageBox.Show("Quote Submitted Against Selected Enquiry, Hence Cannot Be Regreted Now");
                    return;
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible=false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string cellValue;
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
                        var mappingName = sfDataGrid1.Columns[1].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                            var ci = db.Sale_Enquiry_Masters.Where(w => w.Enq_NO == cellVaue && w.Company_ID == logIn.company && w.bu_id == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Status = 2219;
                                if (comboBox1.Text != "Others")
                                {
                                    ci.Reason_To_Regret = comboBox1.Text;
                                }
                                else
                                {
                                    ci.Reason_To_Regret = textBox1.Text;
                                }
                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();
                            }


                            //SqlCommand cmd = new SqlCommand();

                            //SO_No = cellVaue.ToString();
                            //cmd.CommandText = "Update Sale_Enquiry_Master set status = '2219' where Enq_No =@param1 and Company_ID =@compName";
                            //cmd.Parameters.AddWithValue("@param1", SO_No);
                            //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            //cmd.Connection = con;
                            //con.Open();
                            //cmd.ExecuteNonQuery();
                            //con.Close();
                            //cmd.Parameters.Clear();
                            //string strT = logIn.username + "-" + DateTime.Now;
                            //cmd.CommandText = "Update Sale_Enquiry_Master set Modified_By = @strT where Enq_No=@param1 and Company_ID =@compName";
                            //cmd.Parameters.AddWithValue("@strT", strT);
                            //cmd.Parameters.AddWithValue("@param1", SO_No);
                            //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            //cmd.Connection = con;
                            //con.Open();
                            //cmd.ExecuteNonQuery();
                            //con.Close();
                        }
                    }
                }
                MessageBox.Show("Selected Enquiry Status Updated Successfully");
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }

        private void allotStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }



       
        

        
        public void BindOrderslist()
        {
            try
            {
                string p = "";

                //var da = (from obj in db.User_Roles
                //          where obj.Role_ID == logIn.UserRoleID && obj.Company_ID == logIn.company
                //          select obj).ToList();

                //if (da[0].Roll_Type == "User")
                //{
                //    p = "user";
                //}
                //else
                //{
                    p = "admin";
                //}

                var d = (from data in db.ShowEnqList(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date, p,logIn.BU_ID, logIn.userID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Enq_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Enq_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Enq_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Enq_NO"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Customer_name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "ComboBox";
                    this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Equals;
                    this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
                    //string cellValue;
                    //for (int i = 2; i < sfDataGrid1.RowCount; i++)
                    //{
                    //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //    if (cellVaue.ToString() == "Quot Submitted")
                    //    {
                    //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                    //    }
                    //    if (cellVaue.ToString() == "Created")
                    //    {
                    //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                    //    }
                    //    if (cellVaue.ToString() == "Costing Done")
                    //    {
                    //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.YellowGreen);
                    //    }
                    //    if (cellVaue.ToString() == "Regreted")
                    //    {
                    //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Record Not Found");
                    //    //txtSearch.Text = "";
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "Status")
            {
                if (e.DisplayText == "Quot Submitted")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Created")
                {
                    e.Style.BackColor = Color.SkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Regreted")
                {
                    e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Black;
                }
            }

            //if (e.Column.MappingName == "Ord_Type")
            //{
            //    if (e.DisplayText == "Conversion")
            //    {
            //        e.Style.BackColor = Color.LightGreen;
            //        e.Style.TextColor = Color.Black;
            //    }
            //    else if (e.DisplayText == "Direct Sale")
            //    {
            //        e.Style.BackColor = Color.SkyBlue;
            //        e.Style.TextColor = Color.Black;
            //    }
            //}
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                editMode = false;
                //this.Close();
                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
                OrderManagement.Transactions.frmNewEnquiry frm = new OrderManagement.Transactions.frmNewEnquiry();
                frm.MdiParent = this.ParentForm;
                frm.Show();
                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
                //BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //showSOListTableAdapter.Fill(ioneDataSet.ShowSOList, logIn.company, txtSearch.Text);
            //dgvRecordList.DataSource = showSOListBindingSource;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var currentCellValue =  (rowData.GetType().GetProperty("Enq_NO").GetValue(rowData, null).ToString());
                   // var currentAmendValue = (rowData.GetType().GetProperty("Enq_Amend_No").GetValue(rowData, null).ToString());
                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                    {
                        if (cellVaue.ToString() == "Created" ||  cellVaue.ToString() == "Costing Done" || cellVaue.ToString() == "Despatches Started")
                        {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Sale Enquiry" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if(uRole[0].Modify_Role==true)
                                {
                                    SO_No = currentCellValue.ToString();
                                //SO_Amend_No = currentAmendValue.ToString();
                                    var = "0";
                                    editMode = true;
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                                OrderManagement.Transactions.frmNewEnquiry frm = new frmNewEnquiry();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

                            }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Enquiry");
                                    return;
                                }
                         }
                        else
                        {
                            SO_No = currentCellValue.ToString();
                            //SO_Amend_No = currentAmendValue.ToString();
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.AllCells;
                            var = "0";
                            editMode = true;
                            OrderManagement.Transactions.frmNewEnquiry frm = new frmNewEnquiry();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Enquiry Cannot Be Modified Either Closed or Quote Recived");
                        //i1 = 0;
                    }
                }
                //else
                //{
                //    MessageBox.Show("No Order is Selected to Modify");

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
