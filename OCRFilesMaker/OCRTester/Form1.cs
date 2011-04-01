using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SimpleOCR;


namespace OCRTester
{
    public partial class Form1 : Form
    {
        private OCRReader _reader;
        private OCRFont _font;

        public Form1()
        {
            InitializeComponent();

            var sizeMode = PictureBoxSizeMode.Normal;
            pictureBox1.SizeMode = sizeMode;
            pictureBox2.SizeMode = sizeMode;
            pictureBox3.SizeMode = sizeMode;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] RGB;

            if (checkBox1.Checked)
            {
                RGB = new[] { 255, 255, 255 };
            }
            else
            {
                RGB = new[] { 0, 0, 0 };
            }

            _font = OCRFont.Load(button2.Text);

            _reader = new OCRReader(_font, Color.FromArgb(RGB[0], RGB[1], RGB[2]),
                                    checkBox1.Checked ? true : false);


            if ((_font == null) || (pictureBox1.Image == null))
            {
                MessageBox.Show("Выберите изображение и шрифт");
                return;
            }

            var d = DateTime.Now;

            var img = (Bitmap) pictureBox1.Image;
            Bitmap s;

            textBox1.Text = _reader.Recognize(ref img, out s);

            pictureBox2.Image = _reader.Crop((Bitmap) pictureBox1.Image);

            pictureBox3.Image = s;
            
            var time = DateTime.Now - d;

            button1.Text = time.TotalMilliseconds.ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save(@"C:\test.png");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "*.png|*.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    pictureBox1.Load(dlg.FileName);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog();
                dlg.Filter = "*.pft|*.pft";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        button2.Text = dlg.FileName;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибочка");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
