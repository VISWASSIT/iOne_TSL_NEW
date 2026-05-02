using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Ione_DAL;
namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class frmHolidays : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmHolidays()
        {
            InitializeComponent();


        }

        private void frmHolidays_Load(object sender, EventArgs e)
        {

        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)

        {


        }


        private void picker_ValueChanged(object sender, EventArgs e)

        {


        }


        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)

        {


        }



        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)

        {
            try
            {

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].Name;
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];
                if (columnName == "H_Date")
                {
                    string eDate = R1.Cells["H_Date"].Value.ToString();
                    //DateTime t = Convert.ToDateTime(R1.Cells["H_Date"].Value.ToString());

                    DateTime D = DateTime.ParseExact(eDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if ((from u in db.HR_holidays where u.year == cmbYear.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {

                    db.sp_DeleteHolidays(cmbYear.Text, logIn.company, logIn.BU_ID);
                }


                //db.Transaction = transaction;
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    HR_holiday SC = new HR_holiday();
                    SC.year = cmbYear.Text;
                    SC.h_event = (dataGridView1.Rows[i].Cells["Description"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Description"].Value).ToString();

                    if (dataGridView1.Rows[i].Cells["H_Date"].Value.ToString() != "")
                    {
                        //DateTime t = Convert.ToDateTime(dataGridView1.Rows[i].Cells["H_Date"].Value.ToString());

                        string eDate = dataGridView1.Rows[i].Cells["H_Date"].Value.ToString();
                        DateTime D = DateTime.ParseExact(eDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        SC.h_date = D;// DateTime.ParseExact(eDate, "MM/dd/yyyy", null); ;
                    }
                    SC.Company_ID = logIn.company;
                    SC.BU_ID = logIn.BU_ID;
                    db.HR_holidays.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully");
                //this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void cmbYear_Leave(object sender, EventArgs e)
        {
            try
            {
               
                if ((from u in db.HR_holidays where u.year == cmbYear.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {

                    SqlCommand cmd2 = new SqlCommand("sp_getHolidays", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@year", cmbYear.Text);
                    cmd2.Parameters.AddWithValue("@company_id", logIn.company);
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);                   
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);

                    dataGridView1.DataSource = ds2;
                    //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataTable dtr = new DataTable();
                    //da2.Fill(dtr);
                    //if (dtr.Rows.Count >= 0)
                    //dataGridView1.DataSource = dm1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
    }
}
