using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUMALINOG_DIP
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed;
        Bitmap imageB, imageA, colorgreen;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
   
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Jpeg Image|*.jpeg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";
            saveFileDialog1.ShowDialog();
            
            switch (saveFileDialog1.FilterIndex)
            {
                case 1:
                    pictureBox2.Image.Save(saveFileDialog1.FileName,
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;

                case 2:
                    pictureBox2.Image.Save(saveFileDialog1.FileName,
                        System.Drawing.Imaging.ImageFormat.Bmp);
                    break;

                case 3:
                    pictureBox2.Image.Save(saveFileDialog1.FileName,
                        System.Drawing.Imaging.ImageFormat.Gif);
                    break;

                case 4:
                    pictureBox2.Image.Save(saveFileDialog1.FileName,
                        System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }

            pictureBox2.Image.Save(saveFileDialog1.FileName);

        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color c = loaded.GetPixel(i, j);
                    int avg = (c.R + c.G + c.B) / 3;
                    processed.SetPixel(i, j, Color.FromArgb(avg, avg, avg));
                }
            }

            label.Text = "Greyscale";
            pictureBox2.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color c = loaded.GetPixel(i, j);
                    int r = 255 - c.R;
                    int g = 255 - c.G;
                    int b = 255 - c.B;
                    processed.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }

            label.Text = "Color Inversion";
            pictureBox2.Image = processed;
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            // process image to sepia
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color c = loaded.GetPixel(i, j);
                    int r = (int)(c.R * .393 + c.G * .769 + c.B * .189);
                    int g = (int)(c.R * .349 + c.G * .686 + c.B * .168);
                    int b = (int)(c.R * .272 + c.G * .534 + c.B * .131);
                    if (r > 255) r = 255;
                    if (g > 255) g = 255;
                    if (b > 255) b = 255;
                    processed.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }

            label.Text = "Sepia";
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            //process image to histogram
            int[] hist = new int[256];
            for (int i = 0; i < loaded.Width; i++)
            {
                for (int j = 0; j < loaded.Height; j++)
                {
                    Color c = loaded.GetPixel(i, j);
                    int avg = (c.R + c.G + c.B) / 3;
                    hist[avg]++;
                }
            }

            //draw histogram
            int max = hist.Max();
            int scale = 100;
            int width = 256;
            int height = 100;
            Bitmap histImage = new Bitmap(width, height);
            for (int i = width - 1; i >= 0; i--)
            {
                for (int j = 0; j < height; j++)
                {
                    if (j < (hist[i] * scale / max))
                    {
                        histImage.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        histImage.SetPixel(i, j, Color.White);
                    }
                }
            }

            // rotate histogram
            histImage.RotateFlip(RotateFlipType.Rotate180FlipNone);

            label.Text = "Histogram";
            pictureBox2.Image = histImage;

        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image rotated = pictureBox1.Image;
            rotated.RotateFlip(RotateFlipType.RotateNoneFlipX);
            label.Text = "Horizontal Flip";
            pictureBox2.Image = rotated;
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image rotated = pictureBox1.Image;
            rotated.RotateFlip(RotateFlipType.RotateNoneFlipY);
            label.Text = "Vertical Flip";
            pictureBox2.Image = rotated;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void histogramToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {

        }

        private void bLoadImage_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void bSubstract_Click(object sender, EventArgs e)
        {
            // change greenscreen of imageB to imageA
            Bitmap resultImage = new Bitmap(imageA.Width, imageA.Height);

            Color mygreen = Color.FromArgb(0, 254, 103);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractValue = Math.Abs(grey - greygreen);
                    if (subtractValue < threshold)
                    {
                        resultImage.SetPixel(x, y, backpixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                }
            }

            pictureBox3.Image = resultImage;

            label1.Text = "Substract";
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = imageB;
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = imageA;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            label.Text = "";

            pictureBox1.Update();
            pictureBox2.Update();
            pictureBox3.Update();

        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
            label.Text = "Basic Copy";
        }
    }
}
