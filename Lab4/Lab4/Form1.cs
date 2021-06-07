using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar1.TickFrequency = 20;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вариант 5, Препарирование изображения(неполная пороговая обработка), применение произвольных масочных фильтров");
        }

        private void LoadIm_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (openFileDialog1.FileName == null) return;
                pictureBox1.Load(openFileDialog1.FileName.ToString());

            }
        }

        private void ImGray_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            Bitmap img = new Bitmap(pictureBox1.Image,pictureBox1.Width,pictureBox1.Height);
            Color col = new Color();
            
            for(int i=0; i<img.Width; i++)
                for(int j=0; j<img.Height;j++)
                {
                    col = img.GetPixel(i, j);
                    double gray = 0.3 * col.R + 0.59 * col.G + 0.11 * col.B;
                    img.SetPixel(i, j, Color.FromArgb(Convert.ToInt32(gray), Convert.ToInt32(gray), Convert.ToInt32(gray)));
                }
            pictureBox2.Image = img;
            col = img.GetPixel(200,200);
        }

        private void PrepIm_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                Bitmap bmp = new Bitmap(pictureBox2.Image, pictureBox2.Width, pictureBox2.Height);
                int brg;
                int choose = Int32.Parse(label3.Text.Trim());

                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        brg = (int)(bmp.GetPixel(i, j).R + bmp.GetPixel(i, j).G + bmp.GetPixel(i, j).B);
                        if (brg >= choose)
                        {
                            brg = 255;
                        }
                        bmp.SetPixel(i, j, Color.FromArgb(brg, brg, brg));
                    }
                pictureBox3.Image = bmp;
            }
            else MessageBox.Show("Загрузите картинку");
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
            PrepIm_Click(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
                (pictureBox3.Image as Bitmap).Save(textBox1.Text + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            else MessageBox.Show("Введите имя файла");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
        }

        private void FilterIm_Click(object sender, EventArgs e)
        {
            Filter(pictureBox4);
        }
        public void Filter(PictureBox pb)
        {
            int[,] matrix = new int[,] 
            {{0, -1, 0},
            {-1, 5, -1},
            {0, -1, 0}};
            Bitmap bmp = new Bitmap(pictureBox2.Image);
            Bitmap bmp1 = new Bitmap(bmp.Width, bmp.Height);
            int r, g, b;
            for (int i = 2; i < bmp.Width - 2; i++)
            {
                for (int j = 2; j < bmp.Height - 2; j++)
                {
                    r = 0; g = 0; b = 0;
                    for (int n = i - 2; n < i + 1; n++) 
                    {
                        for (int k = j - 2; k < j + 1; k++)
                        {

                            r += matrix[n + 2 - i, k + 2 - j] * bmp.GetPixel(n, k).R;
                            g += matrix[n + 2 - i, k + 2 - j] * bmp.GetPixel(n, k).G;
                            b += matrix[n + 2 - i, k + 2 - j] * bmp.GetPixel(n, k).B;
                        }
                    }
                    bmp1.SetPixel(i, j, Color.FromArgb(Math.Max((Math.Min(r,255)),0), Math.Max((Math.Min(g, 255)), 0), Math.Max((Math.Min(b, 255)), 0)));
                }
            }

            pb.Image = bmp1;
        }
    }
}
