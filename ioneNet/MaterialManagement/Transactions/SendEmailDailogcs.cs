using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.Windows.Forms.Tools.Win32API;
using Ione_DAL;
using System.Configuration;
using System.Data.SqlClient;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Transactions
{
   
    public partial class SendEmailDailogcs : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public SendEmailDailogcs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            var Email = (from em in db.EMailServerSettings
                         where em.company_ID == logIn.company
                         select new { em.SmtpServer, em.SmptPort,em.POMailID,em.POMailPW }).ToList();
            //string email = Email[0].Email_Id + ","+ Email[0].Default_CC_Mail_id;
            string email = "";
            textBox1.Text = Email[0].POMailID;
            textBox2.Text = Email[0].POMailPW;

            //Get User Mail ID
            //var Umail = (from c in db.Supplier_informations

            //             where c.Company_ID == logIn.company && c.ID == Convert.ToInt32(vcode)
            //             select new { c.Email_Id }).ToList();
            //if (Umail.Count > 0)
            //{
            //    if (Umail[0].Email_Id != "")
            //    {
            //        email = Umail[0].Email_Id;
            //    }
            //    else
            //    {
            //        email = "";
            //    }
            //}
            //else
            //{
            email = textBox3.Text;
            //}
            if (email == "")
            {
                MessageBox.Show("E Mail ID of Supplier Is Not Avaiable, Could Not Send the Mail");
                return;
            }
            // string email = Email[0].Cust_Eail;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(textBox1.Text);
            mm.To.Add(email);
            mm.CC.Add(textBox4.Text);
            mm.Subject = "Request for Quotation :" + label6.Text;
            mm.Body = "Dear Sir," + "\n" + "Above referred Enquiry attached here with. Please send us your competetive offer at the earliest";
            // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
            mm.Attachments.Add(new Attachment(RequestForQuotation.filepath));
            string nme;


            //}
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = Email[0].SmtpServer;
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = textBox1.Text;
            NetworkCred.Password = textBox2.Text; ;
            
            smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(Email[0].SmptPort);
            smtp.Send(mm);
            //var ci = db.Purchase_Order_Masters.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company).FirstOrDefault();
            //{
            //    ci.email_Sent = true;
            //    db.SubmitChanges();
            //}
            MessageBox.Show("RFQ Sent by Email Successfully");
        }

        private void SendEmailDailogcs_Load(object sender, EventArgs e)
        {
            label6.Text = GlobalVariables.docRefNo;
            label8.Text = GlobalVariables.doctosend;

            if(label8.Text == "RFQ")
            {
                var dm2 = (from s in db.SP_Get_Suppliers_RFQ(label6.Text, logIn.company, logIn.BU_ID)
                           select new
                           {
                               //S_No = s.ProdSno,
                               Supplier_Code = s.DataItem,
                               Supplier_Name = s.supplier_name                             

                           }).ToList();
                comboBox1.DataSource = dm2;
                comboBox1.ValueMember = "Supplier_Code";
                comboBox1.DisplayMember = "Supplier_Name";
                comboBox1.SelectedIndex = 0;


            }
            var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Name == comboBox1.Text select new { m.Email_Id }).Distinct().ToList();
            if (Buyerblind.Count > 0)
            {
                textBox3.Text = Buyerblind[0].Email_Id;

            }
            else
            {
                MessageBox.Show("Select Valid Supplier Name");
                comboBox1.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Name == comboBox1.Text select new { m.Email_Id }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    textBox3.Text = Buyerblind[0].Email_Id;

                }
                else
                {
                    MessageBox.Show("Select Valid Supplier Name");
                    comboBox1.Text = "";
                }
            }
        }
    }
}
