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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using System.Net.Mail;
using System.Net;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;

namespace ioneNet.FinanceManagement.Transactions
{

    public partial class PaymentRequestList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;

        private void sfButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Request_No"].MappingName;
                 var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre-Closed")
                    {
                        if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Reviewed")
                        {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Payment Request" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Modify_Role == true)
                                {

                                    SO_No = cellVaue;                                   
                                    var = "0";
                                    editMode = true;
                                    FinanaceManagement.PaymentRequest frm = new FinanaceManagement.PaymentRequest();
                                    //OrderManagement.Transactions.
                                    
                                    frm.ShowDialog();
                                    //FrmInv.ShowDialog();
                                    //i1 = 0;
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Payment Requests");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Modify The Payment Requests");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Request Already Approved / Reviewed or Further Processed.. No Work Done");

                        }
                    }
                    else
                    {
                        MessageBox.Show("The Selected Request is Closed, Fourther Mordifications Not Allowed");
                        return;
                        //i1 = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    return;
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void authorizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Request_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                        string Status = cellVaue1.ToString();
                        string OrdNo = cellVaue.ToString();
                        if (Status == "Created")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Payment Request" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Approve_Role == true)
                                {
                                    var ci = db.Payment_Request_Masters.Where(w => w.Voucher_No == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {
                                        ci.Status = 4;
                                        ci.Reviewed_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Authorize The Payment Requests");
                                    return;
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Request No " + OrdNo + " Is Already Approved / Reviewed or Further Processed.. No Work Done");

                        }
                    }

                    //}
                }
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Request_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                        string Status = cellVaue1.ToString();
                        string OrdNo = cellVaue.ToString();
                        if (Status == "Created" || Status == "Reviewed")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Payment Request" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Approve_Role == true)
                                {
                                    var ci = db.Payment_Request_Masters.Where(w => w.Voucher_No == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {
                                        ci.Status = 6;
                                        ci.Approved_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The Payment Requests");
                                    return;
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Request No " + OrdNo + " Is Already Approved / Reviewed or Further Processed.. No Work Done");

                        }
                    }

                    //}
                }
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void amendmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Request_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                   
                    if (cellvalue1.ToString() == "Approved")
                    {
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Payment Voucher" && m.Role_ID == logIn.UserRoleID select new { m.Create_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Create_Role == true)
                            {
                                 
                                SO_No = cellVaue;
                                var = "0";
                                editMode = true;
                                FinanaceManagement.AccountVoucher frm = new FinanaceManagement.AccountVoucher();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();
                                //FrmInv.ShowDialog();
                                //i1 = 0;
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Generate The Payment Voucher");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Generate The Payment Voucher");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Only Approved Requests Can Generate Vouchers.. No Work Done");

                    }                   
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    return;
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public static Boolean editMode;
        public PaymentRequestList()
        {
            InitializeComponent();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            FinanaceManagement.PaymentRequest frm = new FinanaceManagement.PaymentRequest();
            //frm.MdiParent = this.MdiParent;
            frm.ShowDialog();
            BindOrderslist();
        }

        private void PaymentRequestList_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowPaymentRequestList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Request_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Request_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Request_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Request_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["supplier_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["supplier_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["supplier_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["supplier_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "ComboBox";
                    this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Equals;

                    this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                    string cellValue;
                    for (int i = 2; i < sfDataGrid1.RowCount; i++)
                    {
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        if (cellVaue.ToString() == "Approved")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                        }
                        if (cellVaue.ToString() == "Reviewed")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                        }
                        if (cellVaue.ToString() == "Payment Done")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                        }
                        if (cellVaue.ToString() == "Cancelled")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Orange);
                        }
                    }
                }
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


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
    }
}
