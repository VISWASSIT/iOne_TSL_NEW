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

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmRFQList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string date, voucherNo;
        public frmRFQList()
        {
            InitializeComponent();
        }

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    voucherNo = dgList.Rows[e.RowIndex].Cells["RFQ_No"].Value.ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void frmRFQList_Load(object sender, EventArgs e)
        {
            try
            {
                var s = (from ob in db.Req_Quation_Masters
                         where ob.Company_ID == logIn.company && ob.BU_ID == logIn.BU_ID
                         && ob.Q_Date >= logIn.fy_Start_Date && ob.Q_Date <= logIn.fy_End_Date
                         select new { ob.RFQ_No, ob.Q_Date, ob.Basis }).Distinct();
                dgList.DataSource = s;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
