using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MesajSistemi
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string numara;
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-KACV7HQ\\SQLEXPRESS;Initial Catalog=MesajlasmaSistemi;Integrated Security=True");
        void gelenkutusu()
        {
            SqlDataAdapter da = new SqlDataAdapter("select MESAJID,(AD+' '+SOYAD) as GONDEREN,BASLIK,ICERIK from TBLMESAJLAR inner join TBLKISILER on TBLMESAJLAR.GONDEREN = TBLKISILER.NUMARA where ALICI=" + numara,baglanti);
            DataTable dt = new DataTable(); 
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        void gidenkutusu()
        {
            SqlDataAdapter da = new SqlDataAdapter("select MESAJID,(AD+' '+SOYAD) as ALICI,BASLIK,ICERIK from TBLMESAJLAR inner join TBLKISILER on TBLMESAJLAR.ALICI = TBLKISILER.NUMARA where GONDEREN=" + numara, baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            LblNumara.Text = numara;
            gelenkutusu();
            gidenkutusu();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select Ad,Soyad from TBLKISILER where numara=" + numara,baglanti);
            SqlDataReader dr = komut.ExecuteReader();   
            while (dr.Read()) 
            {
                LblAdSoyad.Text = dr[0] + " " + dr[1];
            }
            baglanti.Close();

            baglanti.Open();
            SqlCommand komut1 = new SqlCommand("select Ad,Soyad from TBLKISILER where numara!="+numara,baglanti);
            SqlDataReader dr1 = komut1.ExecuteReader();
            while(dr1.Read())
            {
                comboBox1.Items.Add(dr1[0] + " " + dr1[1]);
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLMESAJLAR (gonderen,alıcı,baslık,ıcerık) values (@p1,@p2,@p3,@p4)",baglanti);
            komut.Parameters.AddWithValue("@p1", numara);
            komut.Parameters.AddWithValue("@p2", comboBox1.Text);
            komut.Parameters.AddWithValue("@p3", textBox1.Text);
            komut.Parameters.AddWithValue("@p4", richTextBox1.Text);
            komut.ExecuteNonQuery();
            MessageBox.Show("Mesajınız İletildi");
            gidenkutusu();
        }
    }
}
