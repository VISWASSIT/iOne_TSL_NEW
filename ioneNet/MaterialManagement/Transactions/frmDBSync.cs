using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmDBSync : Form
    {
        //string conString = "Data Source=SG2NWPLS14SQL-v09.shr.prod.sin2.secureserver.net;Initial Catalog=ethio_iOne;User ID=ethioione;Password=ethio@123456;providerName=System.Data.SqlClient";
  
        SqlConnection conOnline = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneOnline"].ConnectionString);
        
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        
        public frmDBSync()
        {
            InitializeComponent(); 
        }

        private void frmDBSync_Load(object sender, EventArgs e)
        {
            //SqlConnection conOnline = new SqlConnection();
            //conOnline = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString); 
            //"Data Source=SG2NWPLS14SQL-v09.shr.prod.sin2.secureserver.net;Initial Catalog=ethio_iOne;User ID=ethioione;Password=ethio@123456;providerName=System.Data.SqlClient";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            var grnM = (from s in db.GoodsReceiptNote_Masters
                        where s.Grn_Date >= dtpFrmDate.Value && s.Grn_Date <= dtpToDate.Value && s.BU_ID == logIn.BU_ID
                        orderby s.Grn_Date
                        select new { s.Grn_NO }).ToList();
                    if (grnM.Count > 0)
                    {



                for (int i = 0; i < grnM.Count; i++)
                {
                    string GRN = grnM[i].Grn_NO;
                    var grnC = (from s in db.GoodsReceiptNote_Masters                                
                                where s.Grn_NO == GRN && s.BU_ID == logIn.BU_ID
                                select s).FirstOrDefault();
                    // MessageBox.Show(d[i].Grn_NO);

                    if (conOnline.State != ConnectionState.Open)
                        conOnline.Open();
                    //con.Open();

                    SqlCommand cmd1 = new SqlCommand("delete  from [GoodsReceiptNote_Child] where [Grn_NO] =@ProdID", conOnline);
                    cmd1.Parameters.AddWithValue("@ProdID", GRN);

                    cmd1.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("delete  from [GoodsReceiptNote_Master] where [Grn_NO] =@ProdID", conOnline);
                    cmd2.Parameters.AddWithValue("@ProdID", GRN);
                    cmd2.ExecuteNonQuery();
                    // conOnline.Close();

                    //Get Supplier ID
                    //Supplier Code

                    var supp = (from s in db.Supplier_informations
                                where s.ID == grnC.SupplierName && s.Company_ID == logIn.company
                                select s).FirstOrDefault();


                    int SuppCode = 0;
                    SqlDataReader myreader3;
                    SqlCommand cmdSupp = new SqlCommand();
                    cmdSupp.CommandText = "select id from Supplier_information  where [Supplier_Id] ='" + supp.Supplier_Id + "'";
                    cmdSupp.Connection = conOnline;
                    cmdSupp.ExecuteNonQuery();
                    myreader3 = cmdSupp.ExecuteReader();
                    Boolean reccnt = false;
                    while (myreader3.Read())
                    {
                        SuppCode = myreader3.GetInt32(0);
                        reccnt = true;
                    }
                    if (reccnt == false)
                    {
                        SqlCommand cmd7 = new SqlCommand();
                        SqlCommand cmd8 = new SqlCommand();
                        SqlDataReader myreader4;
                        cmd7.CommandText = "insert into [Supplier_information]([Supplier_Id],[Supplier_Category],[Supplier_Name],[Supplier_Alias_Name]" +
                            ",[Supplier_Type],[Account_Group],[Group_Ledger],[Address_1],[Address_2],[City],[State],[StateCode],[Country],[Pincode]" +
                            ",[Phone_No],[Email_Id]"+
                            ",[GSTIN_NO],[PAN_No],[BankName],[BranchName],[AccNo],[IFSCCode],[Account_Manager],[Credit_Limit],[Apply_Credit_Limit]" +
                            ",[Price_List],[Product_services],[Region_Name],[Customer_Plant_ID],[Status],[Company_ID],[Created_By],[Modified_By]) " +
                                "values('" + supp.Supplier_Id + "','" + supp.Supplier_Category + "','" + supp.Supplier_Name + "','" + supp.Supplier_Alias_Name + "'," +
                                "'" + supp.Supplier_Type + "','" + supp.Account_Group + "','" + supp.Group_Ledger + "','" + supp.Address_1 + "'," +
                                "'" + supp.Address_2 + "','" + supp.City + "','" + supp.State + "','" + supp.StateCode + "'," +
                                "'" + supp.Country + "', '" + supp.Pincode + "','" + supp.Phone_No + "'," + "','" + supp.Email_Id + "'," + "','" + supp.GSTIN_NO + "'," +
                                "'" + supp.PAN_No + "','" + supp.BankName + "','" + supp.BranchName + "','" + supp.AccNo + "'," +
                                "'" + supp.IFSCCode + "','" + supp.Account_Manager + "','" + supp.Credit_Limit + "' ,'" + supp.Apply_Credit_Limit + "'," +
                                "'" + supp.Price_List + "','" + supp.Product_services + "' ,'" + supp.Region_Name + "' ,'" + supp.Customer_Plant_ID + "'," +
                                "'" + supp.Status + "' ,'" + supp.Company_ID + "','" + supp.Created_By + "' ,'" + supp.Modified_By + "')";
                        // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                        cmd7.Connection = conOnline;
                        cmd7.ExecuteNonQuery();

                        cmd8.CommandText = "select id from  Supplier_information  where [Supplier_Id] ='" + supp.Supplier_Id + "' and Company_ID = '" + logIn.company + "'";
                        cmd8.Connection = conOnline;
                        cmd8.ExecuteNonQuery();
                        myreader4 = cmd8.ExecuteReader();

                        myreader4.Read();
                        SuppCode = Convert.ToInt32(myreader4[0].ToString());
                    }



                        SqlCommand cmd3 = new SqlCommand();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd3.CommandText = "insert into GoodsReceiptNote_Master ([Grn_NO],Grn_Date,[Purchase_Basis],[SupplierName],[DC_No]," +
                        "DC_Date,[Supplier_InvNo],Supplier_InvDate,[OriginalInvReceived],[TAX_Class],[Purchase_Account]" +
                        ",[TotalQty],[SubTotal],[Tot_Discount],[Freight],[Other_Charges],[Tot_TaxableValue],[Tot_CGST_Amnt],[Tot_SGST_Amnt]" +
                        ",[Tot_IGST_Amnt],[Total_Amount],[Tcs_Per],[Tcs_Amount],[Rounding],[Tot_Ord_Value] ,[TDS_Amount],[Net_GRN_Amount]" +
                        ",[Warehouse_Code],[Vehicle_No],[Transporter_Name],[LrNo_LrDate],[Other_Terms],[RCM],[IneligibleTax],[Status]" +
                        ",[isDeleted],[BU_ID],[Company_ID],[Created_By],[Modified_By],[GRN_Type],[PF_Charges],[Frieght_Paid],[Hamali_Charges]) " +
                                    "values(@Grn_NO,@Grn_Date, @Purchase_Basis,@SupplierName,@DC_No,@DC_Date,@Supplier_InvNo,@Supplier_InvDate" +
                        " ,@OriginalInvReceived,@TAX_Class,@Purchase_Account,@TotalQty,@SubTotal,@Tot_Discount,@Freight,@Other_Charges,@Tot_TaxableValue,@Tot_CGST_Amnt,@Tot_SGST_Amnt" +
                        ",@Tot_IGST_Amnt,@Total_Amount,@Tcs_Per,@Tcs_Amount,@Rounding,@Tot_Ord_Value,@TDS_Amount,@Net_GRN_Amount,@Warehouse_Code,@Vehicle_No" +
                        ",@Transporter_Name,@LrNo_LrDate,@Other_Terms,@RCM,@IneligibleTax,@Status,@isDeleted,@BU_ID,@Company_ID,@Created_By,@Modified_By,@GRN_Type,@PF_Charges,@Frieght_Paid,@Hamali_Charges)";
                    //'" + Convert.ToDateTime(grnC.Grn_Date) + "'
                    //'" + Convert.ToDateTime(grnC.DC_Date) + "',
                    //'" + Convert.ToDateTime(grnC.Supplier_InvDate) + "',


                    cmd3.Parameters.AddWithValue("@Grn_NO", grnC.Grn_NO);
                    cmd3.Parameters.AddWithValue("@Grn_Date", grnC.Grn_Date);                  
                    cmd3.Parameters.AddWithValue("@Purchase_Basis", grnC.Purchase_Basis);
                    cmd3.Parameters.AddWithValue("@SupplierName", SuppCode);
                    cmd3.Parameters.AddWithValue("@DC_No", grnC.DC_No);
                    cmd3.Parameters.AddWithValue("@DC_Date", grnC.Supplier_InvDate);
                    //  cmd.Parameters.AddWithValue("@ConsigneeName", Convert.ToInt32(1));
                    // cmd.Parameters.AddWithValue("@Multi_Loc_Delivery", logIn.BU_ID);
                    cmd3.Parameters.AddWithValue("@Supplier_InvNo", (grnC.Supplier_InvNo == "") ? "" : grnC.Supplier_InvNo);                    
                    cmd3.Parameters.AddWithValue("@Supplier_InvDate", grnC.Supplier_InvDate);
                    cmd3.Parameters.AddWithValue("@OriginalInvReceived", grnC.OriginalInvReceived);
                    cmd3.Parameters.AddWithValue("@TAX_Class", grnC.TAX_Class);
                    cmd3.Parameters.AddWithValue("@Purchase_Account", grnC.Purchase_Account);
                    cmd3.Parameters.AddWithValue("@TotalQty", Convert.ToDecimal(grnC.TotalQty));
                    cmd3.Parameters.AddWithValue("@SubTotal", Convert.ToDecimal(grnC.SubTotal));
                    cmd3.Parameters.AddWithValue("@Tot_Discount", Convert.ToDecimal(grnC.Tot_Discount));
                    cmd3.Parameters.AddWithValue("@Freight", Convert.ToDecimal(grnC.Freight));
                    cmd3.Parameters.AddWithValue("@Other_Charges", Convert.ToDecimal(grnC.Other_Charges));
                    cmd3.Parameters.AddWithValue("@Tot_TaxableValue", Convert.ToDecimal(grnC.Tot_TaxableValue));
                    cmd3.Parameters.AddWithValue("@Tot_CGST_Amnt", Convert.ToDecimal(grnC.Tot_CGST_Amnt));
                    cmd3.Parameters.AddWithValue("@Tot_SGST_Amnt", Convert.ToDecimal(grnC.Tot_SGST_Amnt));
                    cmd3.Parameters.AddWithValue("@Tot_IGST_Amnt", Convert.ToDecimal(grnC.Tot_IGST_Amnt));
                    cmd3.Parameters.AddWithValue("@Total_Amount", Convert.ToDecimal(grnC.Total_Amount));
                    cmd3.Parameters.AddWithValue("@Tcs_Per", Convert.ToDecimal(grnC.Tcs_Per));
                    cmd3.Parameters.AddWithValue("@Tcs_Amount", Convert.ToDecimal(grnC.Tcs_Amount));
                    cmd3.Parameters.AddWithValue("@Rounding", Convert.ToDecimal(grnC.Rounding));
                    cmd3.Parameters.AddWithValue("@Tot_Ord_Value", Convert.ToDecimal(grnC.Tot_Ord_Value));
                    cmd3.Parameters.AddWithValue("@TDS_Amount", Convert.ToDecimal(grnC.TDS_Amount));
                    cmd3.Parameters.AddWithValue("@Net_GRN_Amount", Convert.ToDecimal(grnC.Net_GRN_Amount));
                    cmd3.Parameters.AddWithValue("@Warehouse_Code", grnC.Warehouse_Code);
                    cmd3.Parameters.AddWithValue("@Vehicle_No", grnC.Vehicle_No);                    
                    cmd3.Parameters.AddWithValue("@Transporter_Name", grnC.Transporter_Name);
                    cmd3.Parameters.AddWithValue("@LrNo_LrDate", grnC.LrNo_LrDate);
                    cmd3.Parameters.AddWithValue("@Other_Terms", grnC.Other_Terms);
                    cmd3.Parameters.AddWithValue("@RCM", grnC.RCM);
                    cmd3.Parameters.AddWithValue("@IneligibleTax", grnC.IneligibleTax);
                    cmd3.Parameters.AddWithValue("@Status", grnC.Status);
                    cmd3.Parameters.AddWithValue("@isDeleted", grnC.isDeleted);
                    cmd3.Parameters.AddWithValue("@BU_ID",grnC.BU_ID);
                    cmd3.Parameters.AddWithValue("@Company_ID", grnC.Company_ID);
                    cmd3.Parameters.AddWithValue("@Created_By", grnC.Created_By);
                    cmd3.Parameters.AddWithValue("@Modified_By", grnC.Modified_By);
                    cmd3.Parameters.AddWithValue("@GRN_Type", grnC.GRN_Type);
                    cmd3.Parameters.AddWithValue("@PF_Charges", Convert.ToDecimal(grnC.PF_Charges));
                    cmd3.Parameters.AddWithValue("@Frieght_Paid",  Convert.ToDecimal(grnC.Frieght_Paid));
                    cmd3.Parameters.AddWithValue("@Hamali_Charges", Convert.ToDecimal(grnC.Hamali_Charges));
                    cmd3.Connection = conOnline;
                    cmd3.ExecuteNonQuery();
                    //Products

                    var grnP = (from s in db.GoodsReceiptNote_Childs
                                join p in db.Products on s.Prod_Code equals p.prod_ID
                                where s.Grn_NO == GRN && s.Company_ID == logIn.company
                                select new {s.Grn_NO,s.Product_Description,s.Prod_Spec,s.Prod_Grade,s.HSN_Code,s.Uom,s.PO_Qty,s.Challan_Qty,
                                s.ReceivedQty,s.Tole_Qty,s.RejectedQty,s.AcceptedQty,s.Price,s.Amount,s.Disc_Per,s.Disc_Amount,s.Taxable_Value,
                                s.CGST_Amnt,s.CGST_Per,s.SGST_Amnt,s.SGST_Per,s.IGST_Amnt,s.IGST_Per,s.Net_Amount,s.Remarks,s.Company_ID,s.TCNo,
                                s.Heat_No,s.PR_No,s.PO_No,s.PR_Date,s.PO_Date,s.ProdSno,s.Int_Batch_No,p.Prod_Code,p.prod_ID}).ToList();
                    for (int k = 0; k < grnP.Count; k++)
                    {
                       
                            //get prod id
                        SqlDataReader myreader2;
                        SqlCommand cmd8 = new SqlCommand();
                        int Prodcode = 0;
                        cmd8.CommandText = "select prod_id from Products  where [Prod_Code] ='" + grnP[k].Prod_Code + "' and Company_ID = '" + logIn.company + "'";
                        cmd8.Connection = conOnline;
                        cmd8.ExecuteNonQuery();
                        myreader2 = cmd8.ExecuteReader();
                        Boolean reccnt1 = false;
                        while (myreader2.Read())
                        {
                            Prodcode = myreader2.GetInt32(0);
                            reccnt1 = true;
                        }

                        if (reccnt1 == false)
                        {
                            var prod = (from s in db.Products
                                        where s.prod_ID == grnP[k].prod_ID && s.Company_ID == logIn.company
                                        select s).FirstOrDefault();
                            SqlCommand cmd7 = new SqlCommand();
                            SqlCommand cmd9 = new SqlCommand();
                            SqlDataReader myreader4;
                            cmd7.CommandText = "insert into [Products]([Prod_Code],[Prod_Name],[Prod_Alias_Name]" +
                                ",[Prod_Type_Id] ,[Prod_Group_Id] ,[Prod_Primary_UOM_Id]  ,[Prod_Alternative_UOM_Id]   ,[Conv_Formula]" +
                                "  ,[Prod_Storage_Location_Id] ,[Prod_Description] ,[Prod_isShelfLife] ,[Prod_Shelf_Life] ,[Prod_IsCritical_Item]" +
                                " ,[Prod_IsBOM_Item] ,[Prod_Scrap_Product_Id] ,[Prod_HSN_Code],[Prod_Tax_Class] " +
                                ",[Prod_Status_ID] ,[Prod_Field1] ,[Prod_Field2] ,[Company_ID] ,[Created_By],[Modified_BY]) " +
                                    "values('" + prod.Prod_Code + "','" + prod.Prod_Name + "','" + prod.Prod_Alias_Name + "','" + prod.Prod_Type_Id + "'," +
                                    "'" + prod.Prod_Group_Id + "','" + prod.Prod_Primary_UOM_Id + "','" + prod.Prod_Alternative_UOM_Id + "','" + prod.Conv_Formula + "'," +
                                    "'" + prod.Prod_Storage_Location_Id + "','" + prod.Prod_Description + "','" + prod.Prod_isShelfLife + "','" + prod.Prod_Shelf_Life + "'," +
                                    "'" + prod.Prod_IsCritical_Item + "', '" + prod.Prod_IsBOM_Item + "','" + prod.Prod_Scrap_Product_Id + "'," +
                                    "'" + prod.Prod_HSN_Code + "','" + prod.Prod_Tax_Class + "','" + prod.Prod_Status_ID + "','" + prod.Prod_Field1 + "'," +
                                    "'" + prod.Prod_Field2 + "','" + prod.Company_ID + "','" + prod.Created_By + "' ,'" + prod.Modified_BY + "' )";
                            // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                            cmd7.Connection = conOnline;
                            cmd7.ExecuteNonQuery();

                            cmd9.CommandText = "select prod_id from Products  where [Prod_Code] ='" + prod.Prod_Code + "' and Company_ID = '" + logIn.company + "'";
                            cmd9.Connection = conOnline;
                            cmd9.ExecuteNonQuery();
                            myreader4 = cmd9.ExecuteReader();

                            myreader4.Read();
                            Prodcode = Convert.ToInt32(myreader4[0].ToString());

                        }




                        cmd4.CommandText = "insert into GoodsReceiptNote_Child([GRN_Master_ID],[Grn_NO],[Prod_Code],[Product_Description],[Prod_Spec]" +
                        ",[Prod_Grade],[HSN_Code],[Uom],[PO_Qty],[Challan_Qty],[ReceivedQty],[Tole_Qty],[RejectedQty],[AcceptedQty],[Price]" +
                        ",[Amount],[Disc_Per],[Disc_Amount],[Taxable_Value],[CGST_Per],[CGST_Amnt],[SGST_Amnt],[SGST_Per],[IGST_Per]" +
                        ",[IGST_Amnt],[Net_Amount],[Remarks],[Company_ID],[TCNo],[Heat_No],[PR_No],[PO_No],[PR_Date],[PO_Date],[ProdSno]" +
                        ",[Int_Batch_No]) " +
                                    "values(@grnid,'" + grnP[k].Grn_NO + "','" + Prodcode + "','" + grnP[k].Product_Description + "','" + grnP[k].Prod_Spec + "'," +
                                    "'" + grnP[k].Prod_Grade + "','" + grnP[k].HSN_Code + "','" + grnP[k].Uom + "','" + grnP[k].PO_Qty + "','" + grnP[k].Challan_Qty + "'," +
                                    "'" + grnP[k].ReceivedQty + "','" + grnP[k].Tole_Qty + "','" + grnP[k].RejectedQty + "','" + grnP[k].AcceptedQty + "','" + grnP[k].Price + "'," +
                                    "'" + grnP[k].Amount + "','" + grnP[k].Disc_Per + "','" + grnP[k].Disc_Amount + "','" + grnP[k].Taxable_Value + "','" + grnP[k].CGST_Per + "'," +
                                    "'" + grnP[k].CGST_Amnt + "','" + grnP[k].SGST_Amnt + "','" + grnP[k].SGST_Per + "','" + grnP[k].IGST_Per + "','" + grnP[k].IGST_Amnt + "'," +
                                    "'" + grnP[k].Net_Amount + "','" + grnP[k].Remarks + "','" + grnP[k].Company_ID + "','" + grnP[k].TCNo + "','" + grnP[k].Heat_No + "'," +
                                    "'" + grnP[k].PR_No + "','" + grnP[k].PO_No + "','" + grnP[k].PR_Date + "','" + grnP[k].PO_Date + "','" + grnP[k].ProdSno + "'," +
                                    "'" + grnP[k].Int_Batch_No + "')";
                        // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                        SqlDataReader myreader;
                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.CommandText = "select ID from GoodsReceiptNote_Master where Grn_NO ='" + GRN + "' and Company_ID = '" + logIn.company + "'";
                        cmd5.Connection = conOnline;
                        cmd5.ExecuteNonQuery();
                        myreader = cmd5.ExecuteReader();

                        myreader.Read();
                        int grnno = Convert.ToInt32(myreader[0].ToString());
                        cmd4.Parameters.Add("@grnid",grnno); 
                        cmd4.Connection = conOnline;
                        cmd4.ExecuteNonQuery();
                        cmd4.Parameters.Clear();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var grnM = (from s in db.Material_Issue_Masters
                        where s.Slip_Date >= dtpFrmDate.Value && s.Slip_Date <= dtpToDate.Value && s.BU_ID == logIn.BU_ID
                        orderby s.Slip_Date
                        select new { s.Slip_NO }).ToList();
            if (grnM.Count > 0)
            {



                for (int i = 0; i < grnM.Count; i++)
                {
                    string GRN = grnM[i].Slip_NO;
                    var grnC = (from s in db.Material_Issue_Masters
                                where s.Slip_NO == GRN && s.BU_ID == logIn.BU_ID
                                select s).FirstOrDefault();
                    // MessageBox.Show(d[i].Grn_NO);

                    if (conOnline.State != ConnectionState.Open)
                        conOnline.Open();
                    //con.Open();

                    SqlCommand cmd1 = new SqlCommand("delete  from [Material_Issue_Child] where [Slip_NO] =@ProdID", conOnline);
                    cmd1.Parameters.AddWithValue("@ProdID", GRN);

                    cmd1.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("delete  from [Material_Issue_Master] where [Grn_NO] =@ProdID", conOnline);
                    cmd2.Parameters.AddWithValue("@ProdID", GRN);
                    // conOnline.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd3.CommandText = "insert into Material_Issue_Master ([Slip_NO],[Slip_Date],[Ref_Doc_Type],[Indent_No],[Issued_To],[Dept_Name]" +
                        ",[Returnable],[Issued_By],[Issued_Person_To],[Remarks],[isDeleted],[BU_ID],[Company_ID] ,[Created_By],[Modified_By]) " +
                                    "values(@Slip_NO,@Slip_Date, @Ref_Doc_Type,@Indent_No,@Issued_To,@Dept_Name,@Returnable,@Issued_By" +
                        " ,@Issued_Person_To,@Remarks,@isDeleted,@BU_ID,@Company_ID,@Created_By,@Modified_By)";
                    //'" + Convert.ToDateTime(grnC.Grn_Date) + "'
                    //'" + Convert.ToDateTime(grnC.DC_Date) + "',
                    //'" + Convert.ToDateTime(grnC.Supplier_InvDate) + "',


                    cmd3.Parameters.AddWithValue("@Slip_NO", grnC.Slip_NO);
                    cmd3.Parameters.AddWithValue("@Slip_Date", grnC.Slip_Date);
                    cmd3.Parameters.AddWithValue("@Ref_Doc_Type", grnC.Ref_Doc_Type);
                    cmd3.Parameters.AddWithValue("@Indent_No", grnC.Indent_No);
                    cmd3.Parameters.AddWithValue("@Issued_To", grnC.Issued_To);
                    cmd3.Parameters.AddWithValue("@Dept_Name", grnC.Dept_Name);
                    //  cmd.Parameters.AddWithValue("@ConsigneeName", Convert.ToInt32(1));
                    // cmd.Parameters.AddWithValue("@Multi_Loc_Delivery", logIn.BU_ID);
                    cmd3.Parameters.AddWithValue("@Returnable", grnC.Returnable);
                    cmd3.Parameters.AddWithValue("@Issued_By", grnC.Issued_By);
                    cmd3.Parameters.AddWithValue("@Issued_Person_To", grnC.Issued_Person_To);
                    cmd3.Parameters.AddWithValue("@Remarks", grnC.Remarks);
                    cmd3.Parameters.AddWithValue("@isDeleted", grnC.isDeleted);
                    cmd3.Parameters.AddWithValue("@BU_ID", grnC.BU_ID);
                    cmd3.Parameters.AddWithValue("@Company_ID", grnC.Company_ID);
                    cmd3.Parameters.AddWithValue("@Created_By", grnC.Created_By);
                    cmd3.Parameters.AddWithValue("@Modified_By", grnC.Modified_By);
                    cmd3.Connection = conOnline;
                    cmd3.ExecuteNonQuery();
                    //Products

                    var grnP = (from s in db.Material_Issue_Childs
                                where s.Slip_NO == GRN && s.Company_ID == logIn.company
                                select s).ToList();
                    for (int k = 0; k < grnP.Count; k++)
                    {
                         SqlDataReader myreader2;
                        SqlCommand cmd8 = new SqlCommand();
                        int Prodcode = 0;
                        cmd8.CommandText = "select prod_id from Products  where [Prod_Code] ='" + grnP[k].Int_Prod_Code + "' and Company_ID = '" + logIn.company + "'";
                        cmd8.Connection = conOnline;
                        cmd8.ExecuteNonQuery();
                        myreader2 = cmd8.ExecuteReader();

                        myreader2.Read();
                        Prodcode = Convert.ToInt32(myreader2[0].ToString());
                        cmd4.CommandText = "insert into [Material_Issue_Child]([Slip_Master_ID],[Slip_NO],Int_Prod_Code,[Prod_Code],[Product_Description],[Prod_Spec]" +
                        ",[Prod_Grade],[Uom],[Indent_Qty],[Stock_Qty],[Issued_Qty],[Issue_Price],[Issue_Value],[Cost_Center],[Remarks]" +
                        ",[Company_ID],[ProdSno]) " +
                                    "values(@slipid,'" + grnP[k].Slip_NO + "','" + grnP[k].Int_Prod_Code + "','" + Prodcode + "','" + grnP[k].Product_Description + "','" + grnP[k].Prod_Spec + "'," +
                                    "'" + grnP[k].Prod_Grade + "','" + grnP[k].Uom + "','" + grnP[k].Indent_Qty + "','" + grnP[k].Stock_Qty + "'," +
                                    "'" + grnP[k].Issued_Qty + "','" + grnP[k].Issue_Price + "','" + grnP[k].Issue_Value + "','" + grnP[k].Cost_Center + "','" + grnP[k].Remarks + "'," +
                                    "'" + grnP[k].Company_ID + "','" + grnP[k].ProdSno +"')";
                        // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                        SqlDataReader myreader;
                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.CommandText = "select ID from Material_Issue_Master where Slip_NO ='" + GRN + "' and Company_ID = '" + logIn.company + "'";
                        cmd5.Connection = conOnline;
                        cmd5.ExecuteNonQuery();
                        myreader = cmd5.ExecuteReader();

                        myreader.Read();
                        int grnno = Convert.ToInt32(myreader[0].ToString());
                        cmd4.Parameters.Add("@slipid", grnno);
                        cmd4.Connection = conOnline;
                        cmd4.ExecuteNonQuery();
                        cmd4.Parameters.Clear();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var grnM = (from s in db.Purchase_Req_Masters
                        where s.Req_Date >= dtpFrmDate.Value && s.Req_Date <= dtpToDate.Value && s.BU_ID == logIn.BU_ID
                        orderby s.Req_Date 
                        select new { s.PR_NO }).ToList();
            if (grnM.Count > 0)
            {



                for (int i = 0; i < grnM.Count; i++)
                {
                    string GRN = grnM[i].PR_NO;
                    var grnC = (from s in db.Purchase_Req_Masters
                                where s.PR_NO == GRN && s.BU_ID == logIn.BU_ID
                                select s).FirstOrDefault();
                    // MessageBox.Show(d[i].Grn_NO);

                    if (conOnline.State != ConnectionState.Open)
                        conOnline.Open();
                    //con.Open();

                    SqlCommand cmd1 = new SqlCommand("delete  from [Purchase_Req_Child] where [PR_No] =@ProdID", conOnline);
                    cmd1.Parameters.AddWithValue("@ProdID", GRN);

                    cmd1.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("delete  from [Purchase_Req_Master] where [PR_No] =@ProdID", conOnline);
                    cmd2.Parameters.AddWithValue("@ProdID", GRN);
                    cmd2.ExecuteNonQuery();
                    // conOnline.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd3.CommandText = "insert into Purchase_Req_Master ([PR_NO],[Req_Date],[Dept_Name],[Indented_By],[Indentor_Mobile],[Ref_Doc]" +
                        ",[Asset_Code],[Remarks],[Status],[isDeleted],[BU_ID],[Company_ID],[Created_By],[Modified_By]) " +
                                    "values(@Slip_NO,@Slip_Date, @Dept_Name,@Indented_By,@Indentor_Mobile,@Ref_Doc,@Asset_Code,@Remarks" +
                        " ,@Status,@isDeleted,@BU_ID,@Company_ID,@Created_By,@Modified_By)";
                    //'" + Convert.ToDateTime(grnC.Grn_Date) + "'
                    //'" + Convert.ToDateTime(grnC.DC_Date) + "',
                    //'" + Convert.ToDateTime(grnC.Supplier_InvDate) + "',


                    cmd3.Parameters.AddWithValue("@Slip_NO", grnC.PR_NO);
                    cmd3.Parameters.AddWithValue("@Slip_Date", grnC.Req_Date);
                    cmd3.Parameters.AddWithValue("@Dept_Name", grnC.Dept_Name);
                    cmd3.Parameters.AddWithValue("@Indented_By", grnC.Indented_By);
                    cmd3.Parameters.AddWithValue("@Indentor_Mobile", grnC.Indentor_Mobile);
                    cmd3.Parameters.AddWithValue("@Ref_Doc", grnC.Ref_Doc);
                    cmd3.Parameters.AddWithValue("@Asset_Code", grnC.Asset_Code);
                    cmd3.Parameters.AddWithValue("@Remarks", grnC.Remarks);
                    cmd3.Parameters.AddWithValue("@Status", grnC.Status);
                    cmd3.Parameters.AddWithValue("@isDeleted", grnC.isDeleted);
                    cmd3.Parameters.AddWithValue("@BU_ID", grnC.BU_ID);
                    cmd3.Parameters.AddWithValue("@Company_ID", grnC.Company_ID);
                    cmd3.Parameters.AddWithValue("@Created_By", grnC.Created_By);
                    cmd3.Parameters.AddWithValue("@Modified_By", grnC.Modified_By);                    
                    cmd3.Connection = conOnline;
                    cmd3.ExecuteNonQuery();
                    //Products

                    var grnP = (from s in db.Purchase_Req_Childs
                                join p in db.Products on s.Prod_Code equals p.prod_ID
                                where s.PR_NO == GRN && s.Company_ID == logIn.company
                                select new
                                {
                                    s.PR_NO,
                                    s.Product_Description,
                                    s.Prod_Spec,
                                    s.Prod_Grade,
                                    s.Uom,
                                    s.Stock_Qty,
                                    s.PR_Qty,
                                    s.Required_On,
                                    s.Consumption_3Months,
                                    s.Last_Purchase_On,
                                    s.Remarks,
                                    s.Company_ID,
                                    s.ProdSno,
                                    p.prod_ID,
                                    p.Prod_Code
                                }).ToList();
                    for (int k = 0; k < grnP.Count; k++)
                    {
                        int Prodcode=0;
                        SqlDataReader myreader1;
                        SqlCommand cmd6 = new SqlCommand();
                        cmd6.CommandText = "select prod_id,Prod_Customer_Code from Products  where [Prod_Code] ='" + grnP[k].Prod_Code  + "'";
                        cmd6.Connection = conOnline;
                        cmd6.ExecuteNonQuery();
                        myreader1 = cmd6.ExecuteReader();
                        Boolean reccnt = false;
                        while (myreader1.Read())
                        {
                            Prodcode = myreader1.GetInt32(0);
                            reccnt = true;
                        }
                        if(reccnt==false)
                        {
                            var prod = (from s in db.Products
                                        where s.prod_ID == grnP[k].prod_ID && s.Company_ID == logIn.company
                                        select s).FirstOrDefault();
                            SqlCommand cmd7 = new SqlCommand();
                            SqlCommand cmd8 = new SqlCommand();
                            SqlDataReader myreader2;
                            cmd7.CommandText = "insert into [Products]([Prod_Code],[Prod_Name],[Prod_Alias_Name]" +
                                ",[Prod_Type_Id] ,[Prod_Group_Id] ,[Prod_Primary_UOM_Id]  ,[Prod_Alternative_UOM_Id]   ,[Conv_Formula]" +
                                "  ,[Prod_Storage_Location_Id] ,[Prod_Description] ,[Prod_isShelfLife] ,[Prod_Shelf_Life] ,[Prod_IsCritical_Item]" +
                                " ,[Prod_IsBOM_Item] ,[Prod_Scrap_Product_Id] ,[Prod_HSN_Code],[Prod_Tax_Class] " +
                                ",[Prod_Status_ID] ,[Prod_Field1] ,[Prod_Field2] ,[Company_ID] ,[Created_By],[Modified_BY]) " +
                                    "values('" + prod.Prod_Code + "','" +prod.Prod_Name + "','" + prod.Prod_Alias_Name + "','" + prod.Prod_Type_Id + "'," +
                                    "'" + prod.Prod_Group_Id + "','" + prod.Prod_Primary_UOM_Id + "','" + prod.Prod_Alternative_UOM_Id + "','" + prod.Conv_Formula + "'," +
                                    "'" + prod.Prod_Storage_Location_Id + "','" + prod.Prod_Description + "','" + prod.Prod_isShelfLife + "','" + prod.Prod_Shelf_Life + "'," +
                                    "'" + prod.Prod_IsCritical_Item + "', '" + prod.Prod_IsBOM_Item + "','" + prod.Prod_Scrap_Product_Id + "'," +
                                    "'" + prod.Prod_HSN_Code + "','" + prod.Prod_Tax_Class + "','" + prod.Prod_Status_ID + "','" + prod.Prod_Field1 + "'," +
                                    "'" + prod.Prod_Field2 + "','" + prod.Company_ID + "','" + prod.Created_By + "' ,'" + prod.Modified_BY + "' )";
                            // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                            cmd7.Connection = conOnline;
                            cmd7.ExecuteNonQuery();

                            cmd8.CommandText = "select prod_id  from Products  where [Prod_Code] ='" + grnP[k].Prod_Code + "' and Company_ID = '" + logIn.company + "'";
                            cmd8.Connection = conOnline;
                            cmd8.ExecuteNonQuery();
                            myreader2 = cmd8.ExecuteReader();

                            myreader2.Read();
                            Prodcode = Convert.ToInt32(myreader2[0].ToString());

                        }
                        

                       

                            cmd4.CommandText = "insert into [Purchase_Req_Child]([PR_Master_ID],[PR_NO],[Prod_Code],[Product_Description],[Prod_Spec]" +
                            ",[Prod_Grade],[Uom],[Stock_Qty],[PR_Qty],[Required_On],[Consumption_3Months],[Last_Purchase_On]" +
                            ",[Remarks],[Company_ID],[ProdSno]) " +
                                    "values(@slipid,'" + grnP[k].PR_NO + "','" + Prodcode + "','" + grnP[k].Product_Description + "','" + grnP[k].Prod_Spec + "'," +
                                    "'" + grnP[k].Prod_Grade + "','" + grnP[k].Uom + "','" + grnP[k].Stock_Qty + "','" + grnP[k].PR_Qty + "'," +
                                    "'" + grnP[k].Required_On + "','" + grnP[k].Consumption_3Months + "','" + grnP[k].Last_Purchase_On + "','" + grnP[k].Remarks + "'," +
                                    "'" + grnP[k].Company_ID + "','" + grnP[k].ProdSno + "')";
                        // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                        SqlDataReader myreader;
                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.CommandText = "select ID from Purchase_Req_Master where PR_NO ='" + GRN + "' and Company_ID = '" + logIn.company + "'";
                        cmd5.Connection = conOnline;
                        cmd5.ExecuteNonQuery();
                        myreader = cmd5.ExecuteReader();

                        myreader.Read();
                        int grnno = Convert.ToInt32(myreader[0].ToString());
                        cmd4.Parameters.Add("@slipid", grnno);
                        cmd4.Connection = conOnline;
                        cmd4.ExecuteNonQuery();
                        cmd4.Parameters.Clear();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var grnM = (from s in db.Supplier_informations
                        where  s.Company_ID == logIn.company
                        
                        select new { s.Supplier_Name }).ToList();
            if (grnM.Count > 0)
            {



                for (int i = 0; i < grnM.Count; i++)
                {
                    string GRN = grnM[i].Supplier_Name;
                    var grnC = (from s in db.Purchase_Req_Masters
                                where s.PR_NO == GRN && s.BU_ID == logIn.BU_ID
                                select s).FirstOrDefault();
                    // MessageBox.Show(d[i].Grn_NO);

                    if (conOnline.State != ConnectionState.Open)
                        conOnline.Open();
                    //con.Open();

                    SqlCommand cmd1 = new SqlCommand("delete  from [Purchase_Req_Child] where [PR_No] =@ProdID", conOnline);
                    cmd1.Parameters.AddWithValue("@ProdID", GRN);

                    cmd1.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("delete  from [Purchase_Req_Master] where [PR_No] =@ProdID", conOnline);
                    cmd2.Parameters.AddWithValue("@ProdID", GRN);
                    cmd2.ExecuteNonQuery();
                    // conOnline.Close();

                    SqlCommand cmd3 = new SqlCommand();
                    SqlCommand cmd4 = new SqlCommand();
                    cmd3.CommandText = "insert into Purchase_Req_Master ([PR_NO],[Req_Date],[Dept_Name],[Indented_By],[Indentor_Mobile],[Ref_Doc]" +
                        ",[Asset_Code],[Remarks],[Status],[isDeleted],[BU_ID],[Company_ID],[Created_By],[Modified_By]) " +
                                    "values(@Slip_NO,@Slip_Date, @Dept_Name,@Indented_By,@Indentor_Mobile,@Ref_Doc,@Asset_Code,@Remarks" +
                        " ,@Status,@isDeleted,@BU_ID,@Company_ID,@Created_By,@Modified_By)";
                    //'" + Convert.ToDateTime(grnC.Grn_Date) + "'
                    //'" + Convert.ToDateTime(grnC.DC_Date) + "',
                    //'" + Convert.ToDateTime(grnC.Supplier_InvDate) + "',


                    cmd3.Parameters.AddWithValue("@Slip_NO", grnC.PR_NO);
                    cmd3.Parameters.AddWithValue("@Slip_Date", grnC.Req_Date);
                    cmd3.Parameters.AddWithValue("@Dept_Name", grnC.Dept_Name);
                    cmd3.Parameters.AddWithValue("@Indented_By", grnC.Indented_By);
                    cmd3.Parameters.AddWithValue("@Indentor_Mobile", grnC.Indentor_Mobile);
                    cmd3.Parameters.AddWithValue("@Ref_Doc", grnC.Ref_Doc);
                    cmd3.Parameters.AddWithValue("@Asset_Code", grnC.Asset_Code);
                    cmd3.Parameters.AddWithValue("@Remarks", grnC.Remarks);
                    cmd3.Parameters.AddWithValue("@Status", grnC.Status);
                    cmd3.Parameters.AddWithValue("@isDeleted", grnC.isDeleted);
                    cmd3.Parameters.AddWithValue("@BU_ID", grnC.BU_ID);
                    cmd3.Parameters.AddWithValue("@Company_ID", grnC.Company_ID);
                    cmd3.Parameters.AddWithValue("@Created_By", grnC.Created_By);
                    cmd3.Parameters.AddWithValue("@Modified_By", grnC.Modified_By);
                    cmd3.Connection = conOnline;
                    cmd3.ExecuteNonQuery();
                    //Products

                    var grnP = (from s in db.Purchase_Req_Childs
                                join p in db.Products on s.Prod_Code equals p.prod_ID
                                where s.PR_NO == GRN && s.Company_ID == logIn.company
                                select new
                                {
                                    s.PR_NO,
                                    s.Product_Description,
                                    s.Prod_Spec,
                                    s.Prod_Grade,
                                    s.Uom,
                                    s.Stock_Qty,
                                    s.PR_Qty,
                                    s.Required_On,
                                    s.Consumption_3Months,
                                    s.Last_Purchase_On,
                                    s.Remarks,
                                    s.Company_ID,
                                    s.ProdSno,
                                    p.prod_ID,
                                    p.Prod_Code
                                }).ToList();
                    for (int k = 0; k < grnP.Count; k++)
                    {
                        int Prodcode = 0;
                        SqlDataReader myreader1;
                        SqlCommand cmd6 = new SqlCommand();
                        cmd6.CommandText = "select prod_id,Prod_Customer_Code from Products  where [Prod_Code] ='" + grnP[k].Prod_Code + "'";
                        cmd6.Connection = conOnline;
                        cmd6.ExecuteNonQuery();
                        myreader1 = cmd6.ExecuteReader();
                        Boolean reccnt = false;
                        while (myreader1.Read())
                        {
                            Prodcode = myreader1.GetInt32(0);
                            reccnt = true;
                        }
                        if (reccnt == false)
                        {
                            var prod = (from s in db.Products
                                        where s.prod_ID == grnP[k].prod_ID && s.Company_ID == logIn.company
                                        select s).FirstOrDefault();
                            SqlCommand cmd7 = new SqlCommand();
                            SqlCommand cmd8 = new SqlCommand();
                            SqlDataReader myreader2;
                            cmd7.CommandText = "insert into [Products]([Prod_Code],[Prod_Name],[Prod_Alias_Name]" +
                                ",[Prod_Type_Id] ,[Prod_Group_Id] ,[Prod_Primary_UOM_Id]  ,[Prod_Alternative_UOM_Id]   ,[Conv_Formula]" +
                                "  ,[Prod_Storage_Location_Id] ,[Prod_Description] ,[Prod_isShelfLife] ,[Prod_Shelf_Life] ,[Prod_IsCritical_Item]" +
                                " ,[Prod_IsBOM_Item] ,[Prod_Scrap_Product_Id] ,[Prod_HSN_Code],[Prod_Tax_Class] " +
                                ",[Prod_Status_ID] ,[Prod_Field1] ,[Prod_Field2] ,[Company_ID] ,[Created_By],[Modified_BY]) " +
                                    "values('" + prod.Prod_Code + "','" + prod.Prod_Name + "','" + prod.Prod_Alias_Name + "','" + prod.Prod_Type_Id + "'," +
                                    "'" + prod.Prod_Group_Id + "','" + prod.Prod_Primary_UOM_Id + "','" + prod.Prod_Alternative_UOM_Id + "','" + prod.Conv_Formula + "'," +
                                    "'" + prod.Prod_Storage_Location_Id + "','" + prod.Prod_Description + "','" + prod.Prod_isShelfLife + "','" + prod.Prod_Shelf_Life + "'," +
                                    "'" + prod.Prod_IsCritical_Item + "', '" + prod.Prod_IsBOM_Item + "','" + prod.Prod_Scrap_Product_Id + "'," +
                                    "'" + prod.Prod_HSN_Code + "','" + prod.Prod_Tax_Class + "','" + prod.Prod_Status_ID + "','" + prod.Prod_Field1 + "'," +
                                    "'" + prod.Prod_Field2 + "','" + prod.Company_ID + "','" + prod.Created_By + "' ,'" + prod.Modified_BY + "' )";
                            // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                            cmd7.Connection = conOnline;
                            cmd7.ExecuteNonQuery();

                            cmd8.CommandText = "select prod_id  from Products  where [Prod_Code] ='" + grnP[k].Prod_Code + "' and Company_ID = '" + logIn.company + "'";
                            cmd8.Connection = conOnline;
                            cmd8.ExecuteNonQuery();
                            myreader2 = cmd8.ExecuteReader();

                            myreader2.Read();
                            Prodcode = Convert.ToInt32(myreader2[0].ToString());

                        }




                        cmd4.CommandText = "insert into [Purchase_Req_Child]([PR_Master_ID],[PR_NO],[Prod_Code],[Product_Description],[Prod_Spec]" +
                        ",[Prod_Grade],[Uom],[Stock_Qty],[PR_Qty],[Required_On],[Consumption_3Months],[Last_Purchase_On]" +
                        ",[Remarks],[Company_ID],[ProdSno]) " +
                                "values(@slipid,'" + grnP[k].PR_NO + "','" + Prodcode + "','" + grnP[k].Product_Description + "','" + grnP[k].Prod_Spec + "'," +
                                "'" + grnP[k].Prod_Grade + "','" + grnP[k].Uom + "','" + grnP[k].Stock_Qty + "','" + grnP[k].PR_Qty + "'," +
                                "'" + grnP[k].Required_On + "','" + grnP[k].Consumption_3Months + "','" + grnP[k].Last_Purchase_On + "','" + grnP[k].Remarks + "'," +
                                "'" + grnP[k].Company_ID + "','" + grnP[k].ProdSno + "')";
                        // var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == GRN && a.isDeleted == false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                        SqlDataReader myreader;
                        SqlCommand cmd5 = new SqlCommand();
                        cmd5.CommandText = "select ID from Purchase_Req_Master where PR_NO ='" + GRN + "' and Company_ID = '" + logIn.company + "'";
                        cmd5.Connection = conOnline;
                        cmd5.ExecuteNonQuery();
                        myreader = cmd5.ExecuteReader();

                        myreader.Read();
                        int grnno = Convert.ToInt32(myreader[0].ToString());
                        cmd4.Parameters.Add("@slipid", grnno);
                        cmd4.Connection = conOnline;
                        cmd4.ExecuteNonQuery();
                        cmd4.Parameters.Clear();
                    }
                }
            }
        }
    }
}
