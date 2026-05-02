using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using Ione_DAL;
using ioneNet.ProductionManagement.Transactions;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmEnqProductSpecs : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SpecType;
        public static string finishWt, ForgeWt;
        public static string mtrlClassGroup;
        public string OutDia = "";
        public string InnerDia = "";
        public decimal OWt = 0;
        public decimal IWt = 0;
        public frmEnqProductSpecs()
        {
            InitializeComponent();
        }


        private void frmEnqProductSpecs_Load(object sender, EventArgs e)
        {
            try
            {

                //Bind Shapes
                var Buyerblind = (from m in db.Forg_Material_Class_Masters select new { m.id, m.Material_Class }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbForgingShape.DataSource = Buyerblind;
                    cmbForgingShape.ValueMember = "id";
                    cmbForgingShape.DisplayMember = "Material_Class";
                    cmbForgingShape.SelectedIndex = -1;
                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Supply Condition" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbSupplyCondition.DataSource = pStatus;
                    cmbSupplyCondition.ValueMember = "ID";
                    cmbSupplyCondition.DisplayMember = "Descr";

                    cmbSupplyCondition.SelectedIndex = -1;
                }

                string MtrlGrade = "";
                if (frmMain.frmname == "Enquiry")
                {
                    txtEnqNo.Text = frmNewEnquiry.Enq_NO;
                    txtItemShape.Text = frmNewEnquiry.Item_Shape;
                    txtItemCode.Text = frmNewEnquiry.ItemCode;
                    MtrlGrade = frmNewEnquiry.MtrlGrade;
                }
                else
                if (frmMain.frmname == "Costing")
                {
                    txtEnqNo.Text = frmCostingSheet.Enq_NO;
                    txtItemShape.Text = frmCostingSheet.Item_Shape;
                    txtItemCode.Text = frmCostingSheet.ItemCode;
                    MtrlGrade = frmCostingSheet.MtrlGrade;
                }
                else
                if (frmMain.frmname == "Quotation")
                {
                    txtEnqNo.Text = frmQuotation.Enq_NO;
                    MtrlGrade = frmQuotation.MtrlGrade;
                    txtItemCode.Text = frmQuotation.ItemCode;
                }
                else
                if (frmMain.frmname == "Order")
                {
                    txtEnqNo.Text = frmNewOrder_Forging.Enq_NO;
                    MtrlGrade = frmNewOrder_Forging.MtrlGrade;
                    txtItemCode.Text = frmNewOrder_Forging.ItemCode;
                }
                else
                if (frmMain.frmname == "MO")
                {
                    txtEnqNo.Text = frmManufacturingOrder.Enq_NO;
                    MtrlGrade = frmManufacturingOrder.MtrlGrade;
                    txtItemCode.Text = frmManufacturingOrder.ItemCode;
                }
                //Get Shape
                var Shape = (from m in db.Sale_Enquiry_Childs
                             where m.Enq_NO == txtEnqNo.Text && m.Prod_Code == Convert.ToInt32(txtItemCode.Text)
                             select new { m.Prod_Shape, }).ToList();
                if (Shape.Count > 0)
                {
                    txtItemShape.Text = Shape[0].Prod_Shape;
                }
                else
                {
                    txtItemShape.Text = frmNewEnquiry.Item_Shape;
                }

                //Get Density
                var MDensity = (from m in db.MaterialGrades
                                where m.Material_Grade == MtrlGrade
                                select new { m.Density, m.App_Specs }).ToList();
                if (MDensity.Count > 0)
                {

                    txtDensity.Text = MDensity[0].Density.ToString();
                    //Bind HT Condition
                    string s = MDensity[0].App_Specs.ToString();
                    string[] values = s.Split(',');
                    cmbHTCondition.Items.Clear();
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                        string m = values[j].ToString();
                        cmbHTCondition.Items.Add(m);

                    }
                }


                //Get Mtrl Class Group
                var MClass = (from m in db.Forg_Material_Class_Masters
                              where m.Material_Class == txtItemShape.Text
                              select new { m.Material_Class_Group, }).ToList();
                if (MClass.Count > 0)
                {

                    txtMtrlClass.Text = MClass[0].Material_Class_Group.ToString();
                }

                //ENable user to enter weights manually if shape is others
                txtFinishWt.Enabled = false;
                txtForgeWt.Enabled = false;
                txtProofWt.Enabled = false;
                txtForgingSize.Enabled = false;
                txtFinishSize.Enabled = false;
                txtProofSize.Enabled = false;
                if (txtItemShape.Text =="Others")
                {
                    txtFinishWt.Enabled = true;
                    txtForgeWt.Enabled = true;
                    txtProofWt.Enabled = true;
                    txtForgingSize.Text = "NA";
                    txtFinishSize.Text = "NA";
                    txtProofSize.Text = "NA";
                    txtForgingSize.Enabled = true;
                    txtFinishSize.Enabled = true;
                    txtProofSize.Enabled = true;
                }

                //Bind Data
                var BindData = (from m in db.Sale_Enquiry_Prod_Specs
                                where m.Enq_No == txtEnqNo.Text && m.Company_ID == logIn.company && m.Prod_Code == Convert.ToInt32(txtItemCode.Text)
                                select new
                                {
                                    m.Finishing_Size,
                                    m.Finish_Wt,
                                    m.Proof_Size,
                                    m.ProofMachining_Wt,
                                    m.Forging_Size,
                                    m.Forging_Wt,
                                    m.Supply_Codition,
                                    m.HT_Condition,
                                    m.Forging_Shape,
                                    m.Drawing_File_Path,
                                    m.Qap_File_Path
                                }).Distinct().ToList();
                if (BindData.Count > 0)
                {
                    txtFinishSize.Text = BindData[0].Finishing_Size;
                    txtFinishWt.Text = BindData[0].Finish_Wt.ToString();
                    txtProofSize.Text = BindData[0].Proof_Size;
                    txtProofWt.Text = BindData[0].ProofMachining_Wt.ToString();
                    txtForgingSize.Text = BindData[0].Forging_Size;
                    txtForgeWt.Text = BindData[0].Forging_Wt.ToString();
                    cmbSupplyCondition.SelectedValue = BindData[0].Supply_Codition;
                    cmbHTCondition.Text = BindData[0].HT_Condition;
                    cmbForgingShape.Text = BindData[0].Forging_Shape;
                    txtDrawingLink.Text = BindData[0].Drawing_File_Path;
                    txtQAPLink.Text = BindData[0].Qap_File_Path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ForgeWt = txtForgeWt.Text;
            if (cmbSupplyCondition.Text == "Proof Machined")
            {
                finishWt = txtProofWt.Text;
            }
            if (cmbSupplyCondition.Text == "Finish Machined")
            {
                finishWt = txtFinishWt.Text;
            }
            if (cmbSupplyCondition.Text == "Forged")
            {
                finishWt = txtForgeWt.Text;
            }

            this.Close();

        }

        private void label14_Click(object sender, EventArgs e)

        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SpecType = "Finish";
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                mtrlClassGroup = txtMtrlClass.Text;
                switch (txtMtrlClass.Text)
                {
                    case "Stepped - Shaft":
                    case "Spacer":
                        var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                   where s.Enq_No == txtEnqNo.Text && s.Prod_Code == Convert.ToInt32(txtItemCode.Text) && s.Company_ID == logIn.company && s.Prod_Stage == SpecType
                                   
                                   select new

                                   {
                                       Step = s.Step_No,
                                       s.Outer_Dia,
                                       Thick = s.Outer_Thick,
                                       OD_Wt = s.Outer_Wt,
                                       s.Inner_Dia,
                                       s.Inner_Thick,
                                       ID_Wt = s.Inner_Wt,
                                       s.CB

                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count > 0)
                            dataGridView1.DataSource = dtr;
                        groupBox3.Visible = true;
                        break;
                    case "Blank":
                    case "Round":
                        label7.Text = "Dia";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = false;
                        if (txtFinishSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtFinishSize.Text;
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    txtLength.Text = val.ToString();
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Ring/Tube":
                        label7.Text = "OD";
                        label8.Text = "ID";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = true;
                        if (txtFinishSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtFinishSize.Text;
                            // string S1 = CharacterCasing.Upper('X');
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {

                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtWidth.Text = val.ToString();
                                    }
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Flat":
                        label7.Text = "Width";
                        label8.Text = "Thick";
                        label9.Text = "Length";

                        if (txtFinishSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtFinishSize.Text;
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {
                                        txtLength.Text = m;
                                    }
                                    else
                                    {
                                        txtWidth.Text = m;
                                    }
                                }
                                else
                                {
                                    txtDia.Text = m;
                                }
                            }

                        }

                        txtWidth.Enabled = true;
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {

            try
            {
                decimal Width = (txtWidth.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtWidth.Text);
                decimal Length = (txtLength.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtLength.Text);
                decimal dia = (txtDia.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDia.Text);
                decimal Density = (txtDensity.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDensity.Text);
                decimal pievalue = Convert.ToDecimal("0.000785714");
                decimal Wt = 0;
                string Size = "";
                switch (mtrlClassGroup)
                {
                    case "Blank":
                    case "Round":

                        Wt = (dia * dia * Density * Length * pievalue) / 1000;
                        Size = "" + (char)216 + txtDia.Text + 'X' + txtLength.Text;
                        break;
                    case "Ring/Tube":
                        decimal ODWt = (dia * dia * Density * Length * pievalue) / 1000;
                        decimal IDWt = (Width * Width * Density * Length * pievalue) / 1000;

                        Wt = ODWt - IDWt;
                        Size = "" + (char)216 + txtDia.Text + 'X' + "" + (char)216 + txtWidth.Text + 'X' + txtLength.Text;
                        break;
                    case "Flat":

                        Wt = (dia * Width * Density * Length) / 1000000;
                        Size = txtDia.Text + 'X' + txtWidth.Text + 'X' + txtLength.Text;
                        break;
                }
                if (SpecType == "Finish")
                {
                    txtFinishWt.Text = Wt.ToString("0.000");
                    txtFinishSize.Text = Size;
                }
                if (SpecType == "Forge")
                {
                    txtForgeWt.Text = Wt.ToString("0.000");
                    txtForgingSize.Text = Size;
                }
                if (SpecType == "Proof")
                {
                    txtProofWt.Text = Wt.ToString("0.000");
                    txtProofSize.Text = Size;
                }
                //string size222 = ""+ (char)216 ; 
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                groupBox2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SpecType = "Proof";
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                mtrlClassGroup = txtMtrlClass.Text;
                switch (txtMtrlClass.Text)
                {
                    case "Stepped - Shaft":
                    case "Spacer":
                        var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                   where s.Enq_No == txtEnqNo.Text && s.Prod_Code == Convert.ToInt32(txtItemCode.Text) && s.Company_ID == logIn.company && s.Prod_Stage == SpecType

                                   select new

                                   {
                                       Step = s.Step_No,
                                       s.Outer_Dia,
                                       Thick = s.Outer_Thick,
                                       OD_Wt = s.Outer_Wt,
                                       s.Inner_Dia,
                                       s.Inner_Thick,
                                       ID_Wt = s.Inner_Wt,
                                       s.CB
                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count > 0)
                            dataGridView1.DataSource = dtr;
                        groupBox3.Visible = true;
                        break;
                    case "Blank":
                    case "Round":
                        label7.Text = "Dia";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = false;
                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    txtLength.Text = val.ToString();
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Ring/Tube":
                        label7.Text = "OD";
                        label8.Text = "ID";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = true;
                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            // string S1 = CharacterCasing.Upper('X');
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {

                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtWidth.Text = val.ToString();
                                    }
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Flat":
                        label7.Text = "Width";
                        label8.Text = "Thick";
                        label9.Text = "Length";
                        txtWidth.Enabled = true;

                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            string[] values = s.Split('x');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();

                                if (txtDia.Text != "")
                                {
                                    txtWidth.Text = m;
                                }
                                else if (txtWidth.Text != "")
                                {
                                    txtLength.Text = m;
                                }
                                else
                                {
                                    txtDia.Text = m;
                                }
                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                SpecType = "Forge";
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";              
                //Get Mtrl Class Group
                var MClass = (from m in db.Forg_Material_Class_Masters
                              where m.Material_Class == cmbForgingShape.Text
                              select new { m.Material_Class_Group, }).ToList();
                if (MClass.Count > 0)
                {

                    mtrlClassGroup = MClass[0].Material_Class_Group.ToString();
                }
                switch (mtrlClassGroup)
                {
                    case "Stepped - Shaft":
                    case "Spacer":
                        var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                   where s.Enq_No == txtEnqNo.Text && s.Prod_Code == Convert.ToInt32(txtItemCode.Text) && s.Company_ID == logIn.company && s.Prod_Stage == SpecType

                                   select new

                                   {
                                       Step = s.Step_No,
                                       s.Outer_Dia,
                                       Thick = s.Outer_Thick,
                                       OD_Wt = s.Outer_Wt,
                                       s.Inner_Dia,
                                       s.Inner_Thick,
                                       ID_Wt = s.Inner_Wt,
                                       s.CB
                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count > 0)
                            dataGridView1.DataSource = dtr;
                        groupBox3.Visible = true;
                        break;
                    case "Blank":
                    case "Round":
                        label7.Text = "Dia";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = false;
                        if (txtForgingSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtForgingSize.Text;
                            string[] values = s.Split('X');

                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    txtLength.Text = val.ToString();
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Ring/Tube":
                        label7.Text = "OD";
                        label8.Text = "ID";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = true;
                        if (txtForgingSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtForgingSize.Text;
                            // string S1 = CharacterCasing.Upper('X');
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {

                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtWidth.Text = val.ToString();
                                    }
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Flat":
                        label7.Text = "Width";
                        label8.Text = "Thick";
                        label9.Text = "Length";
                        txtWidth.Enabled = true;

                        if (txtForgingSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtForgingSize.Text;
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {
                                        txtLength.Text = m;
                                    }
                                    else
                                    {
                                        txtWidth.Text = m;
                                    }
                                }
                                else if (txtWidth.Text != "")
                                {
                                    txtLength.Text = m;
                                }
                                else
                                {
                                    txtDia.Text = m;
                                }
                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbSupplyCondition.Text == "Proof Machined")
                {
                    if (txtProofWt.Text == "" || Convert.ToDecimal(txtProofWt.Text) == 0)
                    {
                        MessageBox.Show("Proof Machine Wt Cannot Be Blank When Supply Condition is Proof Machined");
                        return;
                    }
                }
                if (cmbSupplyCondition.Text == "Finish Machined")
                {
                    if (txtFinishWt.Text == "" || Convert.ToDecimal(txtFinishWt.Text) == 0)
                    {
                        MessageBox.Show("Finish Wt Cannot Be Blank When Supply Condition is Finish Machined");
                        return;
                    }
                }
                if (cmbSupplyCondition.Text == "Forged")
                {
                    if (txtForgeWt.Text == "" || Convert.ToDecimal(txtForgeWt.Text) == 0)
                    {
                        MessageBox.Show("Forge Wt Cannot Be Blank When Supply Condition is Forged");
                        return;
                    }
                }
                if (cmbSupplyCondition.Text == "")
                {

                    MessageBox.Show("Select Supply Condition");
                    cmbSupplyCondition.Focus();
                    return;
                }
                else
                {
                    if (cmbHTCondition.Text == "")
                    {

                        MessageBox.Show("Select HT Condition");
                        cmbHTCondition.Focus();
                        return;
                    }

                    else
                    {
                        if ((from a in db.Sale_Enquiry_Prod_Specs
                             where a.Company_ID == logIn.company && a.Enq_No == txtEnqNo.Text && a.Prod_Code == Convert.ToInt32(txtItemCode.Text)
                             select a).Count() > 0)
                        {



                            var p1 = db.Sale_Enquiry_Prod_Specs.Where(w => w.Prod_Code == Convert.ToInt32(txtItemCode.Text) && w.Enq_No == txtEnqNo.Text && w.Company_ID == logIn.company).FirstOrDefault();


                            //Production_Report pb = new Production_Report();


                            p1.Forging_Shape = cmbForgingShape.Text;
                            p1.Finishing_Size = (txtFinishSize.Text == null) ? "" : txtFinishSize.Text;
                            p1.Proof_Size = (txtProofSize.Text == null) ? "" : txtProofSize.Text;
                            p1.Forging_Size = (txtForgingSize.Text == null) ? "" : txtForgingSize.Text;
                            p1.HT_Condition = cmbHTCondition.Text;
                            p1.Supply_Codition = Convert.ToInt32(cmbSupplyCondition.SelectedValue.ToString());
                            // decimal ODia = (R1.Cells["Outer_Dia"].Value == "" || R1.Cells["Outer_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Outer_Dia"].Value);

                            p1.Finish_Wt = (txtFinishWt.Text == "" || txtFinishWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtFinishWt.Text);
                            p1.ProofMachining_Wt = (txtProofWt.Text == "" || txtProofWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtProofWt.Text);
                            p1.Forging_Wt = (txtForgeWt.Text == "" || txtForgeWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtForgeWt.Text);
                            p1.Drawing_File_Path = txtDrawingLink.Text;
                            //if (txtDrawingLink.Text != "")
                            //{
                            //    string varFilePath = txtDrawingLink.Text;
                            //    byte[] file;
                            //    using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                            //    {
                            //        using (var reader = new BinaryReader(stream))
                            //        {
                            //            file = reader.ReadBytes((int)stream.Length);
                            //        }
                            //    }

                            //    p1.Drawing_File = file;
                            //}
                            //if (txtQAPLink.Text != "")
                            //{
                            //    string varFilePath = txtQAPLink.Text;
                            //    byte[] file;
                            //    using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                            //    {
                            //        using (var reader = new BinaryReader(stream))
                            //        {
                            //            file = reader.ReadBytes((int)stream.Length);
                            //        }
                            //    }

                            //    p1.QAP_File = file;
                            //}
                            p1.Qap_File_Path = txtQAPLink.Text;
                            p1.Company_ID = logIn.company;
                            //pb.Created_By = lnkus1;
                            p1.Modified_By = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();
                            MessageBox.Show("Record Updated Sucessfully");
                            return;
                        }
                        else
                        {

                            Sale_Enquiry_Prod_Spec pb = new Sale_Enquiry_Prod_Spec();

                            pb.Enq_No = txtEnqNo.Text;
                            pb.Prod_Code = Convert.ToInt32(txtItemCode.Text);
                            pb.Forging_Shape = cmbForgingShape.Text;
                            pb.Finishing_Size = (txtFinishSize.Text == null) ? "" : txtFinishSize.Text;
                            pb.Proof_Size = (txtProofSize.Text == null) ? "" : txtProofSize.Text;
                            pb.Forging_Size = (txtForgingSize.Text == null) ? "" : txtForgingSize.Text;
                            pb.HT_Condition = cmbHTCondition.Text;
                            pb.Supply_Codition = Convert.ToInt32(cmbSupplyCondition.SelectedValue.ToString());
                            pb.Finish_Wt = (txtFinishWt.Text == "" || txtFinishWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtFinishWt.Text);
                            pb.ProofMachining_Wt = (txtProofWt.Text == "" || txtProofWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtProofWt.Text);
                            pb.Forging_Wt = (txtForgeWt.Text == "" || txtForgeWt.Text == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(txtForgeWt.Text);
                            pb.Drawing_File_Path = txtDrawingLink.Text;
                            pb.Qap_File_Path = txtQAPLink.Text;
                            pb.Company_ID = logIn.company;
                            //pb.Created_By = lnkus1;
                            pb.Modified_By = logIn.username + "-" + DateTime.Now;
                            pb.Created_By = logIn.username + "-" + DateTime.Now;
                            db.Sale_Enquiry_Prod_Specs.InsertOnSubmit(pb);
                            db.SubmitChanges();
                            db.SubmitChanges();
                            MessageBox.Show("Record Saved Sucessfully");
                            //clear();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdBrowseDrawing_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    string file = open.FileName;
                    txtDrawingLink.Text = file;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtDrawingLink.Text;
                //con.Open();
                //string cuerito = "select Drawing_File from Sale_Enquiry_Prod_Specs where [Enq_No]=@EnqNo and [Prod_Code]=@ItemCode";
                //SqlCommand command = new SqlCommand(cuerito, con);
                //command.Parameters.AddWithValue("@EnqNo", txtEnqNo.Text);
                //command.Parameters.AddWithValue("@ItemCode", Convert.ToInt32(txtItemCode.Text));
                //using (SqlDataReader dr = command.ExecuteReader())
                //{
                //    while (dr.Read())
                //    {
                //        int size = 1024 * 1024;
                //        byte[] buffer = new byte[size];
                //        int readBytes = 0;
                //        int index = 0;

                //        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                //        {
                //            while ((readBytes = (int)dr.GetBytes(0, index, buffer, 0, size)) > 0)
                //            {
                //                fs.Write(buffer, 0, readBytes);
                //                index += readBytes;
                //            }
                //        }

                //    }
                //}
                //con.Close();
                Process prc = new Process();
                prc.StartInfo.FileName = fileName;
                prc.Start();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                //daDiagnosis.Dispose();
                //daDiagnosis = null;
            }
        }
        
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].Name;
                decimal ODia = (R1.Cells["Outer_Dia"].Value == "" || R1.Cells["Outer_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Outer_Dia"].Value);
                decimal OThik = (R1.Cells["Thick"].Value == "" || R1.Cells["Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Thick"].Value);
                decimal IDia = (R1.Cells["Inner_Dia"].Value == "" || R1.Cells["Inner_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inner_Dia"].Value);
                decimal IThik = (R1.Cells["Inner_Thick"].Value == "" || R1.Cells["Inner_Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inner_Thick"].Value);
                decimal Density = (txtDensity.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDensity.Text);
                decimal pievalue = Convert.ToDecimal("0.000785714");

                OWt = (ODia * ODia * Density * OThik * pievalue) / 1000;
                IWt = (IDia * IDia * Density * IThik * pievalue) / 1000;

                R1.Cells["OD_Wt"].Value = OWt.ToString();
                R1.Cells["ID_Wt"].Value = IWt.ToString();
                OWt = 0;
                IWt = 0;
                CalculateWeight();
                textBox1.Text = (OWt - IWt).ToString("0.000");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void sfButton4_Click(object sender, EventArgs e)
        {
            try {
                //string OutDia = "";
                //string InnerDia = "";
                OWt = 0;
                IWt = 0;
                CalculateWeight();

                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if(dataGridView1.Rows[i].Cells["Outer_Dia"].Value != DBNull.Value || dataGridView1.Rows[i].Cells["Inner_Dia"].Value != DBNull.Value)
                    {
                        if (dataGridView1.Rows[i].Cells["Step"].Value == null || dataGridView1.Rows[i].Cells["Step"].Value.ToString() =="")
                        {
                            MessageBox.Show("Step No Cannot be Blank");
                            return;
                        }
                    }
                      
                }
                //Save Data in Table
                db.Sp_delete_Enq_Dimensions(logIn.company, Convert.ToInt32(txtItemCode.Text), txtEnqNo.Text, SpecType);
            for (int k = 0; k < dataGridView1.RowCount - 1; k++)
            {
                Sale_Enquiry_Step_Dimension SC = new Sale_Enquiry_Step_Dimension();

                SC.Enq_No = txtEnqNo.Text;
                SC.Prod_Stage = SpecType;

                SC.Prod_Code = Convert.ToInt32(txtItemCode.Text);
                SC.Step_No = Convert.ToInt32(dataGridView1.Rows[k].Cells["Step"].Value.ToString());
                SC.Outer_Dia = (dataGridView1.Rows[k].Cells["Outer_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["Outer_Dia"].Value);
                SC.Outer_Thick = (dataGridView1.Rows[k].Cells["Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["Thick"].Value);
                SC.Outer_Wt = (dataGridView1.Rows[k].Cells["OD_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["OD_Wt"].Value);

                SC.Inner_Dia = (dataGridView1.Rows[k].Cells["Inner_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["Inner_Dia"].Value);
                SC.Inner_Thick = (dataGridView1.Rows[k].Cells["Inner_Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["Inner_Thick"].Value);
                SC.Inner_Wt = (dataGridView1.Rows[k].Cells["ID_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[k].Cells["ID_Wt"].Value);
                SC.CB = (dataGridView1.Rows[k].Cells["CB"].Value == DBNull.Value) ? false : Convert.ToBoolean(dataGridView1.Rows[k].Cells["CB"].Value);


                SC.Company_ID = logIn.company;
                db.Sale_Enquiry_Step_Dimensions.InsertOnSubmit(SC);
            }
            db.SubmitChanges();


            if (SpecType == "Finish")
            {
                txtFinishSize.Text = OutDia + "/" + InnerDia;
                txtFinishWt.Text = (OWt - IWt).ToString("0.00");
            }
            if (SpecType == "Proof")
            {
                txtProofSize.Text = OutDia + "/" + InnerDia;
                txtProofWt.Text = (OWt - IWt).ToString("0.00");
            }
            if (SpecType == "Forge")
            {
                txtForgingSize.Text = OutDia + "/" + InnerDia;
                txtForgeWt.Text = (OWt - IWt).ToString("0.00");
            }
            groupBox3.Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
     }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void txtForgingSize_KeyDown(object sender, KeyEventArgs e)
        {
            
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    SpecType = "Forge";
                    txtDia.Text = "";
                    txtWidth.Text = "";
                    txtLength.Text = "";
                    switch (txtMtrlClass.Text)
                    {
                        case "Stepped - Shaft":
                        case "Spacer":
                            var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                       where s.Enq_No == txtEnqNo.Text && s.Company_ID == logIn.company && s.Prod_Stage == SpecType

                                       select new

                                       {
                                           Step = s.Step_No,
                                           s.Outer_Dia,
                                           Thick = s.Outer_Thick,
                                           OD_Wt = s.Outer_Wt,
                                           s.Inner_Dia,
                                           s.Inner_Thick,
                                           ID_Wt = s.Inner_Wt,
                                           s.CB
                                       });

                            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                            DataTable dtr = new DataTable();
                            da2.Fill(dtr);
                            if (dtr.Rows.Count > 0)
                                dataGridView1.DataSource = dtr;
                            groupBox3.Visible = true;
                            break;
                        case "Blank":
                        case "Round":
                            label7.Text = "Dia";
                            label9.Text = "Length / Thick";
                            txtWidth.Enabled = false;
                            if (txtForgingSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtForgingSize.Text;
                                string[] values = s.Split('x');

                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    string a = m;
                                    string b = string.Empty;
                                    int val = 0;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            b += a[i];
                                    }

                                    if (b.Length > 0)
                                        val = int.Parse(b);
                                    if (txtDia.Text != "")
                                    {
                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtDia.Text = val.ToString();
                                    }

                                }

                            }
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;
                        case "Ring/Tube":
                            label7.Text = "OD";
                            label8.Text = "ID";
                            label9.Text = "Length / Thick";
                            txtWidth.Enabled = true;
                            if (txtForgingSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtForgingSize.Text;
                                // string S1 = CharacterCasing.Upper('X');
                                string[] values = s.Split('X');
                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    string a = m;
                                    string b = string.Empty;
                                    int val = 0;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            b += a[i];
                                    }

                                    if (b.Length > 0)
                                        val = int.Parse(b);
                                    if (txtDia.Text != "")
                                    {
                                        if (txtWidth.Text != "")
                                        {

                                            txtLength.Text = val.ToString();
                                        }
                                        else
                                        {
                                            txtWidth.Text = val.ToString();
                                        }
                                    }
                                    else
                                    {
                                        txtDia.Text = val.ToString();
                                    }

                                }

                            }
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;
                        case "Flat":
                            label7.Text = "Width";
                            label8.Text = "Thick";
                            label9.Text = "Length";
                            txtWidth.Enabled = true;

                            if (txtForgingSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtForgingSize.Text;
                                string[] values = s.Split('x');
                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    if (txtDia.Text != "")
                                    {
                                        txtWidth.Text = m;
                                    }
                                    else if (txtWidth.Text != "")
                                    {
                                        txtLength.Text = m;
                                    }
                                    else
                                    {
                                        txtDia.Text = m;
                                    }
                                }

                            }
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFinishSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    SpecType = "Finish";
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                    switch (txtMtrlClass.Text)
                    {
                        case "Stepped - Shaft":
                        case "Spacer":
                            var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                       where s.Enq_No == txtEnqNo.Text && s.Company_ID == logIn.company && s.Prod_Stage == SpecType

                                       select new

                                       {
                                           Step = s.Step_No,
                                           s.Outer_Dia,
                                           Thick = s.Outer_Thick,
                                           OD_Wt = s.Outer_Wt,
                                           s.Inner_Dia,
                                           s.Inner_Thick,
                                           ID_Wt = s.Inner_Wt,
                                           s.CB

                                       });

                            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                            DataTable dtr = new DataTable();
                            da2.Fill(dtr);
                            if (dtr.Rows.Count > 0)
                                dataGridView1.DataSource = dtr;
                            groupBox3.Visible = true;
                            break;
                        case "Blank":
                        case "Round":
                            label7.Text = "Dia";
                            label9.Text = "Length / Thick";
                            txtWidth.Enabled = false;
                            if (txtFinishSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtFinishSize.Text;
                                string[] values = s.Split('X');
                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    string a = m;
                                    string b = string.Empty;
                                    int val = 0;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            b += a[i];
                                    }

                                    if (b.Length > 0)
                                        val = int.Parse(b);
                                    if (txtDia.Text != "")
                                    {
                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtDia.Text = val.ToString();
                                    }

                                }

                            }
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;
                        case "Ring/Tube":
                            label7.Text = "OD";
                            label8.Text = "ID";
                            label9.Text = "Length / Thick";
                            txtWidth.Enabled = true;
                            if (txtFinishSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtFinishSize.Text;
                                // string S1 = CharacterCasing.Upper('X');
                                string[] values = s.Split('X');
                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    string a = m;
                                    string b = string.Empty;
                                    int val = 0;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            b += a[i];
                                    }

                                    if (b.Length > 0)
                                        val = int.Parse(b);
                                    if (txtDia.Text != "")
                                    {
                                        if (txtWidth.Text != "")
                                        {

                                            txtLength.Text = val.ToString();
                                        }
                                        else
                                        {
                                            txtWidth.Text = val.ToString();
                                        }
                                    }
                                    else
                                    {
                                        txtDia.Text = val.ToString();
                                    }

                                }

                            }
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;
                        case "Flat":
                            label7.Text = "Width";
                            label8.Text = "Thick";
                            label9.Text = "Length";

                            if (txtFinishSize.Text != "")
                            {
                                //Bind HT Condition
                                string s = txtFinishSize.Text;
                                string[] values = s.Split('X');
                                for (int j = 0; j < values.Length; j++)
                                {
                                    values[j] = values[j].Trim();
                                    string m = values[j].ToString();
                                    if (txtDia.Text != "")
                                    {
                                        if (txtWidth.Text != "")
                                        {
                                            txtLength.Text = m;
                                        }
                                        else
                                        {
                                            txtWidth.Text = m;
                                        }
                                    }
                                    else
                                    {
                                        txtDia.Text = m;
                                    }
                                }

                            }

                            txtWidth.Enabled = true;
                            groupBox2.Visible = true;
                            txtDia.Focus();
                            break;
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProofSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    SpecType = "Proof";
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                switch (txtMtrlClass.Text)
                {
                    case "Stepped - Shaft":
                    case "Spacer":
                        var dm1 = (from s in db.Sale_Enquiry_Step_Dimensions
                                where s.Enq_No == txtEnqNo.Text && s.Company_ID == logIn.company && s.Prod_Stage == SpecType

                                select new

                                {
                                    Step = s.Step_No,
                                    s.Outer_Dia,
                                    Thick = s.Outer_Thick,
                                    OD_Wt = s.Outer_Wt,
                                    s.Inner_Dia,
                                    s.Inner_Thick,
                                    ID_Wt = s.Inner_Wt,
                                    s.CB
                                });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count > 0)
                            dataGridView1.DataSource = dtr;
                        groupBox3.Visible = true;
                        break;
                    case "Blank":
                    case "Round":
                        label7.Text = "Dia";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = false;
                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    txtLength.Text = val.ToString();
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Ring/Tube":
                        label7.Text = "OD";
                        label8.Text = "ID";
                        label9.Text = "Length / Thick";
                        txtWidth.Enabled = true;
                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            // string S1 = CharacterCasing.Upper('X');
                            string[] values = s.Split('X');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                string a = m;
                                string b = string.Empty;
                                int val = 0;

                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (Char.IsDigit(a[i]))
                                        b += a[i];
                                }

                                if (b.Length > 0)
                                    val = int.Parse(b);
                                if (txtDia.Text != "")
                                {
                                    if (txtWidth.Text != "")
                                    {

                                        txtLength.Text = val.ToString();
                                    }
                                    else
                                    {
                                        txtWidth.Text = val.ToString();
                                    }
                                }
                                else
                                {
                                    txtDia.Text = val.ToString();
                                }

                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;
                    case "Flat":
                        label7.Text = "Width";
                        label8.Text = "Thick";
                        label9.Text = "Length";
                        txtWidth.Enabled = true;

                        if (txtProofSize.Text != "")
                        {
                            //Bind HT Condition
                            string s = txtProofSize.Text;
                            string[] values = s.Split('x');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();

                                if (txtDia.Text != "")
                                {
                                    txtWidth.Text = m;
                                }
                                else if (txtWidth.Text != "")
                                {
                                    txtLength.Text = m;
                                }
                                else
                                {
                                    txtDia.Text = m;
                                }
                            }

                        }
                        groupBox2.Visible = true;
                        txtDia.Focus();
                        break;


                }
            }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdBrowseQAP_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    string file = open.FileName;
                    txtQAPLink.Text = file;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpenQAP_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = txtQAPLink.Text;
                //con.Open();
                //string cuerito = "select Qap_File from Sale_Enquiry_Prod_Specs where [Enq_No]=@EnqNo and [Prod_Code]=@ItemCode";
                //SqlCommand command = new SqlCommand(cuerito, con);
                //command.Parameters.AddWithValue("@EnqNo", txtEnqNo.Text);
                //command.Parameters.AddWithValue("@ItemCode", Convert.ToInt32(txtItemCode.Text));
                //using (SqlDataReader dr = command.ExecuteReader())
                //{
                //    while (dr.Read())
                //    {
                //        int size = 1024 * 1024;
                //        byte[] buffer = new byte[size];
                //        int readBytes = 0;
                //        int index = 0;

                //        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                //        {
                //            while ((readBytes = (int)dr.GetBytes(0, index, buffer, 0, size)) > 0)
                //            {
                //                fs.Write(buffer, 0, readBytes);
                //                index += readBytes;
                //            }
                //        }

                //    }
                //}
                //con.Close();
                Process prc = new Process();
                prc.StartInfo.FileName = fileName;
                prc.Start();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                //daDiagnosis.Dispose();
                //daDiagnosis = null;
            }
        }

        public void CalculateWeight()
        {
            try
            {
                OutDia = "";
                InnerDia = "";
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    decimal ODia = (dataGridView1.Rows[i].Cells["Outer_Dia"].Value == "" || dataGridView1.Rows[i].Cells["Outer_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Outer_Dia"].Value);
                    decimal OThik = (dataGridView1.Rows[i].Cells["Thick"].Value == "" || dataGridView1.Rows[i].Cells["Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Thick"].Value);
                    decimal IDia = (dataGridView1.Rows[i].Cells["Inner_Dia"].Value == "" || dataGridView1.Rows[i].Cells["Inner_Dia"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Inner_Dia"].Value);
                    decimal IThik = (dataGridView1.Rows[i].Cells["Inner_Thick"].Value == "" || dataGridView1.Rows[i].Cells["Inner_Thick"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Inner_Thick"].Value);
                    decimal Density = (txtDensity.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDensity.Text);
                    decimal pievalue = Convert.ToDecimal("0.000785714");
                    bool cb1 = (dataGridView1.Rows[i].Cells["CB"].Value == DBNull.Value) ? false : Convert.ToBoolean(dataGridView1.Rows[i].Cells["CB"].Value);

                    if (OutDia == "")
                    {
                        OutDia = "OD" + "" + (char)216 + ODia + "X" + OThik;
                    }
                    else
                    {
                        OutDia = OutDia + "/OD" + "" + (char)216 + ODia + "X" + OThik;
                    }

                    if (InnerDia == "")
                    {
                        if (cb1 == true)
                        {
                            InnerDia = "ID" + "" + (char)216 + IDia + "X" + IThik + "-CB";
                        }
                        else
                        {
                            InnerDia = "ID" + "" + (char)216 + IDia + "X" + IThik;
                        }
                    }
                    else
                    {
                        //Boolean cb = Convert.ToBoolean(dataGridView1.Rows[i].Cells["CB"].Value);


                        if (cb1 == true)
                        {
                            InnerDia = InnerDia + "/ID" + "" + (char)216 + IDia + "X" + IThik + "-CB";
                        }

                        else
                        {
                            InnerDia = InnerDia + "/ID" + "" + (char)216 + IDia + "X" + IThik;
                        }
                    }
                    OWt += (ODia * ODia * Density * OThik * pievalue) / 1000;
                    IWt += (IDia * IDia * Density * IThik * pievalue) / 1000;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
