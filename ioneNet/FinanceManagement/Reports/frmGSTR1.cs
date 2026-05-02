using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.Data;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;

namespace ioneNet.FinanceManagement.Reports
{
    public partial class frmGSTR1 : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmGSTR1()
        {
            InitializeComponent();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }

        private void btnJSon_Click(object sender, EventArgs e)
        {
            try
            {
                string dt1 = dpFromDate.Text;
                string dt2 = dpTodate.Text;
                DateTime t1 = dpFromDate.Value;
                DateTime t2 = dpTodate.Value;
                string month = "";
                int year = 0;
                RootObject obj = new RootObject();
                var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                if (d1.Count > 0)
                {
                    obj.gstin = d1[0].GST_No;
                    month = t1.ToString("MM");
                    year = t1.Year;
                    obj.fp = string.Concat(month, year);   //"032019";
                    obj.gt = 0.00;
                    obj.cur_gt = 0.00;
                }

                
                var buyerGST = (from m in db.Invoice_Masters 
                                join C in db.Supplier_informations on m.BuyerName equals C.ID 
                                where m.Company_ID == logIn.company && m.InvDate >= t1 && m.InvDate<=t2  && C.Supplier_Type == "Registered Dealer"
                                orderby m.Cust_GST_No select new { m.Cust_GST_No }).Distinct().ToList();
                if (buyerGST.Count > 0)
                {
                    foreach (var GSTNo in buyerGST)
                    {
                        B2b b2b1 = new B2b();
                        b2b1.ctin = GSTNo.Cust_GST_No;
                        // Fill B2B               
                        SqlCommand cmd2 = new SqlCommand("GSTR1Data", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@compname", logIn.company);
                        cmd2.Parameters.AddWithValue("@Fromdate", t1);
                        cmd2.Parameters.AddWithValue("@Todate", t2);
                        cmd2.Parameters.AddWithValue("@gstno", GSTNo.Cust_GST_No);
                        cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        //DataSet ds2 = new DataSet();
                        DataTable ds2 = new DataTable();
                        // da2.Fill(ds2, "x");
                        da2.Fill(ds2);
                        foreach (System.Data.DataRow dr in ds2.Rows)
                        {
                            b2b1.inv.Add(new Inv()
                            {

                                inum = dr["Inv_No"].ToString(),
                                idt = dr["InvDate"].ToString(),
                                val = Convert.ToDouble(dr["Tot_Inv_Value"].ToString()),
                                pos = dr["StateCode"].ToString(),
                                rchrg = "N",
                                itms = new List<Itm>(),
                                inv_typ = dr["InvType"].ToString()

                            });

                            var Buyerblind = (from m in db.Invoice_Child_sums where m.Company_ID == logIn.company && m.Inv_No == dr["Inv_No"].ToString() select new { m.Taxable_Value, m.CGST_Amnt, m.SGST_Amnt, m.IGST_Amnt,m.GST_Rate }).Distinct().ToList();
                            if (Buyerblind.Count > 0)
                            {
                                foreach (var buyerItem in Buyerblind)
                                {
                                    Itm itm = new Itm();
                                    itm.num = 1; // WIll be filled later
                                    itm.itm_det = new ItmDet();
                                   

                                    //buterItem.Taxable_Value
                                    itm.itm_det.txval = Convert.ToDouble(buyerItem.Taxable_Value);  // here i need to get data fro
                                    if (dr["InvType"].ToString() == "R")
                                    {
                                        itm.itm_det.rt = Convert.ToInt32(buyerItem.GST_Rate);
                                        if (Convert.ToDouble(buyerItem.IGST_Amnt) > 0)
                                        {
                                            itm.itm_det.iamt = Convert.ToDouble(buyerItem.IGST_Amnt);
                                            itm.itm_det.camt = null;
                                            itm.itm_det.samt = null;
                                        }
                                        else
                                        {
                                            itm.itm_det.camt = Convert.ToDouble(buyerItem.CGST_Amnt);
                                            itm.itm_det.samt = Convert.ToDouble(buyerItem.SGST_Amnt);
                                            itm.itm_det.iamt = null;
                                        }
                                    }
                                    else
                                    {
                                        itm.itm_det.rt = 0;
                                        itm.itm_det.iamt = Convert.ToDouble(buyerItem.IGST_Amnt);
                                        itm.itm_det.camt = null;
                                        itm.itm_det.samt = null;
                                    }
                                    

                                    itm.itm_det.csamt = 0;

                                    b2b1.inv.Last().itms.Add(itm); // Adding item itself
                                }
                            }
                            
                        }
                        obj.b2b.Add(b2b1);
                      //  B2b b2b1 = new B2b();
                    }
                }
                // Fill B2Cs Invoice           

                SqlCommand cmd3 = new SqlCommand("GSTR1Data_B2CS", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@compname", logIn.company);
                cmd3.Parameters.AddWithValue("@Fromdate", t1);
                cmd3.Parameters.AddWithValue("@Todate", t2);
                cmd3.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                //DataSet ds2 = new DataSet();
                DataTable ds3 = new DataTable();
                // da2.Fill(ds2, "x");
                da3.Fill(ds3);

                foreach (System.Data.DataRow dr in ds3.Rows)
                {
                    B2cs b2cs1 = new B2cs();
                    b2cs1.rt = 18;
                    b2cs1.sply_ty = dr["Place_of_Supply"].ToString();
                    b2cs1.pos = dr["StateCode"].ToString();
                    b2cs1.typ = "OE";
                    b2cs1.txval = Convert.ToDouble(dr["Taxable_Value"].ToString());
                    b2cs1.camt = Convert.ToDouble(dr["CGST_Amnt"].ToString());
                    b2cs1.samt = Convert.ToDouble(dr["SGST_Amnt"].ToString());
                    b2cs1.csamt = 0.00;
                    obj.b2cs.Add(b2cs1);
                }


                // Fill Export Invoices               
                SqlCommand cmd4 = new SqlCommand("GSTR1Data_EXPORT", con);
                cmd4.CommandType = CommandType.StoredProcedure;
                cmd4.Parameters.AddWithValue("@compname", logIn.company);
                cmd4.Parameters.AddWithValue("@Fromdate", t1);
                cmd4.Parameters.AddWithValue("@Todate", t2);
                cmd4.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                //DataSet ds2 = new DataSet();
                DataTable ds4 = new DataTable();
                // da2.Fill(ds2, "x");
                da4.Fill(ds4);

                foreach (System.Data.DataRow dr in ds4.Rows)
                {
                    Exp exp1 = new Exp();
                    exp1.exp_typ = "WOPAY";
                    exp1.inv.Add(new Inv2()
                    {

                        inum = dr["Inv_No"].ToString(),
                        idt = dr["InvDate"].ToString(),
                        val = Convert.ToDouble(dr["Tot_Inv_Value"].ToString()),
                        itms = new List<Itm2>()
                    });

                    var Buyerblind = (from m in db.Invoice_Child_sums where m.Company_ID == logIn.company && m.Inv_No == dr["Inv_No"].ToString() select new {  m.Taxable_Value, m.CGST_Amnt, m.SGST_Amnt, m.IGST_Amnt }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        foreach (var buyerItem in Buyerblind)
                        {
                            Itm2 itm = new Itm2();


                            //buterItem.Taxable_Value
                            itm.txval = Convert.ToDouble(buyerItem.Taxable_Value);  // here i need to get data fro
                            itm.rt = 0;
                            itm.iamt = Convert.ToDouble(buyerItem.IGST_Amnt);
                            itm.csamt = 0;


                            exp1.inv.Last().itms.Add(itm); // Adding item itself
                        }
                    }
                    obj.exp.Add(exp1);
                }
                ////Credit / Debit Notes

                //var buyerGST1 = (from m in db.SaleReturns_Masters where m.Company_ID == logIn.company && m.Vch_Date >= t1 && m.Vch_Date <= t2 && m.Cust_GST_No != "NA" orderby m.Cust_GST_No select new { m.Cust_GST_No }).Distinct().ToList();
                //if (buyerGST1.Count > 0)
                //{
                //    foreach (var GSTNo in buyerGST1)
                //    {
                //        Cdnr cdnr = new Cdnr();
                //        cdnr.ctin = GSTNo.Cust_GST_No;
                //        // Fill B2B               
                //        SqlCommand cmd2 = new SqlCommand("GSTR1Data_CDNR", con);
                //        cmd2.CommandType = CommandType.StoredProcedure;
                //        cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //        cmd2.Parameters.AddWithValue("@Fromdate", t1);
                //        cmd2.Parameters.AddWithValue("@Todate", t2);
                //        cmd2.Parameters.AddWithValue("@gstno", GSTNo.Cust_GST_No);
                //        cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //        //DataSet ds2 = new DataSet();
                //        DataTable ds2 = new DataTable();
                //        // da2.Fill(ds2, "x");
                //        da2.Fill(ds2);
                //        foreach (System.Data.DataRow dr in ds2.Rows)
                //        {
                //            cdnr.nt.Add(new Nt()
                //            {

                //                ntty = "C",
                //                nt_num = dr["Vch_No"].ToString(),
                //                nt_dt = dr["Vch_Date"].ToString(),

                //                p_gst = "N",
                //                inum = dr["Inv_No"].ToString(),
                //                idt = dr["InvDate"].ToString(),
                //                val = Convert.ToDouble(dr["Tot_Inv_Value"].ToString()),
                //                itms = new List<Itm3>()


                //            });

                //            var Buyerblind = (from m in db.SaleReturnChild_Sums where m.Company_ID == logIn.company && m.Vch_No == dr["Vch_No"].ToString() select new { m.Taxable_Value, m.csAMT, m.IGST_Amnt, m.GST_Rate }).Distinct().ToList();
                //            if (Buyerblind.Count > 0)
                //            {
                //                foreach (var buyerItem in Buyerblind)
                //                {
                //                    Itm3 itm = new Itm3();
                //                    itm.num = 1; // WIll be filled later
                //                    itm.itm_det = new ItmDet2();


                //                    //buterItem.Taxable_Value


                //                    itm.itm_det.rt = Convert.ToInt32(buyerItem.GST_Rate);
                //                    itm.itm_det.txval = Convert.ToDouble(buyerItem.Taxable_Value);  // here i need to get data fro

                //                    itm.itm_det.iamt = Convert.ToDouble(buyerItem.IGST_Amnt);
                //                    itm.itm_det.csamt = Convert.ToDouble(buyerItem.csAMT);

                //                    cdnr.nt.Last().itms.Add(itm); // Adding item itself
                //                }
                //            }

                //        }
                //        obj.cdnr.Add(cdnr);
                //    }
                //}



                ////obj.cdnr = exp1;
                // Fill HSN CODES               
                SqlCommand cmd5 = new SqlCommand("GSTR1Data_HSN", con);
                cmd5.CommandType = CommandType.StoredProcedure;
                cmd5.Parameters.AddWithValue("@compname", logIn.company);
                cmd5.Parameters.AddWithValue("@Fromdate", t1);
                cmd5.Parameters.AddWithValue("@Todate", t2);
                cmd5.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
                //DataSet ds2 = new DataSet();
                DataTable ds5 = new DataTable();
                // da2.Fill(ds2, "x");
                da5.Fill(ds5);
                int i = 1;
                Hsn hsn1 = new Hsn();
                foreach (System.Data.DataRow dr in ds5.Rows)
                {
                    hsn1.data.Add(new Datum()
                    {

                        num = i,
                        hsn_sc = dr["HSNCode"].ToString(),
                        desc = dr["pdesc"].ToString(),
                        uqc = dr["Uom"].ToString(),
                        qty = Convert.ToDouble(dr["qty"].ToString()),
                        val = Convert.ToDouble(dr["Total_Value"].ToString()),
                        txval = Convert.ToDouble(dr["Taxable_Value"].ToString()),
                        iamt = Convert.ToDouble(dr["IGST_Amnt"].ToString()),
                        camt = Convert.ToDouble(dr["CGST_Amnt"].ToString()),
                        samt = Convert.ToDouble(dr["SGST_Amnt"].ToString()),
                        csamt = 0.00
                    });
                    i = i + 1;
                    obj.hsn = hsn1;

                }

                // Fill Doc Nos              
                SqlCommand cmd6 = new SqlCommand("GSTR1Data_Docs", con);
                cmd6.CommandType = CommandType.StoredProcedure;
                cmd6.Parameters.AddWithValue("@compname", logIn.company);
                cmd6.Parameters.AddWithValue("@Fromdate", t1);
                cmd6.Parameters.AddWithValue("@Todate", t2);
                cmd6.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da6 = new SqlDataAdapter(cmd6);
                //DataSet ds2 = new DataSet();
                DataTable ds6 = new DataTable();
                // da2.Fill(ds2, "x");
                da6.Fill(ds6);
                DocIssue di1 = new DocIssue();
                foreach (System.Data.DataRow dr in ds6.Rows)
                {

                    di1.doc_det.Add(new DocDet()
                    {
                        doc_num = 1,
                        doc_typ = dr["DocType"].ToString(),
                        docs = new List<Doc>()
                    });
                    Doc doc = new Doc();
                    doc.num = 1;
                    doc.from = dr["Min_Inv_no"].ToString();
                    doc.to = dr["Max_inv_no"].ToString();
                    doc.totnum = Convert.ToInt32(dr["No_of_inv"].ToString());
                    doc.cancel = 0;
                    doc.net_issue = Convert.ToInt32(dr["No_of_inv"].ToString());
                    di1.doc_det.Last().docs.Add(doc);
                }
                obj.doc_issue = di1;

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                
                string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });    
                
                //serializer.MaxJsonLength = 1024 * 1024 * 100;// Needed to increases if JSOn is longer than MaxJsonLength
                //string json = serializer.Serialize(obj);
                
                Console.WriteLine(json);
                Console.ReadLine();
                string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                System.IO.File.WriteAllText(Fname + "\\GSTR1_" + string.Concat(month, year) + ".json", json);
                MessageBox.Show("Json File Created and Saved in My Documents Folder");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        public class ItmDet
        {
            public double txval { get; set; }
            public int rt { get; set; }
            public double? camt { get; set; }
            public double? samt { get; set; }           
            public double? iamt { get; set; }
            public double csamt { get; set; }

        }

        public class Itm
        {
            private int m_num;
            private string m_itm_det;

            public int num
            {
                get
                {
                    return m_num;
                }
                set
                {
                    m_num = value;
                    ;
                }
            }
            public ItmDet itm_det { get; set; }

            public Itm()
            {

            }
        }

        public class Inv
        {
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public string pos { get; set; }
            public string rchrg { get; set; }
            public List<Itm> itms { get; set; }
            public string inv_typ { get; set; }
            public Inv()
            {
                itms = new List<Itm>();
            }

        }

        public class B2b
        {
            public string ctin { get; set; }
            public List<Inv> inv { get; set; }

            public B2b()
            {
                inv = new List<Inv>();
            }
        }
        public class B2cs
        {
            public int rt { get; set; }
            public string sply_ty { get; set; }
            public string pos { get; set; }
            public string typ { get; set; }
            public double txval { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class Itm2
        {
            public double txval { get; set; }
            public int rt { get; set; }
            public double iamt { get; set; }
            public double csamt { get; set; }
        }

        public class Inv2
        {
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public List<Itm2> itms { get; set; }

            public Inv2()
            {
                itms = new List<Itm2>();
            }
        }

        public class Exp
        {
            public string exp_typ { get; set; }
            public List<Inv2> inv { get; set; }

            public Exp()
            {
                inv = new List<Inv2>();
            }
        }

        public class ItmDet2
        {
            public int rt { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
            public double csamt { get; set; }
        }

        public class Itm3
        {
            public int num { get; set; }
            public ItmDet2 itm_det { get; set; }
        }

        public class Nt
        {
            public string ntty { get; set; }
            public string nt_num { get; set; }
            public string nt_dt { get; set; }
            public string p_gst { get; set; }
            public string inum { get; set; }
            public string idt { get; set; }
            public double val { get; set; }
            public List<Itm3> itms { get; set; }

            public Nt()
            {
                itms = new List<Itm3>();
            }
        }

        public class Cdnr
        {
            public string ctin { get; set; }
            public List<Nt> nt { get; set; }

            //public Cdnr()
            //{
            //    nt = new List<Nt>();
            //}
        }

        public class Datum
        {
            public int num { get; set; }
            public string hsn_sc { get; set; }
            public string desc { get; set; }
            public string uqc { get; set; }
            public double qty { get; set; }
            public double val { get; set; }
            public double txval { get; set; }
            public double iamt { get; set; }
            public double camt { get; set; }
            public double samt { get; set; }
            public double csamt { get; set; }
        }

        public class Hsn
        {
            public List<Datum> data { get; set; }

            public Hsn()
            {
                data = new List<Datum>();
            }
        }

        public class Doc
        {
            public int num { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public int totnum { get; set; }
            public int cancel { get; set; }
            public int net_issue { get; set; }
        }

        public class DocDet
        {
            public int doc_num { get; set; }
            public string doc_typ { get; set; }
            public List<Doc> docs { get; set; }
            public DocDet()
            {
                docs = new List<Doc>();
            }
        }

        public class DocIssue
        {
            public List<DocDet> doc_det { get; set; }

            public DocIssue()
            {
                doc_det = new List<DocDet>();
            }
        }

        public class RootObject
        {
            public string gstin { get; set; }
            public string fp { get; set; }
            public double gt { get; set; }
            public double cur_gt { get; set; }
            public List<B2b> b2b { get; set; }
            public List<B2cs> b2cs { get; set; }
            public List<Exp> exp { get; set; }
            public List<Cdnr> cdnr { get; set; }
            public Hsn hsn { get; set; }
            public DocIssue doc_issue { get; set; }

            public RootObject()
            {
                b2b = new List<B2b>();

                b2cs = new List<B2cs>();

                exp = new List<Exp>();

                cdnr = new List<Cdnr>();

                hsn = new Hsn();

                doc_issue = new DocIssue();
            }

            //    this.gstin = "36AAAFY5479P2Z8"; this.fp = "042019"; this.gt = 0.00; this.cur_gt = 0.00;                   



            //}


            //string data = JsonConvert.DeserializeObject<RootObject>(jsonString);


        }
        // }

        private void frmGSTR1_Load(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            bindb2bdata();

            bindb2CSdata();
            bindCDNRdata();
            bindExportdata();

            bindHSNdata();
            bindDocsdata();
        }

        public void bindb2bdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();

                var d = (from data in db.GSTR1_B2B_Excel(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2),logIn.BU_ID) select data).ToList();

                //SqlCommand cmd2 = new SqlCommand("GSTR1_B2B_Excel", con);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                ////DataSet ds2 = new DataSet();
                //DataTable ds2 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                
                B2B_Data.DataSource = null;
                if (d.Count > 0)
                {
                    B2B_Data.DataSource = d;
                
                this.B2B_Data.TableSummaryRows.Clear();
                this.B2B_Data.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.B2B_Data.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Taxable_Value";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "Taxable_Value";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "Total_CGST_Amnt";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "CGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                GsummaryColumn3.Name = "Total_SGST_Amnt";
                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn3.Format = "{Sum}";
                GsummaryColumn3.MappingName = "SGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                GsummaryColumn4.Name = "Total_IGST_Amnt";
                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn4.Format = "{Sum}";
                GsummaryColumn4.MappingName = "IGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                // Adds the summary row in the GroupSummaryRows collection.
                this.B2B_Data.GroupSummaryRows.Add(groupSummaryRow1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void bindb2CSdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();
                var d = (from data in db.GSTR1_B2C_Excel(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2), logIn.BU_ID) select data).ToList();



                //SqlCommand cmd2 = new SqlCommand("GSTR1_B2C_Excel", con);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                ////DataSet ds2 = new DataSet();
                //DataTable ds2 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                B2CL_Data.DataSource = d;// ds2;

                this.B2CL_Data.TableSummaryRows.Clear();
                this.B2CL_Data.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.B2CL_Data.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Taxable_Value";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "Taxable_Value";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "Total_CGST_Amnt";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "CGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                GsummaryColumn3.Name = "Total_SGST_Amnt";
                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn3.Format = "{Sum}";
                GsummaryColumn3.MappingName = "SGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                GsummaryColumn4.Name = "Total_IGST_Amnt";
                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn4.Format = "{Sum}";
                GsummaryColumn4.MappingName = "IGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                // Adds the summary row in the GroupSummaryRows collection.
                this.B2CL_Data.GroupSummaryRows.Add(groupSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // }
        }

        public void bindCDNRdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();

                var d = (from data in db.GSTR1Data_CDNR_Excel(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2), logIn.BU_ID) select data).ToList();

                //SqlCommand cmd2 = new SqlCommand("GSTR1_B2B_Excel", con);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                ////DataSet ds2 = new DataSet();
                //DataTable ds2 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                CDNR_Data.DataSource = d;

                this.CDNR_Data.TableSummaryRows.Clear();
                this.CDNR_Data.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Tot_Inv_Value";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Tot_Inv_Value";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                this.CDNR_Data.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Taxable_Value";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "Taxable_Value";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "Tot_Inv_Value";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "Tot_Inv_Value";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);
                

                // Adds the summary row in the GroupSummaryRows collection.
                this.CDNR_Data.GroupSummaryRows.Add(groupSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public void bindExportdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();

                var d = (from data in db.GSTR1Data_EXPORT(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2), logIn.BU_ID) select data).ToList();

                //SqlCommand cmd2 = new SqlCommand("GSTR1Data_EXPORT", con);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                ////DataSet ds2 = new DataSet();
                //DataTable ds2 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                Export_Data.DataSource = d;

                this.Export_Data.TableSummaryRows.Clear();
                this.Export_Data.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";
                

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_Invoice_Value";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "Tot_Inv_Value";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.Export_Data.TableSummaryRows.Add(tableSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // }
        }

        public void bindHSNdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");
                var d = (from data in db.GSTR1Data_HSN(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2), logIn.BU_ID) select data).ToList();

                // var data = db.GSTR1Data_HSN(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                //SqlCommand cmd2 = new SqlCommand("GSTR1Data_HSN", con);
                //cmd2.CommandType = CommandType.StoredProcedure;
                //cmd2.Parameters.AddWithValue("@compname", logIn.company);
                //cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                ////DataSet ds2 = new DataSet();
                //DataTable ds2 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da2.Fill(ds2);
                HSNCode_Data.DataSource = null;
                if (d.Count > 0)
                {
                    HSNCode_Data.DataSource = d;

                    this.HSNCode_Data.TableSummaryRows.Clear();
                    this.HSNCode_Data.GroupSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total_Taxable_Value";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "{Sum}";
                    summaryColumn1.MappingName = "Taxable_Value";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "Total_Invoice_Value";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "{Sum}";
                    summaryColumn3.MappingName = "Total_Value";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Total_CGST_Amnt";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "{Sum}";
                    summaryColumn2.MappingName = "CGST_Amnt";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);


                    GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                    summaryColumn5.Name = "Total_SGST_Amnt";
                    summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn5.Format = "{Sum}";
                    summaryColumn5.MappingName = "SGST_Amnt";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn5);


                    GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                    summaryColumn4.Name = "Total_IGST_Amnt";
                    summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn4.Format = "{Sum}";
                    summaryColumn4.MappingName = "IGST_Amnt";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                    this.HSNCode_Data.TableSummaryRows.Add(tableSummaryRow1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // }
        }
        public void bindDocsdata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");
                var d = (from data in db.GSTR1Data_Docs(logIn.company, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2), logIn.BU_ID) select data).ToList();

        
                //SqlCommand cmd6 = new SqlCommand("GSTR1Data_Docs", con);
                //cmd6.CommandType = CommandType.StoredProcedure;
                //cmd6.Parameters.AddWithValue("@compname", logIn.company);
                //cmd6.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                //cmd6.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //SqlDataAdapter da6 = new SqlDataAdapter(cmd6);
                ////DataSet ds2 = new DataSet();
                //DataTable ds6 = new DataTable();
                //// da2.Fill(ds2, "x");
                //da6.Fill(ds6);
                //DocIssue di1 = new DocIssue();
                //// da2.Fill(ds2, "x");
                
                Doc_Data.DataSource = d;

                //this.sfgHSNCode.TableSummaryRows.Clear();
                //this.sfgHSNCode.GroupSummaryRows.Clear();
                //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                //tableSummaryRow1.Name = "TableSummary";
                //tableSummaryRow1.ShowSummaryInRow = false;
                //tableSummaryRow1.Position = VerticalPosition.Bottom;

                //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                //summaryColumn1.Name = "Total_Taxable_Value";
                //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn1.Format = "{Sum}";
                //summaryColumn1.MappingName = "Taxable_Value";


                //GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                //summaryColumn3.Name = "Total_Invoice_Value";
                //summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn3.Format = "{Sum}";
                //summaryColumn3.MappingName = "Total_Value";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                //GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                //summaryColumn2.Name = "Total_CGST_Amnt";
                //summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn2.Format = "{Sum}";
                //summaryColumn2.MappingName = "CGST_Amnt";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn2);


                //GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                //summaryColumn5.Name = "Total_SGST_Amnt";
                //summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn5.Format = "{Sum}";
                //summaryColumn5.MappingName = "SGST_Amnt";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn5);


                //GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                //summaryColumn4.Name = "Total_IGST_Amnt";
                //summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn4.Format = "{Sum}";
                //summaryColumn4.MappingName = "IGST_Amnt";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                //this.sfgHSNCode.TableSummaryRows.Add(tableSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {

            SfDataGrid[] dgv = new SfDataGrid[] { B2B_Data, B2CL_Data, B2CS_Data,CDNR_Data,CDNUR_Data, Export_Data,HSNCode_Data,Doc_Data };
            DataGridviewImportToExcel(dgv, "GSTR1");
            
        }

        private bool DataGridviewImportToExcel(SfDataGrid[] dgv, string fileName)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel file|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0)
                return false;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("can not create Excel");
                return false;
            }
            Excel.Workbooks workbooks = xlApp.Workbooks;
            Excel.Workbook workbook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets[1];
            worksheet.Name = dgv[0].Name;
            for (int index = 0; index < dgv.Length; index++)
            {
                for (int i = 0; i < dgv[index].ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dgv[index].Columns[i].HeaderText;
                }

                for (int r = 1; r < dgv[index].RowCount-1; r++)
                {
                    for (int i = 0; i < dgv[index].ColumnCount; i++)
                    {
                                                
                      //  var currentCellValue = dgv[index].CurrentCell.CellRenderer.GetControlValue();
                        var rowData = dgv[index].GetRecordAtRowIndex(r);
                        var mappingName = dgv[index].Columns[i].MappingName;                        
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


                        worksheet.Cells[r + 2, i + 1] = cellVaue;  //dgv[index].Rows[r].Cells[i].Value;
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();
                if (index < dgv.Length - 1)
                    worksheet.Name = dgv[index].Name;
                    worksheet = (Excel.Worksheet)workbook.Worksheets.Add();
                    //if (index > 0)
                    //{
                        
                    //}
            }

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error,file maybe is opening！\n" + ex.Message);
                    return false;
                }
            }
            xlApp.Quit();
            GC.Collect();
            MessageBox.Show("File： " + fileName + ".xls save Successfully", "tip ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }



    }
}
