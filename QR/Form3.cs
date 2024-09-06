using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QR
{
    public partial class Form3 : Form
    {
        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;
        public Form3()
        {
            InitializeComponent();
        }

        //method oluşturuldu
        void KullaniciGetir()
        {
            baglanti = new SqlConnection("server=.;initial Catalog=ticaret;Integrated Security=SSPI");
            baglanti.Open();
            da = new SqlDataAdapter("SELECT *FROM fis", baglanti) ;
            DataTable tablo = new DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            KullaniciGetir();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells [5].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "INSERT INTO fis(name,date,total,total tax,sale receipt no) VALUES(@name,@date,@total,@total tax,@sale receipt no)";
            komut = new SqlCommand(sorgu,baglanti) ;
            komut.Parameters.AddWithValue("@name",textBox2.Text) ;
            komut.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            komut.Parameters.AddWithValue("@total", textBox4.Text);
            komut.Parameters.AddWithValue("@total tax", textBox5.Text);
            komut.Parameters.AddWithValue("@sale receipt no", textBox6.Text);
            baglanti.Open();
            komut.ExecuteXmlReader();
            baglanti.Close();
            KullaniciGetir();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sorgu = "DELETE FROM fis WHERE fs=@fs";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@fs",Convert.ToInt32(textBox1.Text));
            baglanti.Open() ; 
            komut.ExecuteNonQuery();
            baglanti.Close();
            KullaniciGetir();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sorgu = "UPDATE fis SET name=@name,date=@date,total=@total,total tax=@total tax,sale receipt no=@sale receipt no WHERE fis=@fis";
            komut = new SqlCommand (sorgu, baglanti);
            komut.Parameters.AddWithValue("@fis",Convert.ToInt32(textBox1.Text));
            komut.Parameters.AddWithValue("@name", textBox2.Text);
            komut.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            komut.Parameters.AddWithValue("@total",textBox4.Text );
            komut.Parameters.AddWithValue("@total tax", textBox5.Text);
            komut.Parameters.AddWithValue("@sale receipt no", textBox6.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            KullaniciGetir();
        }
    }
}
