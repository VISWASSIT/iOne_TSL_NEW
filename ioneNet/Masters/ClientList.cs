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
namespace ioneNet.Masters
{
    public partial class ClientList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string comp_id, comp_name;
        public ClientList()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            comp_id = "";
            comp_name = "";

            Masters.ClientRegistration frm = new Masters.ClientRegistration();
            //frm.MdiParent = this.MdiParent;

            frm.ShowDialog();
        }

        private void CompanyList_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'ioneDataSet.Client_Info' table. You can move, or remove it, as needed.
            this.client_InfoTableAdapter.Fill(this.ioneDataSet.Client_Info);
            // TODO: This line of code loads data into the 'ioneDataSet.Company_Info' table. You can move, or remove it, as needed.
            //this.company_InfoTableAdapter.Fill(this.ioneDataSet.Company_Info,logIn.company);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                comp_id = agvCompList.Rows[agvCompList.CurrentRow.Index].Cells[0].Value.ToString();
                comp_name = agvCompList.Rows[agvCompList.CurrentRow.Index].Cells[1].Value.ToString();
                Masters.ClientRegistration frm = new Masters.ClientRegistration();
                //frm.MdiParent = this.MdiParent;

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
