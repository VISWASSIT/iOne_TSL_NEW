using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.Plant_Maintenance
{
    public partial class EquipmentList : Form
    {
        public EquipmentList()
        {
            InitializeComponent();
        }

        private void EquipmentList_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'laksazko_laksana_SRKDataSet.Asset_Master_View' table. You can move, or remove it, as needed.
            this.asset_Master_ViewTableAdapter.Fill(this.laksazko_laksana_SRKDataSet.Asset_Master_View);
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)
        {
            this.assetMasterViewBindingSource.Filter = advancedDataGridView1.FilterString;
        }

        private void advancedDataGridView1_SortStringChanged(object sender, EventArgs e)
        {
            this.assetMasterViewBindingSource.Sort = advancedDataGridView1.SortString;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Plant_Maintenance.EquipmentMaster frm = new Plant_Maintenance.EquipmentMaster();
           
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
