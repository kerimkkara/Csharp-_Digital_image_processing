using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _191220041_KerimKara
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;

        }

        public void ResimDegistir(Image resim)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = resim;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ResimDegistir((Bitmap)pictureBox1.Image);
            f3.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            if (item.Equals("(a) Renkli resmi > Gri seviye resme dönüştürme"))
            {
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; 
                int ResimYuksekligi = GirisResmi.Height;
                Bitmap CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                int GriDeger = 0;
                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        double R = OkunanRenk.R;
                        double G = OkunanRenk.G;
                        double B = OkunanRenk.B;
                        int GriDegeri = Convert.ToInt16(OkunanRenk.R * 0.21 + OkunanRenk.G * 0.71 + OkunanRenk.B * 0.071); //Gri-ton formülü
                        if (GriDeger > 255)
                            GriDeger = 255;
                        DonusenRenk = Color.FromArgb(GriDeger, GriDeger, GriDeger);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox1.Image = CikisResmi;
            }
            else if (item.Equals("(b) Gri resmi > Siyah Beyaz resme dönüştürme"))
            {

                int R = 0, G = 0, B = 0;
                Color OkunanRenk, DonusenRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);
                int ResimGenisligi = GirisResmi.Width; 
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); 
                int EsiklemeDegeri = Convert.ToInt32(textBox1.Text);
                int EsiklemeDegeri1 = Convert.ToInt32(textBox2.Text);

                for (int x = 0; x < ResimGenisligi; x++)
                {
                    for (int y = 0; y < ResimYuksekligi; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        if (OkunanRenk.R >= EsiklemeDegeri && OkunanRenk.R <= EsiklemeDegeri1)
                            R = 255;
                        else
                            R = 0;
                        if (OkunanRenk.G >= EsiklemeDegeri && OkunanRenk.R <= EsiklemeDegeri1)
                            G = 255;
                        else
                            G = 0;
                        if (OkunanRenk.B >= EsiklemeDegeri && OkunanRenk.R <= EsiklemeDegeri1 )
                            B = 255;
                        else
                            B = 0;
                        DonusenRenk = Color.FromArgb(R, G, B);
                        CikisResmi.SetPixel(x, y, DonusenRenk);
                    }
                }
                pictureBox1.Image = CikisResmi;
            }

            else if (item.Equals("(c) Zoom in – Zoom out"))
            {
                Form7 f7 = new Form7();
                f7.ResimDegistir(pictureBox1.Image);
                f7.ShowDialog();

            }
            else if (item.Equals("(d) Resimden istenilen bölgenin kesilip alınması"))
            {
                             Bitmap Resim1, Resim2, CikisResmi;
                             Resim1 = new Bitmap(pictureBox1.Image);
                             Resim2 = new Bitmap(pictureBox2.Image);
                             int ResimGenisligi = Resim1.Width;
                             int ResimYuksekligi = Resim1.Height;
                             CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
                             Color Renk1, Renk2;
                             int x, y;
                             int R = 0, G = 0, B = 0;
                             for (x = 0; x < ResimGenisligi; x++) 
                             {
                             for (y = 0; y < ResimYuksekligi; y++)
                             {
                             Renk1 = Resim1.GetPixel(x, y);
                             Renk2 = Resim2.GetPixel(x, y);
                             string binarySayi1 = Convert.ToString(Renk1.R, 2).PadLeft(8, '0'); 
                             string binarySayi2 = Convert.ToString(Renk2.R, 2).PadLeft(8, '0');
                             string Bit1 = null, Bit2 = null, StringIkiliSayi = null;
                             for (int i = 0; i < 8; i++)
                             {
                             Bit1 = binarySayi1.Substring(i, 1);
                             Bit2 = binarySayi2.Substring(i, 1);
                           
                            if (Bit1 == "0" && Bit2 == "0") StringIkiliSayi = StringIkiliSayi + "1";
                            else if (Bit1 == "1" && Bit2 == "1") StringIkiliSayi = StringIkiliSayi + "0";
                            else StringIkiliSayi = StringIkiliSayi + "1";
                        }
                        R = Convert.ToInt32(StringIkiliSayi, 2); 
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R, R, R));
                    }
                }
                pictureBox1.Image = CikisResmi;
            }
        }

        

       

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            openFileDialog1.ShowDialog();
            pictureBox2.ImageLocation = openFileDialog1.FileName;
        }
    }



 }
