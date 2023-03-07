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

namespace _191220041_KerimKara
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.ResimDegistir(pictureBox1.Image);
            f6.ShowDialog();
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

        private void Form5_Load(object sender, EventArgs e)
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

            if (item.Equals("(a) Siyah beyaz resimde genişletme"))
            {
                Bitmap image = (Bitmap)pictureBox1.Image;
                int se_dim = 3;
                int w = image.Width;
                int h = image.Height;

                BitmapData image_data = image.LockBits(
                    new Rectangle(0, 0, w, h),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

                int bytes = image_data.Stride * image_data.Height;
                byte[] buffer = new byte[bytes];
                byte[] result = new byte[bytes];

                Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
                image.UnlockBits(image_data);

                int o = (se_dim - 1) / 2;
                for (int i = o; i < w - o; i++)
                {
                    for (int j = o; j < h - o; j++)
                    {
                        int position = i * 3 + j * image_data.Stride;
                        for (int k = -o; k <= o; k++)
                        {
                            for (int l = -o; l <= o; l++)
                            {
                                int se_pos = position + k * 3 + l * image_data.Stride;
                                for (int c = 0; c < 3; c++)
                                {
                                    result[se_pos + c] = Math.Max(result[se_pos + c], buffer[position]);
                                }
                            }
                        }
                    }
                }

                Bitmap res_img = new Bitmap(w, h);
                BitmapData res_data = res_img.LockBits(
                    new Rectangle(0, 0, w, h),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                Marshal.Copy(result, 0, res_data.Scan0, bytes);
                res_img.UnlockBits(res_data);
                pictureBox1.Image = res_img;
            }
            else if (item.Equals("(b) Siyah beyaz resimde erozyon"))
            {
                
                Bitmap image = (Bitmap)pictureBox1.Image;
                int se_dim = 3;
                int w = image.Width;
                int h = image.Height;

                BitmapData image_data = image.LockBits(
                    new Rectangle(0, 0, w, h),
                    ImageLockMode.ReadOnly,
                    PixelFormat.Format24bppRgb);

                int bytes = image_data.Stride * image_data.Height;
                byte[] buffer = new byte[bytes];
                byte[] result = new byte[bytes];

                Marshal.Copy(image_data.Scan0, buffer, 0, bytes);
                image.UnlockBits(image_data);

                int o = (se_dim - 1) / 2;
                for (int i = o; i < w - o; i++)
                {
                    for (int j = o; j < h - o; j++)
                    {
                        int position = i * 3 + j * image_data.Stride;
                        byte val = 255;
                        for (int x = -o; x <= o; x++)
                        {
                            for (int y = -o; y <= o; y++)
                            {
                                int kposition = position + x * 3 + y * image_data.Stride;
                                val = Math.Min(val, buffer[kposition]);
                            }
                        }

                        for (int c = 0; c < 3; c++)
                        {
                            result[position + c] = val;
                        }
                    }
                }

                Bitmap res_img = new Bitmap(w, h);
                BitmapData res_data = res_img.LockBits(
                    new Rectangle(0, 0, w, h),
                    ImageLockMode.WriteOnly,
                    PixelFormat.Format24bppRgb);
                Marshal.Copy(result, 0, res_data.Scan0, bytes);
                res_img.UnlockBits(res_data);

                pictureBox1.Image =  res_img;
            }
            else if (item.Equals("(c) İskelet çıkartma (Skeletonization)"))
            {
                
            }
                
         
        }

    }
}
