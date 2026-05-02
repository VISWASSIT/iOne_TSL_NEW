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
using Ione_DAL;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class SO_MultiDeliveryAddress : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public SO_MultiDeliveryAddress()
        {
            InitializeComponent();
        }

        private void dgDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgDetails.CurrentCell.ColumnIndex;
                string columnName = dgDetails.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "Customer Name")
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
                DataGridViewRow R1 = dgDetails.Rows[dgDetails.CurrentRow.Index];

                int columnIndex = dgDetails.CurrentCell.ColumnIndex;
                string columnName = dgDetails.Columns[columnIndex].HeaderText;

                if (columnName == "Customer Name")
                {
                    var Prodname = (from d in db.Supplier_informations select new { d.Supplier_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Supplier_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Supplier_Name);
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

        private void dgDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            DataGridViewRow R1 = dgDetails.Rows[dgDetails.CurrentRow.Index];
            int columnIndex = dgDetails.CurrentCell.ColumnIndex;
            int RowIndex = dgDetails.CurrentCell.RowIndex;
            string columnName = dgDetails.Columns[columnIndex].Name;
            if (columnName == "Consignee_Name")
            {
                var d1 = (from a in db.Supplier_informations where a.Supplier_Name == R1.Cells["Consignee_Name"].Value.ToString() && a.Company_ID == logIn.company select new { a.ID, a.GSTIN_NO }).ToList();
                if (d1.Count > 0)
                {
                    R1.Cells["Con_GSTINNO"].Value = d1[0].GSTIN_NO;
                    R1.Cells["Cust_ID"].Value = d1[0].ID;
                }
            }
            if(columnName == "Ord_Qty")
            {
                decimal x = 0,y=0,z=0;
                y = Convert.ToDecimal(txtOrdQty.Text);
                

                for (int i = 0; i < dgDetails.Rows.Count - 1; i++)
                {

                    x += (dgDetails.Rows[i].Cells["Ord_Qty"].Value == "" || dgDetails.Rows[i].Cells["Ord_Qty"].Value == null || dgDetails.Rows[i].Cells["Ord_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgDetails.Rows[i].Cells["Ord_Qty"].Value);
                  
                }

                if (x > y)
                {
                    MessageBox.Show("Planned Qty Cannot Be Greater Than Order Qty");

                    //dgDetails.CurrentCell = dgDetails.Rows[RowIndex].Cells["Ord_Qty"];
                    dgDetails.Rows[RowIndex].Cells["Ord_Qty"].Selected = true;
                    //dgDetails.Focus;
                    
                        

                }
                else
                {
                    txtPlannedQty.Text = x.ToString(".00");
                }

            }
        }

        private void SO_MultiDeliveryAddress_Load(object sender, EventArgs e)
        {
            try
            {
                txtSONo.Text = frmNewOrder_TSL.SONo;
                txtItemNo.Text = frmNewOrder_TSL.ItemCode;
                txtOrdQty.Text = frmNewOrder_TSL.OrdQty;
                var dm1 = (from s in db.Sales_Order_Delivery_Addresses
                           where s.SO_NO == txtSONo.Text && s.Item_Sno == Convert.ToInt32(txtItemNo.Text) && s.Company_ID == logIn.company
                           select new
                           {
                               Consignee_Name = s.ConsigneeName,
                               Cust_ID = s.ConsigneeCode,
                               Con_GSTINNO = s.Con_GSTINNo,
                               Ord_Qty = s.Del_Qty,

                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                {
                    dgDetails.DataSource = dtr;
                }
                decimal x = 0;
                for (int i = 0; i < dgDetails.Rows.Count - 1; i++)
                {

                    x += (dgDetails.Rows[i].Cells["Ord_Qty"].Value == "" || dgDetails.Rows[i].Cells["Ord_Qty"].Value == null || dgDetails.Rows[i].Cells["Ord_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgDetails.Rows[i].Cells["Ord_Qty"].Value);

                }
                txtPlannedQty.Text = x.ToString(".00");
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItemNo.Text);
                myString = txtSONo.Text;
                if ((from u in db.Sales_Order_Delivery_Addresses where u.SO_NO == myString && u.Item_Sno  == Icode &&  u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSONo.Text;
                    
                    db.sp_SO_DeliveryData_Delete(myString, Icode, logIn.company);
                }
                else
                {
                    
                   // myString = txtSoNo.Text;

                }
                decimal y, x;
                y = Convert.ToDecimal(txtOrdQty.Text);
                x = Convert.ToDecimal(txtPlannedQty.Text);

                if (x != y)
                {
                    MessageBox.Show("Order Qty and Planned Qty Should be Equal");
                }
                else
                {
                    dgDetails.Enabled = false;
                    //db.Transaction = transaction;
                    for (int i = 0; i < dgDetails.RowCount - 1; i++)
                    {
                        Sales_Order_Delivery_Address SC = new Sales_Order_Delivery_Address();

                        SC.SO_NO = myString;
                        SC.Item_Sno = Convert.ToInt32(txtItemNo.Text);
                        SC.OrdQty = Convert.ToDecimal(txtOrdQty.Text);
                        SC.ConsigneeName = (dgDetails.Rows[i].Cells["Consignee_Name"].Value == null) ? "" : (dgDetails.Rows[i].Cells["Consignee_Name"].Value).ToString();
                        SC.ConsigneeCode = Convert.ToInt32(dgDetails.Rows[i].Cells["Cust_ID"].Value);
                        SC.Con_GSTINNo = (dgDetails.Rows[i].Cells["Con_GSTINNO"].Value == null) ? "" : (dgDetails.Rows[i].Cells["Con_GSTINNO"].Value).ToString();
                        SC.Del_Qty = (dgDetails.Rows[i].Cells["Ord_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgDetails.Rows[i].Cells["Ord_Qty"].Value);


                        SC.Company_ID = logIn.company;
                        db.Sales_Order_Delivery_Addresses.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    //transaction.Commit();               
                    MessageBox.Show("Delivery Details Updated Sucessfully");
                    this.Close();
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
}
