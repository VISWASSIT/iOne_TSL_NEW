using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Masters
{
    public partial class BomSearch : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public BomSearch()
        {
            InitializeComponent();
        }
        
        public static string FInname, receipcode;

        private void BomSearch_Load(object sender, EventArgs e)
        {
            try
            {
                var sa = (from a in db.BOMLists where a.Company_ID == logIn.company                      
                          select new {
                              Item_Code= a.Prod_Code,
                              Item_Name = a.Prod_Name,
                              Item_Group =a.FG_Group_Name,
                              Item_UOM = a.FG_Uom,
                              Receipe_Code =a.Bom_ReceipeCode,
                              LotBatchsize = a.Bom_Batchsize }).Distinct().ToList();
                if(sa.Count>0)
                {
                    dgvBOMData.DataSource = sa;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvBOMData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {

                    var sa = (from a in db.BOMLists
                              where a.Company_ID == logIn.company
                               && a.Prod_Code.Contains(txtSearch.Text) || a.Prod_Name.Contains(txtSearch.Text)
                              select new
                              {
                                  Item_Code = a.Prod_Code,
                                  Item_Name = a.Prod_Name,
                                  Item_Group = a.FG_Group_Name,
                                  Item_UOM = a.FG_Uom,
                                  Receipe_Code = a.Bom_ReceipeCode,
                                  LotBatchsize = a.Bom_Batchsize
                              }).Distinct().ToList();
                    if (sa.Count > 0)
                    {
                        dgvBOMData.DataSource = sa;
                    }

                   

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {

                var sa = (from a in db.BOMLists
                          where a.Company_ID == logIn.company
                           && a.Prod_Code.Contains(txtSearch.Text) || a.Prod_Name.Contains(txtSearch.Text)
                          select new
                          {
                              Item_Code = a.Prod_Code,
                              Item_Name = a.Prod_Name,
                              Item_Group = a.FG_Group_Name,
                              Item_UOM = a.FG_Uom,
                              Receipe_Code = a.Bom_ReceipeCode,
                              LotBatchsize = a.Bom_Batchsize
                          }).Distinct().ToList();
                if (sa.Count > 0)
                {
                    dgvBOMData.DataSource = sa;
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var sa = (from a in db.BOMLists
                          where a.Company_ID == logIn.company
                          select new
                          {
                              Item_Code = a.Prod_Code,
                              Item_Name = a.Prod_Name,
                              Item_Group = a.FG_Group_Name,
                              Item_UOM = a.FG_Uom,
                              Receipe_Code = a.Bom_ReceipeCode,
                              LotBatchsize = a.Bom_Batchsize
                          }).Distinct().ToList();
                if (sa.Count > 0)
                {
                    dgvBOMData.DataSource = sa;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FInname = dgvBOMData.Rows[e.RowIndex].Cells["Item_Name"].Value.ToString();
                receipcode = dgvBOMData.Rows[e.RowIndex].Cells["Receipe_Code"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }
    }
}
