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
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Ione_DAL;
using System.Net.Mail;
using System.Net;

namespace ioneNet.MaterialManagement
{
    public partial class GatePassList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public static int gp_id;

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Vch_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Id"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    if (cellVaue1.ToString() != "")
                    {
                        string custId = cellVaue1;
                        

                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Gate Pass /Job Work Challan" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Delete_Role == true)
                            {
                                var ci = db.GatePass_Masters.Where(w => w.Id == Convert.ToInt32(custId) && w.Company_ID == logIn.company).FirstOrDefault();
                                {
                                    ci.isDeleted = true;
                                    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    db.SubmitChanges();
                                    MessageBox.Show("Gatepass Deleted Sucessfully");
                                    BindOrderslist();
                                }
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Delete The Gate Pass");
                                return;
                            }

                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select GatePass To Proceed");
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            try
            {
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Gate Pass /Job Work Challan"
                               select new
                               {
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                this.modifyToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Modify_Role == true)
                {
                    this.modifyToolStripMenuItem.Enabled = true;
                }
                


                this.deleteToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Delete_Role == true)
                {

                    this.deleteToolStripMenuItem.Enabled = true;
                }
                btnAddnew.Enabled = false;
                if (bindLoc[0].Create_Role == true)
                {
                    btnAddnew.Enabled = true;
                }
                BindOrderslist();
            }
            catch(Exception ex)
            {
                
            }
           
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.SP_ShowGatepasslist(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                    }
                    if (cellVaue.ToString() == "Despatches Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                    }
                }
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    //txtSearch.Text = "";
                //}

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
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                int r = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(r);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                SO_No = cellVaue.ToString();
                var mappingName1 = sfDataGrid1.Columns["Id"].MappingName;
                
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
              // boolen GPType = cellVaue1;
                //cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where Company_ID = @compID";
                //cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                //cmd1.Parameters.AddWithValue("@compID", logIn.company);
                //cmd1.ExecuteNonQuery();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "GatePass.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);


                if (fi1.Exists)
                {
                    fi1.Delete();
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                rep = new MaterialManagement.Transactions.GatePass();
                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID; 
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int i = 0; i < crTables.Count; i++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[i].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[i].ApplyLogOnInfo(crTableLogOnInfo);
                    //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

                }
                int GPID = Convert.ToInt32(cellVaue1);
                //rep.SetParameterValue("Creation_Company", frmLogin.Creation_Company);                 
                rep.RecordSelectionFormula = "{GatePass_Master.Id} = " + GPID + "  and { GatePass_Master.Company_ID} = " + logIn.company + "";



                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filepath = "";
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                int r = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(r);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                SO_No = cellVaue.ToString();
                var mappingName1 = sfDataGrid1.Columns["Id"].MappingName;

                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                // boolen GPType = cellVaue1;
                //cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where Company_ID = @compID";
                //cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                //cmd1.Parameters.AddWithValue("@compID", logIn.company);
                //cmd1.ExecuteNonQuery();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "GatePass.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);


                if (fi1.Exists)
                {
                    fi1.Delete();
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                rep = new MaterialManagement.Transactions.GatePass();
                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int i = 0; i < crTables.Count; i++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[i].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[i].ApplyLogOnInfo(crTableLogOnInfo);
                    //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

                }
                int GPID = Convert.ToInt32(cellVaue1);
                //rep.SetParameterValue("Creation_Company", frmLogin.Creation_Company);                 
                rep.RecordSelectionFormula = "{GatePass_Master.Id} = " + GPID + "  and { GatePass_Master.Company_ID} = " + logIn.company + "";



                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                filepath = path;
                var Email = (from c in db.GatePass_Masters
                             join inv in db.Supplier_informations
                             on c.PartyName equals inv.ID
                             join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                             where c.Company_ID == logIn.company && c.Id == GPID
                             select new { inv.Email_Id, em.SmtpServer, em.SmptPort, em.POMailID, em.POMailPW, em.Default_CC_Mail_id }).ToList();
                string email = Email[0].Email_Id;
                if (email == "" || email == null)
                {
                    MessageBox.Show("Customer Email Not Found In the Records");
                    return;
                }
                // string email = Email[0].Cust_Eail;
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(Email[0].POMailID);

                mm.To.Add(email);
                if (Email[0].Default_CC_Mail_id != null)
                {
                    mm.CC.Add(Email[0].Default_CC_Mail_id);
                }
                mm.Subject = "Gate Pass No :" + cellVaue;
                mm.Body = "Dear Sir," + "\n" + "Please Find Attached Gate Pass for Your information";
                // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
                mm.Attachments.Add(new Attachment(filepath));
                string nme;


                //}
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Email[0].SmtpServer;
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = Email[0].POMailID;
                NetworkCred.Password = Email[0].POMailPW;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = Convert.ToInt32(Email[0].SmptPort);
                smtp.Send(mm);
                //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == cellVaue && w.Company_ID == logIn.company).FirstOrDefault();
                //{
                //    ci.email_Sent = true;
                //    db.SubmitChanges();
                //}
                MessageBox.Show("Gate Pass Sent by Email Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Id"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {
                            SO_No = cellVaue;
                            gp_id = Convert.ToInt32(cellVaue1);
                            //   GpType = cellVaue1;
                            var = "1";
                            editMode = true;
                            MaterialManagement.Transactions.GatePasReturnable frm = new MaterialManagement.Transactions.GatePasReturnable();
                            //OrderManagement.Transactions.
                            // frm.MdiParent = this.MdiParent;
                            frm.ShowDialog();
                            BindOrderslist();
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
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
        public static Boolean editMode;

        public GatePassList()
        {
            InitializeComponent();
        }

        
        

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            editMode = false;
            MaterialManagement.Transactions.GatePasReturnable frm = new MaterialManagement.Transactions.GatePasReturnable();
           // frm.MdiParent = this.MdiParent;
            frm.ShowDialog();
            BindOrderslist();
            // showGRNListTableAdapter.Fill(ioneDataSet.ShowGRNList, logIn.company, null);
            // dgvRecordList.DataSource = showGRNListBindingSource;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //showGRNListTableAdapter.Fill(ioneDataSet.ShowGRNList, logIn.company, txtSearch.Text);
            //dgvRecordList.DataSource = showGRNListBindingSource;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
            //showGRNListTableAdapter.Fill(ioneDataSet.ShowGRNList, logIn.company, null);
            //dgvRecordList.DataSource = showGRNListBindingSource;
            //txtSearch.Text = "";
            //BindPurInvoicelist();
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
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Id"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue1.ToString() != "")
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true)
                        {
                            SO_No = cellVaue;
                            gp_id = Convert.ToInt32(cellVaue1);
                            //   GpType = cellVaue1;
                            var = "0";
                            editMode = true;
                            MaterialManagement.Transactions.GatePasReturnable frm = new MaterialManagement.Transactions.GatePasReturnable();
                            //OrderManagement.Transactions.
                            // frm.MdiParent = this.MdiParent;
                            frm.ShowDialog();
                            BindOrderslist();
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
