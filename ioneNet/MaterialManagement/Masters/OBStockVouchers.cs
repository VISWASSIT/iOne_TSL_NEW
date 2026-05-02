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
namespace ioneNet.MaterialManagement.Masters
{
    public partial class OBStockVouchers : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string date, voucherNo;
        public OBStockVouchers()
        {
            InitializeComponent();
        }

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    voucherNo = dgList.Rows[e.RowIndex].Cells["Voucher_no"].Value.ToString();

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

        private void OBStockVouchers_Load(object sender, EventArgs e)
        {
            try
            {
                var s = (from ob in db.Show_OB_Vouchers(logIn.company,logIn.BU_ID)
                       // join sl in db.Storage_Locations on ob.Storage_Location equals sl.Storage_Loc_Id 
                          select new { ob.Voucher_no, ob.OB_date, ob.Prod_Grade, ob.Storage_Location }).ToList();
                dgList.DataSource = s;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
