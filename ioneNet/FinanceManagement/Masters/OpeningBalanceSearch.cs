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

namespace ioneNet.FinanaceManagement.Masters
{
    public partial class OpeningBalanceSearch : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string date, voucherNo;
        public OpeningBalanceSearch()
        {
            InitializeComponent();
        }

        private void CashPaymentSearch_Load(object sender, EventArgs e)
        {
            try
            {
                var s = (from ob in db.Account_Opening_Balances where ob.Company_ID == logIn.company && ob.BU_ID == logIn.BU_ID select new { ob.Voucher_no, ob.OB_date }).Distinct();
                dgBankPaymentsearch.DataSource = s;
              
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

       

        private void dgcashPaymentsearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    voucherNo = dgBankPaymentsearch.Rows[e.RowIndex].Cells["Voucher_no"].Value.ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Close();

                }
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                txtVoucherNo.Text = "";
                var s = (from ob in db.Account_Opening_Balances where ob.Company_ID == logIn.company select new { ob.Voucher_no, ob.OB_date }).Distinct();
                dgBankPaymentsearch.DataSource = s;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgBankPaymentsearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string pcode = "%" + txtVoucherNo.Text + "%";
                var s = from ob in db.Account_Opening_Balances where ob.Company_ID == logIn.company && ob.Voucher_no.Contains(txtVoucherNo.Text) select new { ob.Voucher_no, ob.OB_date };
                if (s.Count() > 0)
                {


                    dgBankPaymentsearch.DataSource = s;
                }
                else
                {

                    MessageBox.Show("Records Not Found");
                    return;
                }
             

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
