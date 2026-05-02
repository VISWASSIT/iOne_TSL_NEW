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
using System.Reflection;
using System.IO;
using System.Drawing.Imaging;
using Ione_DAL;
using ioneNet.OrderManagement.Transactions;

namespace ioneNet
{
    public partial class frmMain : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        //MenuStrip MnuStrip;
        public static string frmname;
        public static Image complogo;
        public static string menuName;
        public static string DBUserID, Password, ServerIP, Database;
        ToolStripMenuItem MnuStripItem;
        public frmMain()
        {
            InitializeComponent();
        }
        
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.IsMdiContainer = true;
                MenuStrip MnuStrip = new MenuStrip();
                this.Controls.Add(MnuStrip);
                string ModuleName = frmMenuBoard.Module;
                //Adding Main Menu


                String str = ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString;
                //string DBUserID = Session["DBUserID"].;

                DBUserID = str.Substring(str.IndexOf("User ID") + 8, str.IndexOf("Password") - str.IndexOf("User ID") - 9);
                Password = str.Substring(str.IndexOf("Password") + 9);
                ServerIP = str.Substring(str.IndexOf("Data Source") + 12, str.IndexOf("Initial Catalog") - str.IndexOf("Data Source") - 13);
                Database = str.Substring(str.IndexOf("Initial Catalog") + 16, str.IndexOf("User ID") - str.IndexOf("Initial Catalog") - 17);

                SqlCommand cmd = new SqlCommand("Select DISTINCT [Menu_Group] from [Menu_Items] where [Menu_Module] = @moduleName");

                cmd.Parameters.AddWithValue("@moduleName", ModuleName);
                cmd.Connection = con;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                con.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    MnuStripItem = new ToolStripMenuItem(dr["Menu_Group"].ToString());
                    SubMenu(MnuStripItem, dr["Menu_Group"].ToString(), ModuleName);
                    MnuStrip.Items.Add(MnuStripItem);
                }
                //MnuStrip.Font.Size = 10;
                //nuStrip.Font. = 10;                
                this.MainMenuStrip = MnuStrip;
                this.lblCompany.Text = logIn.BU_To_Show + '-' + logIn.F_Year;
                this.lblUser.Text = logIn.username;

                //Check User Form Level Acccess




                switch (ModuleName)
                {
                    case "Order Management":
                        //this.BackgroundImage = ioneNet.Properties.Resources.Blue_and_White_Watercolor_Castle_Wedding_Poster__2_;
                        ioneNet.OrderManagement.frmCRMDashBoard frm = new ioneNet.OrderManagement.frmCRMDashBoard();
                        frm.MdiParent = this;

                        frm.Show();
                        break;
                    case "Finance Management":
                        //  this.BackgroundImage = ioneNet.Properties.Resources.ioneBack_MM;
                        //ioneNet.FinanceManagement.frmFinanceDashBoard frm1 = new ioneNet.FinanceManagement.frmFinanceDashBoard();
                        //frm1.MdiParent = this;
                        //frm1.Show();
                        break;

                    case "Material Management":
                        //this.BackgroundImage = ioneNet.Properties.Resources.ioneBack_MM;
                        ioneNet.MaterialManagement.frmMMDashBoard frm2 = new ioneNet.MaterialManagement.frmMMDashBoard();
                        frm2.MdiParent = this;
                        frm2.Show();
                        break;
                    case "Production Management":
                        //this.BackgroundImage = ioneNet.Properties.Resources.ioneBack2024;
                        ioneNet.ProductionManagement.frmProd_Dashboard frm4 = new ioneNet.ProductionManagement.frmProd_Dashboard();
                        frm4.MdiParent = this;
                        frm4.Show();
                        break;
                    case "Human Resource":                        
                         ioneNet.HumanResourceManagement.HRMDashboard frm3 = new ioneNet.HumanResourceManagement.HRMDashboard();
                         frm3.MdiParent = this;
                         frm3.Show();
                         break;
                    case "Quality Management":
                        //this.BackgroundImage = ioneNet.Properties.Resources.Article_03;

                        break;
                }
                //var data = (from n in db.Company_Infos where n.Company_Name == logIn.compname select n).ToList();
                //if (data.Count > 0)
                //{


                //    if (data[0].Company_Logo != null)
                //    {
                //        var f = (from s in db.Company_Infos where s.Id == data[0].Id select s);
                //        SqlCommand cmd1 = (SqlCommand)db.GetCommand(f);
                //        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                //        DataTable dt1 = new DataTable();
                //        DataSet ds = new DataSet("MyImages");
                //        byte[] MyData = new byte[0];
                //        da.Fill(ds, "MyImages");
                //        DataRow myRow;
                //        myRow = ds.Tables["MyImages"].Rows[0];                            
                //        MyData = (byte[])myRow["Company_Logo"];                       
                //        MemoryStream stream = new MemoryStream(MyData);
                //        complogo = Image.FromStream(stream);
                //    }
                //    this.pictureBox1.Image = complogo;
                //    //Add Sub Menu

                //}

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void SubMenu(ToolStripMenuItem mnu, string submenu,string modname)

        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("UserMenuAccess ", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@rollID", logIn.UserRoleID);
                cmd2.Parameters.AddWithValue("@menugroup", submenu);
                cmd2.Parameters.AddWithValue("@menumodule", modname);


              //  String Seqchild = "SELECT [Menu_Item] FROM [Menu_Items] WHERE [Menu_Group]='" + submenu + "' and menu_module = '"+modname+ "' and (company_ID = '0' or company_ID = '"+logIn.company+"') order by Menu_DispOrder";
                //Seqchild.Parameters.AddWithValue("@moduleName", ModuleName);
                SqlDataAdapter dachildmnu = new SqlDataAdapter(cmd2);

                DataTable dtchild = new DataTable();

                dachildmnu.Fill(dtchild);



                foreach (DataRow dr in dtchild.Rows)

                {

                    ToolStripMenuItem SSMenu = new ToolStripMenuItem(dr["Menu_Item"].ToString(), null, new EventHandler(ChildClick));

                    mnu.DropDownItems.Add(SSMenu);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void ChildClick(object sender, EventArgs e)

        {
            try
            {

                // MessageBox.Show(string.Concat("You have Clicked ", sender.ToString(), " Menu"), "Menu Items Event",MessageBoxButtons.OK, MessageBoxIcon.Information);



                String Seqtx = "SELECT [FormToOpen],openmode,[Menu_ID] ,Menu_Item FROM [Menu_Items] WHERE [Menu_Item]='" + sender.ToString() + "'";

                SqlDataAdapter datransaction = new SqlDataAdapter(Seqtx, con);

                DataTable dtransaction = new DataTable();

                datransaction.Fill(dtransaction);

                Assembly frmAssembly = Assembly.LoadFile(Application.ExecutablePath);

                foreach (Type type in frmAssembly.GetTypes())

                {

                    //MessageBox.Show(type.Name);

                    if (type.BaseType == typeof(Form))

                    {

                        if (type.Name == dtransaction.Rows[0][0].ToString())

                        {

                            Form frmShow = (Form)frmAssembly.CreateInstance(type.ToString());

                            // then when you want to close all of them simple call the below code



                          //  foreach (Form form in this.MdiChildren)

//{

                           //     form.Close();

                          //  }



                            
                            if (dtransaction.Rows[0]["openmode"].ToString() == "FullScreen")
                              
                            {

                                

                                frmname = dtransaction.Rows[0]["Menu_ID"].ToString();
                                menuName = dtransaction.Rows[0]["Menu_Item"].ToString();
                                //MessageBox.Show(this.MdiParent.Name);
                                //  MessageBox.Show(this.ActiveMdiChild.Name);
                                //Form frmShow1 = (frmShow)this.Parent.Parent;
                                frmShow.MdiParent = this;
                                //this.
                                // frmShow.WindowState = FormWindowState.Maximized;
                                //tabForms.TabIndex = 0;
                                //string tp = this.ActiveMdiChild.Text;

                                //tabForms.SelectedTab = tp;
                                //if (frmShow is null)
                                //{
                                //    MessageBox.Show("OK");
                                //}
                                //else
                                //{
                                //Form frmShow1 = (Form)this.Parent.Parent;
                              //MessageBox.Show(frmMain.ActiveForm.Name);
                                frmShow.Show();
                                //}
                            }
                            else
                            {
                                frmname = dtransaction.Rows[0]["Menu_ID"].ToString();
                                menuName = dtransaction.Rows[0]["Menu_Item"].ToString();

                                frmShow.ShowDialog();                            //frmShow.ControlBox = false;
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void tabForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
            {
                // Minimize flicker when switching between tabs, by suspending layout
                SuspendLayout();
                (tabForms.SelectedTab.Tag as Form).SuspendLayout();
                Form activeMdiChild = this.ActiveMdiChild;
                if (activeMdiChild != null)
                    activeMdiChild.SuspendLayout();

                // Minimize flicker when switching between tabs, by changing to minimized state first
                if ((tabForms.SelectedTab.Tag as Form).WindowState != FormWindowState.Maximized)
                    (tabForms.SelectedTab.Tag as Form).WindowState = FormWindowState.Minimized;

                (tabForms.SelectedTab.Tag as Form).Select();

                // Resume layout again
                if (activeMdiChild != null && !activeMdiChild.IsDisposed)
                    activeMdiChild.ResumeLayout();
                (tabForms.SelectedTab.Tag as Form).ResumeLayout();
                ResumeLayout();
                (tabForms.SelectedTab.Tag as Form).Refresh();
            }
        }

        private void frmMain_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
                tabForms.Visible = false;
            // If no any child form, hide tabControl 
            else
            {
                this.ActiveMdiChild.WindowState =
                FormWindowState.Maximized;
                // Child form always maximized 

                // If child form is new and no has tabPage, 
                // create new tabPage 
                if (this.ActiveMdiChild.Tag == null || this.ActiveMdiChild.Tag == "")
                {
                    // Add a tabPage to tabControl with child 
                    // form caption 
                    TabPage tp = new TabPage(this.ActiveMdiChild
                                             .Text);
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = tabForms;
                    tabForms.SelectedTab = tp;

                    this.ActiveMdiChild.Tag = tp;
                    this.ActiveMdiChild.FormClosed +=
                        new FormClosedEventHandler(
                                        ActiveMdiChild_FormClosed);
                }

                if (!tabForms.Visible) tabForms.Visible = true;

            }
        }
        private void ActiveMdiChild_FormClosed(object sender,
                                    FormClosedEventArgs e)
        {
            ((sender as Form).Tag as TabPage).Dispose();
        }

        private void emplAllowencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //var f = new HumanResourceManagement.Transactions.frmVariable_Allowences_Entry() { MdiParent = this };
            //f.Show();
            ////HumanResourceManagement.Transactions.frmVariable_Allowences_Entry frm = new HumanResourceManagement.Transactions.frmVariable_Allowences_Entry();

            ////frmMain frm1 = new frmMain();
            ////frm1.IsMdiContainer = true;
            ////frm.MdiParent = frm1;
            ////frm.AutoSize = true;
            ////frm.Show();
        }

        private void employeemasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HumanResourceManagement.Masters.EmployeeList frm = new HumanResourceManagement.Masters.EmployeeList();
            //frm.MdiParent = this;
            //frm.Show();
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Masters.frmChangePassword frm = new Masters.frmChangePassword();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            foreach (Form c in this.MdiChildren)
            {
                c.Close();
            }

            Masters.ChangeCostingUnit frm = new Masters.ChangeCostingUnit();
            frm.MdiParent = this;
            frm.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (Form c in this.MdiChildren)
            {
                c.Close();
            }

            Masters.ChangeCostingUnit frm = new Masters.ChangeCostingUnit();
            frm.MdiParent = this;
            frm.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Masters.frmChangePassword frm = new Masters.frmChangePassword();
            //frm.MdiParent = this;
            frm.ShowDialog();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            ////Application.Exit();
            //Cursor.Current = Cursors.Default;
        }

        private void lblCompany_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

