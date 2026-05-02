using Ione_DAL;
using Syncfusion.WinForms.DataGrid;
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

namespace ioneNet.Masters
{
    public partial class frmChangePassword : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            txtUserName.Text = logIn.username;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            var deleteproduct = db.User_Setups.Single(course => course.User_ID == logIn.userID);
           
            deleteproduct.Password = txtNewPassword.Text;
           
            db.SubmitChanges();
            MessageBox.Show("Password Changed Successfully");
        }
    }
}
