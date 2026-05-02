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
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmStockAdjustmentList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string vdate, voucherNo, Modifiedby,Createdby;
        public frmStockAdjustmentList()
        {
            InitializeComponent();
        }

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    voucherNo = dgList.Rows[e.RowIndex].Cells["Slip_No"].Value.ToString();
                    vdate = dgList.Rows[e.RowIndex].Cells["Slip_Date"].Value.ToString();
                    if (dgList.Rows[e.RowIndex].Cells["Modified_By"].Value != null)
                    {
                        Modifiedby = dgList.Rows[e.RowIndex].Cells["Modified_By"].Value.ToString();
                    }
                    else
                    {
                        Modifiedby = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    }
                    if (dgList.Rows[e.RowIndex].Cells["Created_By"].Value != null)
                    {
                        Createdby = dgList.Rows[e.RowIndex].Cells["Created_By"].Value.ToString();
                    }
                    else
                    {
                        Createdby = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    var s = (from ob in db.StockAdjustments
                             where ob.Company_ID == logIn.company && ob.BU_ID == logIn.BU_ID
                             && ob.Slip_Date >= logIn.fy_Start_Date && ob.Slip_Date <= logIn.fy_End_Date
                             && ob.Slip_NO.Contains(txtSearch.Text)
                             select new { ob.Slip_NO, ob.Slip_Date }).Distinct();

                                     

                    SqlCommand cmd1 = (SqlCommand)db.GetCommand(s);
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
        }

        private void dgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmStockAdjustmentList_Load(object sender, EventArgs e)
        {
            try
            {
                var s = (from ob in db.StockAdjustments
                         where ob.Company_ID == logIn.company && ob.BU_ID == logIn.BU_ID
                         && ob.Slip_Date >= logIn.fy_Start_Date && ob.Slip_Date <= logIn.fy_End_Date
                         select new { ob.Slip_NO, ob.Slip_Date,ob.Created_By,ob.Modified_By }).Distinct();
                dgList.DataSource = s;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
