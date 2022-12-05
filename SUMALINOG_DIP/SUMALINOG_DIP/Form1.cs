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
            saveFileDialog1.Filter = "Jpeg Image|*.jpeg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";

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
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            
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
            for (int i = 0; i < width; i++)
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

            pictureBox2.Image = histImage;

        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = pictureBox1.Image;
        }
    }
}
