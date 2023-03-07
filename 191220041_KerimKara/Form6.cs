using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _191220041_KerimKara
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        public void ResimDegistir(Image resim)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = resim;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Resmi Kaydet";
            saveFileDialog1.ShowDialog();

            var item = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            if (saveFileDialog1.FileName != "") //Dosya adı boş değilse kaydedecek.
            { // FileStream nesnesi ile kayıtı gerçekleştirecek.
                FileStream DosyaAkisi = (FileStream)saveFileDialog1.OpenFile();

                if (item.Equals("JPG"))
                {
                    pictureBox1.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else if (item.Equals("BMP"))
                {
                    pictureBox1.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                else if (item.Equals("PNG)"))
                {
                    pictureBox1.Image.Save(DosyaAkisi, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
    }
}
