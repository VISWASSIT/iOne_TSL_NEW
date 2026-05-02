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
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Configuration;

namespace ioneNet.FinanceManagement.Transactions
{
    public partial class StockUpdation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        DateTime fy_Start_Date;
        public StockUpdation()
        {
            InitializeComponent();
        }

        private void StockUpdation_Load(object sender, EventArgs e)
        {
            var pStatus = (from m in db.Financial_Year_Masters where m.Company_ID == logIn.company select new { m.id, m.F_Year }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbFYear.DataSource = pStatus;
                cmbFYear.ValueMember = "id";
                cmbFYear.DisplayMember = "F_Year";
            }

            var d = (from po in db.AccountMasters
                     join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                     where po.Company_ID == logIn.company && A.GroupType == "Asset"
                     select new { po.id, po.AccName }).Distinct().ToList();
            if (d.Count > 0)
            {
                cmbAccName.DataSource = d;
                cmbAccName.ValueMember = "id";
                cmbAccName.DisplayMember = "AccName";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Get Financial Year and Dates
            var bindComp = (from m in db.Financial_Year_Masters
                            where m.F_Year == cmbFYear.Text && m.Company_ID == logIn.company
                            select new
                            {
                                m.Start_Date,
                                m.End_Date,
                                m.F_Year,
                            }).ToList();

            fy_Start_Date = Convert.ToDateTime(bindComp[0].Start_Date);
            DateTime fy_End_Date = Convert.ToDateTime(bindComp[0].End_Date);

            
            
            var stock = (from data in db.sp_Get_StockValues (logIn.company, fy_Start_Date, fy_End_Date, logIn.BU_ID) select data).ToList();
            if (stock.Count > 0)
            {
                //dgProductsList.DataSource = d;
                textBox1.Text = stock[0].OBValue.ToString();
                textBox2.Text = stock[0].ReceiptValue.ToString();
                textBox3.Text = stock[0].IssueValue.ToString();
                textBox5.Text = stock[0].OBValue.ToString();

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbAccName_Leave(object sender, EventArgs e)
        {
            try
            {


                if (cmbAccName.Text != "")

                {
                    //int ss= CmbBuyerName.ValueMember;
                    int s = cmbAccName.SelectedIndex;
                    var State = (from c in db.StockValueUpdations
                                     //where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                                 where c.Balance_Sheet_Account == Convert.ToInt32(cmbAccName.SelectedValue) && c.FYear == cmbFYear.Text
                                 select new { c.Opening_Value, c.Closing_Value }).ToList();
                    if (State.Count > 0)
                    {
                        textBox1.Text = State[0].Opening_Value.ToString();
                        textBox5.Text = State[0].Closing_Value.ToString();

                    }    
                    else
                    {
                        string t1 = fy_Start_Date.ToString("dd/MM/yyyy");

                        var ob = (from c in db.Account_Opening_Balances
                                         //where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                                     where c.Acc_ID == Convert.ToInt32(cmbAccName.SelectedValue) && c.OB_date <= Convert.ToDateTime(t1)
                                  select new { c.Debit_Amount, c.Credit_Amount }).ToList();
                        if (ob.Count > 0)
                        {
                            textBox1.Text = (ob[0].Debit_Amount - ob[0].Credit_Amount).ToString();
                            textBox5.Text = "0.00";
                        }
                        else
                        {
                            textBox1.Text = "0.00";
                            textBox5.Text = "0.00";
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbAccName.Text == string.Empty)
                {
                    MessageBox.Show("Account Name should Not be Empty", "Account Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbAccName.Focus();
                    return;
                }
                else if (cmbFYear.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Finacial Year", "Group Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbFYear.Focus();
                    return;
                }

                else
                {
                    Save();

                      
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void Save()
        {
            try
            {

                //Get Financial Year and Dates
                var bindComp = (from m in db.Financial_Year_Masters
                                where m.F_Year == cmbFYear.Text && m.Company_ID == logIn.company
                                select new
                                {
                                    m.Start_Date,
                                    m.End_Date,
                                    m.F_Year,
                                }).ToList();

                DateTime fy_Start_Date = Convert.ToDateTime(bindComp[0].Start_Date);
                DateTime fy_End_Date = Convert.ToDateTime(bindComp[0].End_Date);

                //if ((from u in db.Product_Groups where u.ID == Convert.ToInt32(txtGroupID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                if (cmbAccName.Text != "")
                {

                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.StockValueUpdations.Where(w => w.FYear == cmbFYear.Text && w.Balance_Sheet_Account == Convert.ToInt32(cmbAccName.SelectedValue)).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Opening_Value = (textBox1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox1.Text);
                        c.Closing_Value = (textBox5.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox5.Text);
                       
                       // c.Created_By = logIn.username;
                        c.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        db.SubmitChanges();
                        MessageBox.Show("Record Upadated Successfully");
                       
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
                    //}
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                    StockValueUpdation ci = new StockValueUpdation();
                    ci.FYear = cmbFYear.Text;
                    ci.FY_StartDate = fy_Start_Date;
                    ci.Balance_Sheet_Account = Convert.ToInt32(cmbAccName.SelectedValue);
                    ci.Opening_Value = (textBox1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox1.Text);
                    ci.Closing_Value = (textBox5.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox5.Text);
                    ci.Company_ID = logIn.company;
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    db.StockValueUpdations.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                                      //autogen();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Save City");
                    //}

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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFYear_Leave(object sender, EventArgs e)
        {
            if (cmbFYear.Text != "")
            {
                //Get Financial Year and Dates
                var bindComp = (from m in db.Financial_Year_Masters
                                where m.F_Year == cmbFYear.Text && m.Company_ID == logIn.company
                                select new
                                {
                                    m.Start_Date,
                                    m.End_Date,
                                    m.F_Year,
                                }).ToList();

                fy_Start_Date = Convert.ToDateTime(bindComp[0].Start_Date);
                DateTime fy_End_Date = Convert.ToDateTime(bindComp[0].End_Date);
            }
        }
    }
}
