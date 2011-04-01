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

namespace OCRFilesMaker
{
    public partial class ImportForm : Form
    {
        public static bool ClosedByOk { get; private set; }
        public static OCRFont OCRFont { get; private set; }
        private int[] RGB;

        private DirectoryInfo _dir;

        public ImportForm()
        {
            InitializeComponent();
            ClosedByOk = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _dir = new DirectoryInfo(dlg.SelectedPath);
                var files = _dir.GetFiles("*.png", SearchOption.AllDirectories);
                label1.Text = dlg.SelectedPath + " (" + files.Count() + " .png файлов)";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                RGB = new[] {int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text)};

                var files = _dir.GetFiles("*.png", SearchOption.AllDirectories);

                var symbols = new List<OCRSymbol>();
                foreach (var fileInfo in files)
                {
                    var img = (Bitmap) Image.FromFile(fileInfo.FullName);
                    var s = new OCRSymbol(fileInfo.Name.Substring(0, fileInfo.Name.LastIndexOf(".")), img.Width,
                                          img.Height);

                    var good = new List<Point>();
                    var bad = new List<Point>();

                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            var pixel = img.GetPixel(i, j);

                            if ((pixel.R == RGB[0])&& (pixel.G == RGB[1])&&(pixel.B==RGB[2]))
                            {
                                bad.Add(new Point(i,j));
                            }
                            else
                            {
                                good.Add(new Point(i, j));
                            }

                        }
                    }
                    s.Good = good;
                    s.Bad = bad;
                    symbols.Add(s);
                }

                var maxWidth = symbols.Max(c => c.Width);
                var maxHeight = symbols.Max(c => c.Height);

                symbols.ForEach(c => c.TopOffset = maxHeight - c.Height);

                OCRFont = new OCRFont(maxWidth, maxHeight);
                
                OCRFont.Symbols = symbols;

                ClosedByOk = true;
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода");
                throw;
            }


        }
    }
}
