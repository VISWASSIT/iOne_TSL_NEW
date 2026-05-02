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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid.Enums;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmInward_Frieght_Advise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmInward_Frieght_Advise()
        {
            InitializeComponent();
        }

        private void frmInward_Frieght_Advise_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            AutoincrementId();

            var PurBasis = (from m in db.Supplier_informations where m.Supplier_Category == 29 && m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
            if (PurBasis.Count > 0)
            {
                cmbTransporter.DataSource = PurBasis;
                cmbTransporter.ValueMember = "ID";
                cmbTransporter.DisplayMember = "Supplier_Name";
            }

            var PurBasis1 = (from m in db.Supplier_informations where m.Supplier_Category == 29 && m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
            if (PurBasis1.Count > 0)
            {
                cmbVehicleOwner.DataSource = PurBasis1;
                cmbVehicleOwner.ValueMember = "ID";
                cmbVehicleOwner.DisplayMember = "Supplier_Name";
            }
            

        }



        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Frieght_Advise(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                txtAdviseNo.Text = result.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtGRNNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtGRNNo.Text != "")
                {
                    var da = (from obj in db.Get_GRN_Data_for_Frieght(logIn.company,txtGRNNo.Text)
                                select new { 
                                  
                                  obj.Supplier_InvNo,obj.Inv_Qty, obj.supplier_name,
                                  obj.Transporter_Name,obj.Freight,
                                  obj.Vehicle_No,obj.Rec_Qty,obj.Accepted_qty, obj.prod_name
                              
                              }).ToList();

                    if (da.Count > 0)
                    {



                        txtSupplierInv.Text = da[0].Supplier_InvNo;
                        txtSupplier.Text = da[0].supplier_name.ToString();
                        txtInvQty.Text = da[0].Inv_Qty.ToString();
                        txtReceivedQty.Text = da[0].Rec_Qty.ToString();
                        txtAcceptedQty.Text = da[0].Accepted_qty.ToString();
                        txtFrieghtAmt.Text = Convert.ToString(da[0].Freight);
                        txtVehicleNo.Text = da[0].Vehicle_No;
                        cmbTransporter.Text = da[0].Transporter_Name;
                        txtItemName.Text = da[0].prod_name;
                        decimal f = Convert.ToDecimal(txtFrieghtAmt.Text);
                        decimal q = Convert.ToDecimal(txtAcceptedQty.Text);
                        txtFrieghtUnitRate.Text = "0";
                        if (f>0)
                        {
                            txtFrieghtUnitRate.Text = (f / q).ToString("0.00");
                        }
                        
                        



                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

       
        
        private void txtVehicleOwner_Leave(object sender, EventArgs e)
        {
            
        }

        private void txtFrieghtUnitRate_Leave(object sender, EventArgs e)
        {
            decimal f = Convert.ToDecimal(txtFrieghtUnitRate.Text);
            decimal q = Convert.ToDecimal(txtAcceptedQty.Text);
            //txtFrieghtUnitRate.Text = "0";
            if (f > 0)
            {
                txtFrieghtAmt.Text = (f * q).ToString("0.00");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AutoincrementId();
                txtGRNNo.Text = "";
                txtSupplier.Text = "";
                txtSupplierInv.Text = "";
                txtInvQty.Text = "";
                txtItemName.Text = "";
                txtReceivedQty.Text = "";
                txtAcceptedQty.Text = "";
                cmbTransporter.Text = "";
                txtVehicleNo.Text = "";
                txtTransportInvNo.Text = "";
                cmbVehicleOwner.Text = "";
                txtPANNo.Text = "";
                txtBankDetails.Text = "";                
                txtFrieghtAmt.Text = "";
                txtFrieghtUnitRate.Text = "";
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


                

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message + " While  clear Getting Error ", "Frieght Advise", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
               if ((from u in db.Frieght_Advises where u.Advise_No == txtAdviseNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    
                    db.sp_FrieghtAdvise_Delete(txtAdviseNo.Text, logIn.company, logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();      
                }
            
                Frieght_Advise S = new Frieght_Advise();
                {
                    
                    S.Advise_No = txtAdviseNo.Text  ;
                    S.Advise_Date = dtAdviseDate.Value;
                    S.GRN_No = txtGRNNo.Text;
                   
                   
                    S.Payment_To_Owner = chkPymtToOwner.Checked;

                    S.Accepted_Qty = (txtAcceptedQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAcceptedQty.Text);
                    S.Frieght_Unit_Rate = (txtFrieghtUnitRate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFrieghtUnitRate.Text);
                    S.Frieght_Amount = (txtFrieghtAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFrieghtAmt.Text);
                    
                    S.VehicleNo = (txtVehicleNo.Text == "") ? "" : txtVehicleNo.Text;
                    S.Transporter_Inv_No = (txtTransportInvNo.Text == "") ? "" : txtTransportInvNo.Text;
                     S.Transporter_Name = Convert.ToInt32(cmbTransporter.SelectedValue.ToString());
                    S.Vehicle_Owner = Convert.ToInt32(cmbVehicleOwner.SelectedValue.ToString());
                    S.PAN_No = txtPANNo.Text;
                    S.Bank_Details = txtBankDetails.Text;
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Frieght_Advises.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                
                
               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtAdviseNo.Text);
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

        private void cmbVehicleOwner_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbVehicleOwner.Text != "")
                {
                    var Prodname = (from m in db.Supplier_informations
                                    where m.Company_ID == logIn.company && m.ID == Convert.ToInt32(cmbVehicleOwner.SelectedValue)
                                    select new { m.PAN_No, m.BankName,m.BranchName,m.IFSCCode,m.AccNo }).ToList();

                    if (Prodname.Count > 0)
                    {
                        txtPANNo.Text = Prodname[0].PAN_No;
                        txtBankDetails.Text = Prodname[0].BankName + ", " + Prodname[0].AccNo + ", " + Prodname[0].IFSCCode  ;
                    }

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

        private void button3_Click(object sender, EventArgs e)
        {
            var d = (from data in db.SP_Get_FrieghtAdvises(logIn.company) select data).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Advise_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Advise_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Advise_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Advise_No"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["GRN_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["GRN_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["GRN_No"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["GRN_No"].FilterRowCondition = FilterRowCondition.Contains;

                groupBox1.Visible = true;

            }
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Advise_No"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {

                    txtAdviseNo.Text = cellVaue.ToString();
                    bindedit();
                    groupBox1.Visible = false;                   
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
        public void bindedit()
        {
            try
            {

                int QuoteMasterID = 0;
                var da = (from obj in db.Get_Frieght_Advise_for_Edit(logIn.company,txtAdviseNo.Text)
                         
                          select obj).ToList();
                if (da.Count > 0)
                {
                    
                    dtAdviseDate.Text = da[0].Advise_Date.ToString();
                    txtGRNNo.Text = da[0].Grn_NO.ToString();
                    txtSupplier.Text = da[0].supplier_name.ToString(); ;
                    txtSupplierInv.Text = da[0].Supplier_InvNo.ToString(); ;
                    txtInvQty.Text = da[0].Inv_Qty.ToString(); ;
                    txtItemName.Text = da[0].prod_name.ToString(); ;
                    txtReceivedQty.Text = da[0].Rec_Qty.ToString(); ;
                    txtAcceptedQty.Text = da[0].Accepted_qty.ToString(); ;
                    cmbTransporter.SelectedValue = da[0].Transporter_Name; 
                    txtVehicleNo.Text = da[0].Vehicle_No.ToString();
                    txtTransportInvNo.Text = da[0].Transporter_Inv_No.ToString();
                    cmbVehicleOwner.SelectedValue = da[0].Vehicle_Owner;
                    txtPANNo.Text = da[0].PAN_No.ToString();
                    txtBankDetails.Text = da[0].Bank_Details.ToString();
                    txtFrieghtAmt.Text = da[0].Frieght_Amount.ToString();
                    txtFrieghtUnitRate.Text = da[0].Frieght_Unit_Rate.ToString();
                }


               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
