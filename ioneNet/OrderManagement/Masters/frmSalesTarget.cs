using Ione_DAL;
using OpenCvSharp;
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

namespace ioneNet.OrderManagement.Masters
{
    public partial class frmSalesTarget : Form
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmSalesTarget()
        {
            InitializeComponent();
        }

        private void frmSalesTarget_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");



        }

        private void dgSalesExe_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                DataGridViewRow R1 = dgSalesExe.Rows[dgSalesExe.CurrentRow.Index];
                int columnIndex = dgSalesExe.CurrentCell.ColumnIndex;
                string columnName = dgSalesExe.Columns[columnIndex].Name;

                
                if (columnName == "Sale_Exe_Name")
                {
                                    
                    var State = (from c1 in db.Sales_Men_Informations                                                               
                                 where c1.Sales_Executive_Name == R1.Cells["Sale_Exe_Name"].Value
                                 select new { c1.Salesmen_Code,}).ToList();
                    if (State.Count > 0)
                    {
                        R1.Cells["Sale_Exe_Code"].Value = State[0].Salesmen_Code;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Sales Executive Name Entered");
                    }


                    

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

        private void dgSalesExe_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgSalesExe.CurrentCell.ColumnIndex;
                string columnName = dgSalesExe.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                
                if (tb3 != null && columnName == "Executive Name")
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
                DataGridViewRow R1 = dgSalesExe.Rows[dgSalesExe.CurrentRow.Index];

                int columnIndex = dgSalesExe.CurrentCell.ColumnIndex;
                string columnName = dgSalesExe.Columns[columnIndex].HeaderText;

                if (columnName == "Executive Name")
                {
                    var Salename = (from d in db.Sales_Men_Informations where d.Company_ID == logIn.company select new { d.Sales_Executive_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Sales_Executive_Name");
                    foreach (var name in Salename)
                    {
                        dt.Rows.Add(name.Sales_Executive_Name);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                
                Save();
            }
            catch (Exception)
            {


            }
        }
        private void Save()
        {
            try
            {

                SqlCommand cmd1 = new SqlCommand("delete  from [Sales_Target] where Target_Month =@Month and Target_Year = @Year ", con);
                cmd1.Parameters.AddWithValue("@Month", comboBox1.Text);
                cmd1.Parameters.AddWithValue("@Year", comboBox2.Text);

                if (con.State != ConnectionState.Open)
                    con.Open();
                //con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();

                for (int i = 0; i < dgSalesExe.RowCount - 1; i++)
                {
                    Sales_Target s = new Sales_Target();
                    var d1 = (from a in db.Sales_Men_Informations where a.Salesmen_Code == dgSalesExe.Rows[i].Cells["Sale_Exe_Code"].Value && a.Company_ID == logIn.company select new { a.Id }).ToList();

                    s.Target_Month = comboBox1.Text;
                    s.Target_Year = comboBox2.Text;
                    s.Sale_Exe_Id = d1[0].Id;
                    s.Target_Qty_Credit = (dgSalesExe.Rows[i].Cells["Target_Qty_Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSalesExe.Rows[i].Cells["Target_Qty_Credit"].Value);
                    s.Target_Qty_Advance = (dgSalesExe.Rows[i].Cells["Target_Qty_Advance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSalesExe.Rows[i].Cells["Target_Qty_Advance"].Value);
                    s.New_Customer_Target = Convert.ToInt32(dgSalesExe.Rows[i].Cells["New_Customer_Target"].Value);
                    s.New_Vendor_Reg = Convert.ToInt32(dgSalesExe.Rows[i].Cells["New_Vendor_Reg"].Value);
                    s.Company_ID = logIn.company;
                    s.Created_By = lblCreatedBy.Text;
                    s.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Sales_Targets.InsertOnSubmit(s);

                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully ");
                this.Close();
            }
            catch (Exception)
            {


            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if(comboBox1.Text !="")
            {
                if(comboBox2.Text !="")
                {
                    var dm1 = (from s in db.Sales_Targets
                                     join a in db.Sales_Men_Informations on s.Sale_Exe_Id equals a.Id
                               where s.Target_Month == comboBox1.Text && s.Company_ID == logIn.company && s.Target_Year == comboBox2.Text
                               select new
                               {
                                   Sale_Exe_Code = a.Salesmen_Code,
                                   Sale_Exe_Name = a.Sales_Executive_Name,
                                   Target_Qty_Credit = s.Target_Qty_Credit,
                                   Target_Qty_Advance = s.Target_Qty_Advance,
                                   s.New_Customer_Target,
                                   s.New_Vendor_Reg



                               });

                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgSalesExe.DataSource = dtr;
                }
            }
        }
    }


}
