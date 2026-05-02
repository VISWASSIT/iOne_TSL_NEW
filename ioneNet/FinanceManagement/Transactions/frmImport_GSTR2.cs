using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
namespace ioneNet.FinanceManagement.Transactions
{
    public partial class frmImport_GSTR2 : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        DataClasses1DataContext db = new DataClasses1DataContext();
        public frmImport_GSTR2()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                fdlg.FileName = txtChooseFile.Text;
                fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    txtChooseFile.Text = fdlg.FileName;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
            }

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtExcellSheet.Text == "")
                {
                    MessageBox.Show("Please Enter the Excell Sheet Name,It should not Empty");
                    txtExcellSheet.Focus();
                }
                else if (txtChooseFile.Text == "")
                {
                    MessageBox.Show("Please select the Choosefile,it should not Empty");
                    txtChooseFile.Focus();
                }
                else if (txtChooseFile.Text != "" && txtExcellSheet.Text != "")
                {
                    Cursor.Current = Cursors.WaitCursor;
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.DataTable DtSet;
                    System.Data.OleDb.OleDbDataAdapter MyCommand;

                    string filename = txtChooseFile.Text;
                    // string ExcellSheet = ;

                    string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                    MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtChooseFile.Text + ";Extended Properties='Excel 8.0;HDR=Yes'");

                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + txtExcellSheet.Text + "$] ", MyConnection);
                    //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
                    MyCommand.TableMappings.Add("Table", filename);
                    DtSet = new System.Data.DataTable();
                    MyCommand.Fill(DtSet);
                    int count = DtSet.Rows.Count;
                    DataTable dt = new DataTable();
                    DataRow dr = null;
                    dt.Columns.Add(new DataColumn("GSTIN", typeof(string)));
                    dt.Columns.Add(new DataColumn("AccName", typeof(string)));
                    dt.Columns.Add(new DataColumn("Inv_No", typeof(string)));
                    dt.Columns.Add(new DataColumn("Inv_Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Inv_Date", typeof(string)));
                    dt.Columns.Add(new DataColumn("Inv_Value", typeof(string)));
                    dt.Columns.Add(new DataColumn("RCM", typeof(string)));
                    dt.Columns.Add(new DataColumn("Tax_Rate", typeof(string)));
                    dt.Columns.Add(new DataColumn("Taxable_Value", typeof(string)));
                    dt.Columns.Add(new DataColumn("IGST", typeof(string)));
                    dt.Columns.Add(new DataColumn("CGST", typeof(string)));
                    dt.Columns.Add(new DataColumn("SGST", typeof(string)));
                    dt.Columns.Add(new DataColumn("Cess", typeof(string)));
                    dt.Columns.Add(new DataColumn("Return_Status", typeof(string)));
                    dt.Columns.Add(new DataColumn("Our_Entry_Ref_No", typeof(string)));
                    
                    for (int i = 0; i < count; i++)
                    {
                        if (DtSet.Rows[i]["GSTIN of supplier"].ToString() != "")
                        {
                            dr = dt.NewRow();
                            dr["GSTIN"] = DtSet.Rows[i]["GSTIN of supplier"].ToString();
                            dr["AccName"] = DtSet.Rows[i]["Trade/Legal name of the Supplier"].ToString();
                            dr["Inv_No"] = DtSet.Rows[i]["Invoice number"].ToString(); ;
                            dr["Inv_Type"] = DtSet.Rows[i]["Invoice type"].ToString();
                            dr["Inv_Date"] = DtSet.Rows[i]["Invoice Date"].ToString();
                            dr["Inv_Value"] = DtSet.Rows[i]["Invoice Value (₹)"].ToString();
                            dr["RCM"] = DtSet.Rows[i]["Supply Attract Reverse Charge"].ToString(); ;
                            dr["Tax_Rate"] = DtSet.Rows[i]["Rate (%)"].ToString();
                            dr["Taxable_Value"] = DtSet.Rows[i]["Taxable Value (₹)"].ToString();
                            dr["IGST"] = DtSet.Rows[i]["Integrated Tax  (₹)"].ToString();
                            dr["CGST"] = DtSet.Rows[i]["Central Tax (₹)"].ToString();
                            dr["SGST"] = DtSet.Rows[i]["State/UT tax (₹)"].ToString(); ;
                            dr["Cess"] = DtSet.Rows[i]["Cess  (₹)"].ToString();
                            dr["Return_Status"] = DtSet.Rows[i]["Counter Party Return status"].ToString(); ;
                            var da = (from obj in db.GoodsReceiptNote_Masters
                                      where obj.Supp_GST_No == DtSet.Rows[i]["GSTIN of supplier"].ToString() && obj.Company_ID == logIn.company && obj.Supplier_InvNo == DtSet.Rows[i]["Invoice number"].ToString()
                                      select obj).ToList();
                            if (da.Count > 0)
                            {
                                dr["Our_Entry_Ref_No"] = da[0].Grn_NO;
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    dgProductData.DataSource = dt;
                    //method();
                    MyConnection.Close();
                    Cursor.Current = Cursors.Default;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
