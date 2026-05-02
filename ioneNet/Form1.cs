using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Ione_DAL;
namespace ioneNet
{
    public partial class frmMenuBoard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);



        public static string Module;
        public static Boolean finModule;
        public frmMenuBoard()
        {
            InitializeComponent();
        }

        private void OrderManagement_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Order Management";
            frm.Show();


        }

        private void btnAdministrator_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Administrator";
            frm.Show();
        }

        private void frmMenuBoard_Load(object sender, EventArgs e)
        {

            //String str = ConfigurationManager.ConnectionStrings["ioneNet.Properties.Settings.viswaSiOneConnectionString"].ConnectionString;
            //User Roles
            btnAdministrator.Enabled = true;
            btnFinanceManagement.Enabled = false;
            btnHRMS.Enabled = false;
            btnMM.Enabled = false;
            OrderManagement.Enabled = false;
            btmPlantMaint.Enabled = false;
            btnProduction.Enabled = false;
            btnQuality.Enabled = false;
            //btnService.Enabled = false;
            //btnProject.Enabled = false;
            finModule = false;

            var d = (from s in db.Company_Infos where s.Id == logIn.company select s).SingleOrDefault();
            if (d != null)
            {
                label3.Text = d.Alias_Name;
                
                //if (d.Company_Logo != null)
                //{
                //    var f = (from s1 in db.Company_Infos where s1.Id == logIn.company select s1);
                //    SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                //    DataTable dt = new DataTable();
                //    DataSet ds = new DataSet("MyImages");
                //    byte[] MyData = new byte[0];
                //    da.Fill(ds, "MyImages");
                //    DataRow myRow;
                //    myRow = ds.Tables["MyImages"].Rows[0];
                //    MyData = (byte[])myRow["Company_Logo"];
                //    MemoryStream stream = new MemoryStream(MyData);
                //    pictureBox3.Image = Image.FromStream(stream);
                //}
            }
           
            var uRoles = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID select new { m.Modules_Allowed }).Distinct().ToList();
            if (uRoles.Count > 0)
            {
                string s = uRoles[0].Modules_Allowed;
                string[] values = s.Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = values[j].Trim();
                    string m = values[j].ToString();
                    if(m=="Administrator")
                    {
                        btnAdministrator.Enabled = true;
                    }
                    if (m == "Finance Management")
                    {
                        btnFinanceManagement.Enabled = true;
                        finModule = true;
                    }
                    if (m == "Human Resource")
                    {
                        btnHRMS.Enabled = true;
                    }
                    if (m == "Material Management")
                    {
                        btnMM.Enabled = true;
                    }
                    if (m == "Order Management")
                    {
                        OrderManagement.Enabled = true;
                    }
                    if (m == "Plant Maintenance")
                    {
                        btmPlantMaint.Enabled = true;
                    }
                    if (m == "Production Management")
                    {
                        btnProduction.Enabled = true;
                    }
                    if (m == "Quality Management")
                    {
                        btnQuality.Enabled = true;
                    }
                    

                }
            }
        }

        private void frmMenuBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.Exit();
            Cursor.Current = Cursors.Default;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //Masters.imageSave frm = new Masters.imageSave();
            //frm.ShowDialog();
            this.Close();
        }

        private void btnMtrlManagement_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Material Management";
            frm.Show();
        }

        private void btnFinanceManagement_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Finance Management";
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Production Management";
            frm.Show();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "iOne Admin";
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Project Management";
            //frm.BackgroundImage = ;
            frm.Show();
        }

        private void btnHRMS_Click(object sender, EventArgs e)
        {

            frmMain frm = new frmMain();
            Module = "Human Resource";
            frm.Show();


            //SqlCommand cmd1 = new SqlCommand("update  [Company_Logged] set compname = @ProdID", con);
            //cmd1.Parameters.AddWithValue("@ProdID", logIn.company);

            //if (con.State != ConnectionState.Open)
            //    con.Open();
            ////con.Open();
            //cmd1.ExecuteNonQuery();
            //con.Close();
            //SqlCommand cmd2 = new SqlCommand("update  [Company_Logged] set BU_ID = @ProdID", con);
            //cmd2.Parameters.AddWithValue("@ProdID", logIn.BU_ID);

            //if (con.State != ConnectionState.Open)
            //    con.Open();
            ////con.Open();
            //cmd2.ExecuteNonQuery();
            //con.Close();
            //Process.Start("iOneHRMS.exe");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnQuality_Click(object sender, EventArgs e)
        {
            frmMain frm = new frmMain();
            Module = "Quality Management";
            //frm.BackgroundImage = ;
            frm.Show();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
