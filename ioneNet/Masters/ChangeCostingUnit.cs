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
using static Syncfusion.Windows.Forms.Tools.NavigationView;
using Syncfusion.Windows.Forms.Tools.XPMenus;

namespace ioneNet.Masters
{
    public partial class ChangeCostingUnit : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ChangeCostingUnit()
        {
            InitializeComponent();
        }

        private void ChangeCostingUnit_Load(object sender, EventArgs e)
        {
            txtUserName.Text = logIn.username.ToString();
           int userID = logIn.userID;

            //Get Business Unit name
            var bindBU = (from m in db.User_Setups
                          where m.User_ID == userID
                          select new
                          {
                              m.BU_Allowed,
                              m.FY_Allowed
                          }).ToList();

            if (bindBU.Count > 0)
            {
                cmbBU_Name.Items.Clear();
                string s = bindBU[0].BU_Allowed;
                string[] values = s.Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = values[j].Trim();
                    string m = values[j].ToString();
                    cmbBU_Name.Items.Add(m.ToString());
                }
                cmbBU_Name.Text = logIn.BU_Name;


                cmbFYear.Items.Clear();
                string s1 = bindBU[0].FY_Allowed;
                string[] values1 = s1.Split(',');
                for (int j = 0; j < values1.Length; j++)
                {
                    values1[j] = values1[j].Trim();
                    string m = values1[j].ToString();
                    cmbFYear.Items.Add(m.ToString());
                }
                cmbFYear.Text = logIn.F_Year;
                //if (cmbFYear.Items.Count == 1)
                //    cmbFYear.SelectedIndex = 0;
                //else
                //    cmbFYear.SelectedIndex = -1;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logIn.BU_Name = cmbBU_Name.Text;
            logIn.F_Year = cmbFYear.Text;

            //Get Financial Year and Dates
            var bindComp = (from m in db.Financial_Year_Masters
                            where m.F_Year == cmbFYear.Text && m.Company_ID == logIn.company
                            select new
                            {
                                m.Start_Date,
                                m.End_Date,
                                m.F_Year,
                            }).ToList();

            logIn.fy_Start_Date = Convert.ToDateTime(bindComp[0].Start_Date);
            logIn.fy_End_Date = Convert.ToDateTime(bindComp[0].End_Date);


            var bindBU = (from m in db.Costing_Units
                          where m.BU_Name == cmbBU_Name.Text && m.Company == logIn.company
                          select new
                          {
                              m.id,
                              m.BU_Name,
                              m.PT_Reg_No

                          }).ToList();
            logIn.BU_ID = bindBU[0].id;
            //BU_Name = cmbBU_Name.Text;
            logIn.BU_To_Show = bindBU[0].BU_Name;
            //logIn.Group_BU_ID = Convert.ToInt32(bindBU[0].PT_Reg_No);
            string ModuleName = frmMenuBoard.Module;
            switch (ModuleName)
            {
                case "Order Management":
                    //this.BackgroundImage = ioneNet.Properties.Resources.ioneBack_Order;
                    ioneNet.OrderManagement.frmCRMDashBoard frm = new ioneNet.OrderManagement.frmCRMDashBoard();
                    frm.MdiParent = this.MdiParent;

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
                    frm2.MdiParent = this.MdiParent;
                    frm2.Show();
                    break;
                case "Production Management":
                    //this.BackgroundImage = ioneNet.Properties.Resources.ioneBack2024;
                    break;
                case "Human Resource":
                    ioneNet.HumanResourceManagement.HRMDashboard frm3 = new ioneNet.HumanResourceManagement.HRMDashboard();
                    frm3.MdiParent = this.MdiParent;
                    frm3.Show();
                    break;
            }
            this.Close();

            //frmMain.lblCompany.Text = logIn.BU_To_Show + '-' + logIn.F_Year;
        }
    }
}
