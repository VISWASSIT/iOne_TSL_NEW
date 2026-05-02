using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.Masters
{
    public partial class imageSave : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public imageSave()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Browse Image
                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    pictureBox1.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Save Image in Database
            if (pictureBox1.Image != null)
            {
                Image img = pictureBox1.Image;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Insert into image_Saving(image_Logo) values(@img)";
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@img", bytes);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Image saved to database");
            }
            
               


            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Get Image from Database
            SqlCommand cmd = new SqlCommand("Select image_Logo from image_Saving", con);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    byte[] img = (byte[])reader["image_Logo"];

                    MemoryStream ms = new MemoryStream(img);

                    pictureBox1.Image = Image.FromStream(ms);
                }
            }

            con.Close();
        }
    }
}
