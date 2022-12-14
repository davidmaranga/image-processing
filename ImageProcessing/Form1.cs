using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loaded, background, processed;

        public Form1()
        {
            InitializeComponent();

            // To save processed image to PNG file format
            saveFileDialog1.Filter = "png (*.png)|*.png";
            saveFileDialog1.DefaultExt = "png";
            saveFileDialog1.AddExtension = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox3.Image.Save(saveFileDialog1.FileName);
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }

            pictureBox3.Image = processed;
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    processed.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }
            }

            pictureBox3.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B)); ;
                }
            }

            pictureBox3.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Instead of modifying directly the 'processsed' variable,
            // we duplicate the 'loaded' value instead and use this
            // duplicated Bitmap to get the histogram data
            Bitmap duplicate = new Bitmap(loaded);
            Color pixel;
            int grey;
            Color greyscale;

            for (int x = 0; x < duplicate.Width; x++)
            {
                for (int y = 0; y < duplicate.Height; y++)
                {
                    pixel = duplicate.GetPixel(x, y);
                    grey = (pixel.R + pixel.G + pixel.B) / 3;
                    greyscale = Color.FromArgb(grey, grey, grey);
                    duplicate.SetPixel(x, y, greyscale);
                }
            }

            // Histogram in 1D Format
            int[] histogram = new int[256]; // Array from 0 to 255
            for (int x = 0; x < duplicate.Width; x++)
            {
                for (int y = 0; y < duplicate.Height; y++)
                {
                    pixel = duplicate.GetPixel(x, y);
                    histogram[pixel.R]++; // Can be any color property R, G or B
                }
            }

            // Bitmap Graph Generation
            // Setting empty Bitmap with background color
            processed = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    processed.SetPixel(x, y, Color.White);
                }
            }

            // Plotting points based from histogram
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histogram[x] / 5, processed.Height - 1); y++)
                {
                    processed.SetPixel(x, (processed.Height - 1) - y, Color.Black);
                }
            }

            pictureBox3.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            background = new Bitmap(openFileDialog2.FileName);
            pictureBox2.Image = background;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);

            Color green = Color.FromArgb(30, 237, 10);
            int greyGreen = (green.R + green.G + green.B) / 3;
            int threshold = 5;

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    Color backPixel = background.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractValue = Math.Abs(grey - greyGreen);
                    if (subtractValue > threshold)
                    {
                        processed.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        processed.SetPixel(x, y, backPixel);
                    }
                }
            }

            pictureBox3.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int tr = Math.Min((int) (.393 * pixel.R + .769 * pixel.G + .189 * pixel.B), 255);
                    int tg = Math.Min((int) (.349 * pixel.R + .686 * pixel.G + .168 * pixel.B), 255);
                    int tb = Math.Min((int) (.272 * pixel.R + .534 * pixel.G + .131 * pixel.B), 255);

                    processed.SetPixel(x, y, Color.FromArgb(tr, tg, tb));
                }
            }

            pictureBox3.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }
    }
}
