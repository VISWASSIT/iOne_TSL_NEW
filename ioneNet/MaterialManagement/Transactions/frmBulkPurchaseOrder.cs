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
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using Ione_DAL;
using Syncfusion.Windows.Forms.Tools;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmBulkPurchaseOrder : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmBulkPurchaseOrder()
        {
            InitializeComponent();
        }

        private void frmBulkPurchaseOrder_Load(object sender, EventArgs e)
        {

        }
    }
}
