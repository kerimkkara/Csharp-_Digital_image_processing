using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _191220041_KerimKara
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.ResimDegistir((Bitmap)pictureBox1.Image);
            f5.ShowDialog();
        }

        public void ResimDegistir(Bitmap resim)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = resim;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
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

            if (item.Equals("(a) Gaussian Bulanıklaştırma filtresi"))
            {
                pictureBox1.Image = GaussFiltresi();
            }
            else if (item.Equals("(b) Keskinleştirme filtresi"))
            {

                Bitmap OrjinalResim = new Bitmap(pictureBox1.Image);

                Bitmap BulanikResim = MeanFiltresi();
                Bitmap KenarGoruntusu = OrjianalResimdenBulanikResmiCikarma(OrjinalResim,BulanikResim);
                Bitmap NetlesmisResim = KenarGoruntusuIleOrjinalResmiBirlestir(OrjinalResim,KenarGoruntusu);
                pictureBox1.Image = NetlesmisResim;



            }
            else if (item.Equals("(c) Kenar bulma filtresi"))
            {
                kenarBulma();  
               
            }
            else if (item.Equals("(d) Ortalama (Mean) filtresi"))
            {
               meanFilter();
            }
            else if (item.Equals("(e) Ortanca (Median) filtresi"))
            {
                Color OkunanRenk;
                Bitmap GirisResmi, CikisResmi;
                GirisResmi = new Bitmap(pictureBox1.Image);

                int ResimGenisligi = GirisResmi.Width;
                int ResimYuksekligi = GirisResmi.Height;
                CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

                int SablonBoyutu = 8;
                int ElemanSayisi = SablonBoyutu * SablonBoyutu;

                int[] R = new int[ElemanSayisi];
                int[] G = new int[ElemanSayisi];
                int[] B = new int[ElemanSayisi];
                int[] Gri = new int[ElemanSayisi];

                int x, y, i, j;

                for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
                {
                    for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                    {
                      
                        int k = 0;
                        for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                        {
                            for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                            {
                                OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                                R[k] = OkunanRenk.R;
                                G[k] = OkunanRenk.G;
                                B[k] = OkunanRenk.B;

                                Gri[k] = Convert.ToInt16(R[k] * 0.299 + G[k] * 0.587 + B[k] * 0.114); 

                                k++;
                            }
                        }

                      
                        int GeciciSayi = 0;

                        for (i = 0; i < ElemanSayisi; i++)
                        {
                            for (j = i + 1; j < ElemanSayisi; j++)
                            {
                                if (Gri[j] < Gri[i])
                                {
                                    GeciciSayi = Gri[i];
                                    Gri[i] = Gri[j];
                                    Gri[j] = GeciciSayi;

                                    GeciciSayi = R[i];
                                    R[i] = R[j];
                                    R[j] = GeciciSayi;

                                    GeciciSayi = G[i];
                                    G[i] = G[j];
                                    G[j] = GeciciSayi;

                                    GeciciSayi = B[i];
                                    B[i] = B[j];
                                    B[j] = GeciciSayi;
                                }
                            }
                        }

                      
                        CikisResmi.SetPixel(x, y, Color.FromArgb(R[(ElemanSayisi - 1) / 2], G[(ElemanSayisi - 1) / 2], B[(ElemanSayisi - 1) / 2]));
                    }
                }

                pictureBox1.Image = CikisResmi;
            }
            else if (item.Equals("(f) Kontra Harmonik Ortalama filtresi"))
            {
               

                var image = new Bitmap(pictureBox1.Image);
                double order = -1;
                int w = image.Width;
                int h = image.Height;
                BitmapData image_data = image.LockBits(
                    new Rectangle(0, 0, w, h),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);
                int bytes = image_data.Stride * image_data.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
                image.UnlockBits(image_data);

                int r = 1;
                int wres = w - 2 * r;
                int hres = h - 2 * r;
                Bitmap result_image = new Bitmap(wres, hres);
                BitmapData result_data = result_image.LockBits(
                    new Rectangle(0, 0, wres, hres),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                int res_bytes = result_data.Stride * result_data.Height;
                byte[] result = new byte[res_bytes];

                for (int x = r; x < w - r; x++)
                {
                    for (int y = r; y < h - r; y++)
                    {
                        int pixel_location = x * 3 + y * image_data.Stride;
                        int res_pixel_loc = (x - r) * 3 + (y - r) * result_data.Stride;
                        double[] sum1 = new double[3];
                        double[] sum2 = new double[3];

                        for (int kx = -r; kx <= r; kx++)
                        {
                            for (int ky = -r; ky <= r; ky++)
                            {
                                int kernel_pixel = pixel_location + kx * 3 + ky * image_data.Stride;

                                for (int c = 0; c < 3; c++)
                                {
                                    sum1[c] += Math.Pow(buffer[kernel_pixel + c], order + 1);
                                    sum2[c] += Math.Pow(buffer[kernel_pixel + c], order);
                                }
                            }
                        }

                        for (int c = 0; c < 3; c++)
                        {
                            result[res_pixel_loc + c] = (byte)(sum1[c] / sum2[c]);
                        }
                    }
                }

                Marshal.Copy(result, 0, result_data.Scan0, res_bytes);
                result_image.UnlockBits(result_data);
                pictureBox1.Image = result_image;
                pictureBox1.Refresh();

            }

        }


        public void kenarBulma()
        {
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

            int SablonBoyutu = 3;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;


            int x, y; 


            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) 
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {

                    Color Renk;
                    int P1, P2, P3, P4, P5, P6, P7, P8, P9;

                    Renk = GirisResmi.GetPixel(x - 1, y - 1);
                    P1 = Renk.R;

                    Renk = GirisResmi.GetPixel(x, y - 1);
                    P2 = Renk.R;

                    Renk = GirisResmi.GetPixel(x + 1, y - 1);
                    P3 = Renk.R;

                    Renk = GirisResmi.GetPixel(x - 1, y);
                    P4 = Renk.R;

                    Renk = GirisResmi.GetPixel(x, y);
                    P5 = Renk.R;

                    Renk = GirisResmi.GetPixel(x + 1, y);
                    P6 = Renk.R;

                    Renk = GirisResmi.GetPixel(x - 1, y + 1);
                    P7 = Renk.R;

                    Renk = GirisResmi.GetPixel(x, y + 1);
                    P8 = Renk.R;

                    Renk = GirisResmi.GetPixel(x + 1, y + 1);
                    P9 = Renk.R;

                    int RenkDegeri = Math.Abs((P1 + 2 * P2 + P3) - (P7 + 2 * P8 + P9)) + Math.Abs((P3 + 2 * P6 + P9) - (P1 + 2 * P4 + P7));

                   
                    if (RenkDegeri > 255) RenkDegeri = 255;
                    CikisResmi.SetPixel(x, y, Color.FromArgb(RenkDegeri, RenkDegeri, RenkDegeri));
                }
            }
            pictureBox1.Image = CikisResmi;
        }
        
        public void meanFilter()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);


            int SablonBoyutu = 9; 
            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;

            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;

                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                            toplamR = toplamR + OkunanRenk.R;
                            toplamG = toplamG + OkunanRenk.G;
                            toplamB = toplamB + OkunanRenk.B;

                        }
                    }

                    ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                    ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                    ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);

                    CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                }
            }

            pictureBox1.Image = CikisResmi;
        }

        public Bitmap GaussFiltresi()
        {
            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);

            int SablonBoyutu = 5;
            int ElemanSayisi = SablonBoyutu * SablonBoyutu;


            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;
            //int R, G, B, Gri;
            int[] Matris = { 1, 4, 7, 4, 1, 4, 20, 33, 20, 4, 7, 33, 55, 33, 7, 4, 20, 33, 20, 4, 1, 4, 7, 4, 1 };
            int MatrisToplami = 1 + 4 + 7 + 4 + 1 + 4 + 20 + 33 + 20 + 4 + 7 + 33 + 55 + 33 + 7 + 4 + 20 + 33 + 20 + 4 + 1 + 4 + 7 + 4 + 1;

            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++) 
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;

                    
                    int k = 0; 
                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                            toplamR = toplamR + OkunanRenk.R * Matris[k];
                            toplamG = toplamG + OkunanRenk.G * Matris[k];
                            toplamB = toplamB + OkunanRenk.B * Matris[k];

                            ortalamaR = toplamR / MatrisToplami;
                            ortalamaG = toplamG / MatrisToplami;
                            ortalamaB = toplamB / MatrisToplami;

                            CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));

                            k++;
                        }

                    }

                }
            }
            return CikisResmi;


        }





        public Bitmap KenarGoruntusuIleOrjinalResmiBirlestir(Bitmap OrjinalResim, Bitmap KenarGoruntusu)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = KenarGoruntusu.GetPixel(x, y);
                    R = OkunanRenk1.R + OkunanRenk2.R;
                    G = OkunanRenk1.G + OkunanRenk2.G;
                    B = OkunanRenk1.B + OkunanRenk2.B;
                    //===============================================================
                    //Renkler sınırların dışına çıktıysa, sınır değer alınacak. 
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;
                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;
                    //================================================================
                    DonusenRenk = Color.FromArgb(R, G, B);
                    CikisResmi.SetPixel(x, y, DonusenRenk);
                }
            }
            return CikisResmi;
        }


        public Bitmap OrjianalResimdenBulanikResmiCikarma(Bitmap OrjinalResim, Bitmap BulanikResim)
        {
            Color OkunanRenk1, OkunanRenk2, DonusenRenk;
            Bitmap CikisResmi;
            int ResimGenisligi = OrjinalResim.Width;
            int ResimYuksekligi = OrjinalResim.Height;
            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);
            int R, G, B;
            double Olcekleme = 2; 
            for (int x = 0; x < ResimGenisligi; x++)
            {
                for (int y = 0; y < ResimYuksekligi; y++)
                {
                    OkunanRenk1 = OrjinalResim.GetPixel(x, y);
                    OkunanRenk2 = BulanikResim.GetPixel(x, y);
                    R = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.R - OkunanRenk2.R));
                    G = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.G - OkunanRenk2.G));
                    B = Convert.ToInt16(Olcekleme * Math.Abs(OkunanRenk1.B - OkunanRenk2.B));
                           
                    if (R > 255) R = 255;
            if (G > 255) G = 255;
            if (B > 255) B = 255;
            if (R < 0) R = 0;
            if (G < 0) G = 0;
            if (B < 0) B = 0;
            //================================================================
            DonusenRenk = Color.FromArgb(R, G, B);
            CikisResmi.SetPixel(x, y, DonusenRenk);
        }
    }
 return CikisResmi;
 }
        public Bitmap MeanFiltresi()
        {

            Color OkunanRenk;
            Bitmap GirisResmi, CikisResmi;
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;

            CikisResmi = new Bitmap(ResimGenisligi, ResimYuksekligi);


            int SablonBoyutu = 9; 
            int x, y, i, j, toplamR, toplamG, toplamB, ortalamaR, ortalamaG, ortalamaB;

            for (x = (SablonBoyutu - 1) / 2; x < ResimGenisligi - (SablonBoyutu - 1) / 2; x++)
            {
                for (y = (SablonBoyutu - 1) / 2; y < ResimYuksekligi - (SablonBoyutu - 1) / 2; y++)
                {
                    toplamR = 0;
                    toplamG = 0;
                    toplamB = 0;

                    for (i = -((SablonBoyutu - 1) / 2); i <= (SablonBoyutu - 1) / 2; i++)
                    {
                        for (j = -((SablonBoyutu - 1) / 2); j <= (SablonBoyutu - 1) / 2; j++)
                        {
                            OkunanRenk = GirisResmi.GetPixel(x + i, y + j);

                            toplamR = toplamR + OkunanRenk.R;
                            toplamG = toplamG + OkunanRenk.G;
                            toplamB = toplamB + OkunanRenk.B;

                        }
                    }

                    ortalamaR = toplamR / (SablonBoyutu * SablonBoyutu);
                    ortalamaG = toplamG / (SablonBoyutu * SablonBoyutu);
                    ortalamaB = toplamB / (SablonBoyutu * SablonBoyutu);

                    CikisResmi.SetPixel(x, y, Color.FromArgb(ortalamaR, ortalamaG, ortalamaB));
                }
            }

            return CikisResmi;
        }


    }
}
