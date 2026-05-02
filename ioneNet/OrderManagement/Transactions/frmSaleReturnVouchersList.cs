using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmSaleReturnVouchersList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string date, voucherNo;
        public frmSaleReturnVouchersList()
        {
            InitializeComponent();
        }

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    voucherNo = dgList.Rows[e.RowIndex].Cells["Vch_No"].Value.ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmSaleReturnVouchersList_Load(object sender, EventArgs e)
        {
            try
            {
                var s = (from ob in db.SaleReturns_Masters
                         join sl in db.Supplier_informations on ob.BuyerName equals sl.ID
                         where ob.Company_ID == logIn.company && ob.BU_ID == logIn.BU_ID
                         && ob.Vch_Date >= logIn.fy_Start_Date && ob.Vch_Date <= logIn.fy_End_Date

                         select new { ob.Vch_No, ob.Vch_Date, sl.Supplier_Name }).Distinct();
                dgList.DataSource = s;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
