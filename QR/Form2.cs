using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace QR
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataReader dr;
        SqlCommand com;
        public Form2()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }
        public string conString = "Data Source=LAPTOP-1GSMFA97\\SQLEXPRESS;Initial Catalog=ticaret;Integrated Security=True;TrustServerCertificate=True";

        private void button1_Click(object sender, EventArgs e)
        {
            string ka = textBox1.Text;
            string sifre = textBox2.Text;

            //Sql bağlantısı oluşturuldu
            con = new SqlConnection("Data Source=LAPTOP-1GSMFA97\\SQLEXPRESS;Initial Catalog=ticaret;Integrated Security=True;TrustServerCertificate=True");
            com = new SqlCommand();
            con.Open();
            com.Connection = con;

            //sadece kullanıcı adı ve şifre kontrolü
            com.CommandText = "Select * From login where username='" + textBox1.Text + "' And password='" + textBox2.Text + "'";

            dr = com.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Kullanıcı girişi başarılı.");
                Form1 gecis = new Form1();
                gecis.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
            }
            con.Close();
        }
    }         
          
}

