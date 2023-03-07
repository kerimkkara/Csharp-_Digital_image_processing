using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Collections; // array kullanımı için
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace _191220041_KerimKara
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.ResimDegistir((Bitmap)pictureBox1.Image);
            f4.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }

        public void ResimDegistir(Bitmap resim)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            if (item.Equals("(a) Histogram oluşturma"))
            {


                pictureBox2.Image = null;

                ArrayList DiziPiksel = new ArrayList();

                int OrtalamaRenk = 0;
                Color OkunanRenk;

                Bitmap GirisResmi; //Histogram için giriş resmi gri-ton olmalıdır.
                GirisResmi = new Bitmap(pictureBox1.Image);

                Bitmap GriResim = ResmiGriTonaDonustur(GirisResmi); //Giriş resmi global olduğu için burada götürmüyor.
                pictureBox1.Image = GriResim;

                int ResimGenisligi = GriResim.Width; //GirisResmi global tanımlandı.
                int ResimYuksekligi = GriResim.Height;

                for (int x = 0; x < GirisResmi.Width; x++)
                {
                    for (int y = 0; y < GirisResmi.Height; y++)
                    {
                        OkunanRenk = GirisResmi.GetPixel(x, y);
                        //OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3; //Griton resimde üç kanal rengi aynı değere sahiptir.

                        DiziPiksel.Add(OkunanRenk.R); //Gri resim olduğu için tek kanalı okuması yeterli olacaktır. 
                    }

                }

                int[] DiziPikselSayilari = new int[256];
                for (int r = 0; r < 255; r++) //256 tane renk tonu için dönecek.
                {
                    int PikselSayisi = 0;
                    for (int s = 0; s < DiziPiksel.Count; s++) //resimdeki piksel sayısınca dönecek. 
                    {
                        if (r == Convert.ToInt16(DiziPiksel[s]))
                            PikselSayisi++;
                    }
                    DiziPikselSayilari[r] = PikselSayisi;
                }


                int RenkMaksPikselSayisi = 0; //Grafikte y eksenini ölçeklerken kullanılacak. 
                for (int k = 0; k <= 255; k++)
                {
                    if (DiziPikselSayilari[k] > RenkMaksPikselSayisi)
                    {
                        RenkMaksPikselSayisi = DiziPikselSayilari[k];
                    }
                }

                //Grafiği çiziyor. 
                Graphics CizimAlani;
                Pen Kalem1 = new Pen(System.Drawing.Color.Blue, 1);
                Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
                CizimAlani = pictureBox2.CreateGraphics();

                pictureBox2.Refresh();
                int GrafikYuksekligi = 450;
                double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi, OlcekX = 1.6;
                for (int x = 0; x <= 255; x++)
                {
                    CizimAlani.DrawLine(Kalem1, (int)(20 + x * OlcekX), GrafikYuksekligi, (int)(20 + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));
                    if (x % 50 == 0)
                        CizimAlani.DrawLine(Kalem2, (int)(20 + x * OlcekX), GrafikYuksekligi, (int)(20 + x * OlcekX), 0);

                }
               


            }
            else if (item.Equals("(b) Histogram Eşitleme"))
            {

                

                var orjinalGoruntu = new Bitmap(pictureBox1.Image);



                pictureBox1.Image = histogramEşitleme(orjinalGoruntu);


            }
            else if (item.Equals("(c) Görüntü Nicemleme"))
            {

            }
        }


        public Bitmap ResmiGriTonaDonustur(Bitmap GirisResmi)
        {
            int R = 0, G = 0, B = 0;
            Color OkunanRenk, DonusenRenk;
            //Bitmap GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width; //GirisResmi global tanımlandı. Fonksiyonla gelmedi.
            int ResimYuksekligi = GirisResmi.Height;

            //CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi, System.Drawing.Imaging.PixelFormat.Format8bppIndexed); //8 bir formatında gri-ton resim oluşturmak için.
            Bitmap CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi); //Cikis resmini oluşturuyor. Boyutları giriş resmi ile aynı olur.

            int i = 0, j = 0; //Çıkış resminin x ve y si olacak.
            for (int x = 0; x < ResimGenisligi; x++)
            {
                j = 0;
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);

                    //int GriDegeri = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B)/3; //Ortalama Gri-ton formülü
                    //int GriDegeri = Convert.ToInt16(OkunanRenk.R * 0.299 + OkunanRenk.G * 0.587 + OkunanRenk.B * 0.114); //Gri-ton formülü
                    int GriDegeri = Convert.ToInt16(OkunanRenk.R * 0.21 + OkunanRenk.G * 0.71 + OkunanRenk.B * 0.071); //Gri-ton formülü

                    R = GriDegeri;
                    G = GriDegeri;
                    B = GriDegeri;
                    DonusenRenk = Color.FromArgb(R, G, B);

                    CikisResmi.SetPixel(i, j, DonusenRenk);
                    j++;
                }
                i++;
            }

            //CikisResmi = Deneme8bit(CikisResmi);

            return CikisResmi;
        }

        public Bitmap histogramEşitleme(Bitmap KaynakResim)
        {
            Bitmap renderedImage = KaynakResim;

            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y, R, G, B;


            int[] HistogramRed2 = new int[256];
            int[] HistogramGreen2 = new int[256];
            int[] HistogramBlue2 = new int[256];


            for (var i = 0; i < renderedImage.Width; i++)
            {
                for (var j = 0; j < renderedImage.Height; j++)
                {
                    var piksel = renderedImage.GetPixel(i, j);

                    HistogramRed2[(int)piksel.R]++;
                    HistogramGreen2[(int)piksel.G]++;
                    HistogramBlue2[(int)piksel.B]++;

                }
            }

            int[] cdfR = HistogramRed2;
            int[] cdfG = HistogramGreen2;
            int[] cdfB = HistogramBlue2;

            for (int r = 1; r <= 255; r++)
            {
                cdfR[r] = cdfR[r] + cdfR[r - 1];
                cdfG[r] = cdfG[r] + cdfG[r - 1];
                cdfB[r] = cdfB[r] + cdfB[r - 1];
            }

            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);

                    R = (int)((decimal)cdfR[pixelColor.R] * Const);
                    G = (int)((decimal)cdfG[pixelColor.G] * Const);
                    B = (int)((decimal)cdfB[pixelColor.B] * Const);

                    Color newColor = Color.FromArgb(R, G, B);
                    renderedImage.SetPixel(x, y, newColor);
                }
            }
            return renderedImage;
        }

    }

        }
    
