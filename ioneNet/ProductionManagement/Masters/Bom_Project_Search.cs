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
namespace ioneNet.ProductionManagement.Masters
{
    public partial class Bom_Project_Search : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static string ModuleNo, MoNO, Proj_Code, Group_Name;
        public static string frmName;
        public Bom_Project_Search()
        {
            InitializeComponent();
        }

        public static string FInname, receipcode;

        private void BomSearch_Load(object sender, EventArgs e)
        {
            try
            {
                if (frmName == "BOM-STD")
                {
                    var sa = (from a in db.BOM_Projects
                              where a.Company_ID == logIn.company && a.MO_No == "STD"
                              join p in db.Project_code_Masters on a.Project_Id equals p.id
                              join b in db.Product_Groups on a.Material_Group equals b.ID
                              select new
                              {
                                  a.MO_No,
                                  a.Project_Id,                                  
                                  Project_Code = p.Project_Code,
                                  b.Prod_Group_Name,
                                  a.Module_No
                              }).Distinct().ToList();
                    if (sa.Count > 0)
                    {
                        dgvBOMData.DataSource = sa;
                    }
                }
                if (frmName == "BOM-PROJECT")
                {
                    var sa = (from a in db.BOM_Projects
                              where a.Company_ID == logIn.company && a.MO_No !="STD"
                              join p in db.Project_code_Masters on a.Project_Id equals p.id
                              join b in db.Product_Groups on a.Material_Group equals b.ID
                              select new
                              {
                                  a.MO_No,
                                  a.Project_Id,
                                  Project_Code = p.Project_Code ,
                                  b.Prod_Group_Name,
                                  a.Module_No
                              }).Distinct().ToList();
                    if (sa.Count > 0)
                    {
                        dgvBOMData.DataSource = sa;
                    }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MoNO = dgvBOMData.Rows[e.RowIndex].Cells["MO_No"].Value.ToString();
                Proj_Code = dgvBOMData.Rows[e.RowIndex].Cells["Project_Id"].Value.ToString();
                Group_Name = dgvBOMData.Rows[e.RowIndex].Cells["Prod_Group_Name"].Value.ToString();
                ModuleNo = dgvBOMData.Rows[e.RowIndex].Cells["Module_No"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }
    }
}
