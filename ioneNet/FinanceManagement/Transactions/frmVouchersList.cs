using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.FinanceManagement.Transactions
{
   
    public partial class frmVouchersList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string vchno;
        public frmVouchersList()
        {
            InitializeComponent();
        }

        private void frmVouchersList_Load(object sender, EventArgs e)
        {
            bindmethod();
        }
        public void bindmethod()
        {
            try
            {
                var d = (from data in db.BindAccountVoucherList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID, ioneNet.FinanaceManagement.AccountVoucher.vochertype) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Voucher_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Voucher_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Voucher_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Voucher_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Doc_Ref_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Doc_Ref_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Doc_Ref_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Doc_Ref_No"].FilterRowCondition = FilterRowCondition.Contains;

                  
                }

                //var p = (from s in db.Voucher_Summary_Reports
                //         where s.Voucher_Type == ioneNet.FinanaceManagement.AccountVoucher.vochertype && s.Company_ID == logIn.company
                //         && s.Voucher_Date >= logIn.fy_Start_Date && s.Voucher_Date <= logIn.fy_End_Date
                //         orderby s.Voucher_Date,s.Voucher_No
                //         select new
                //         {
                //             s.Voucher_Type,
                //             s.Voucher_No,
                //             s.Voucher_Date,
                //             s.Doc_Ref_No,
                //             s.Debit,
                //             s.Credit

                //         }
                //        );
                //SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                //DataTable dt1 = new DataTable();
                //da1.Fill(dt1);

                //if (dt1.Rows.Count >= 0)
                //{
                //    dgList.DataSource = dt1;
                //}
                //else
                //{
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bindmethod();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var p = (from s in db.Voucher_Summary_Reports
                         where s.Voucher_Type == ioneNet.FinanaceManagement.AccountVoucher.vochertype && s.Company_ID == logIn.company
                         && s.Voucher_Date >= logIn.fy_Start_Date && s.Voucher_Date <= logIn.fy_End_Date
                         && s.Voucher_No == textBox1.Text
                         orderby s.Voucher_Date, s.Voucher_No
                         select new
                         {
                             s.Voucher_Type,
                             s.Voucher_No,
                             s.Voucher_Date,
                             s.Doc_Ref_No,
                             s.Debit,
                             s.Credit

                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dgList.DataSource = dt1;
                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                //if (e.RowIndex >= 0)
                //{

                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    // int k = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns[1].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


                    vchno = cellVaue.ToString(); 
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    //CRM.frmAddNewCustomer frmcust = new frmAddNewCustomer();
                    //frmcust.MdiParent = this.ParentForm;
                    //frmcust.Show();
                    //FinanaceManagement.Masters.frmAddNewCustomer form = new OrderManagement.Masters.frmAddNewCustomer();
                    ////var = "1";
                    //form.ShowDialog();

                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One Custome");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
