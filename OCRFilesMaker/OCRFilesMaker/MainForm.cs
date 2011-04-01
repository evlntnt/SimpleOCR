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
    public partial class MainForm : Form
    {
        const int Cell = 30;
        private OCRFont _font;
        private OCRSymbol _selected;

        public MainForm()
        {
            InitializeComponent();

            var prms = Environment.CommandLine.Split(' ');
            if (prms.Length >= 2)
            {
                var startFont = prms[1];
                if (File.Exists(startFont.Trim('"')))
                {
                    _font = OCRFont.Load(startFont.Trim('"'));
                    ReloadFont();
                }
            }
        }

        void DrawImg(int width, int height, int selectedX, int selectedY)
        {
            if (_selected != null)
            {

                var pic = new Bitmap(_selected.Width*Cell + 1, _selected.Height*Cell + 1);

                var g = Graphics.FromImage(pic);

                g.FillRegion(Brushes.White, new Region(new Rectangle(0, 0, img.Width, img.Height)));

                for (int i = 0; i < width + 1; i++)
                {
                    g.DrawLine(new Pen(Brushes.Black), new Point(Cell*i, 0), new Point(Cell*i, pic.Height));
                }
                for (int i = 0; i < height + 1; i++)
                {
                    g.DrawLine(new Pen(Brushes.Black), new Point(0, Cell*i), new Point(pic.Width, Cell*i));
                }

                if ((selectedX != -1) && (selectedY != -1))
                {
                    g.FillRectangle(Brushes.Gray, new Rectangle(selectedX*Cell, selectedY*Cell, Cell, Cell));
                }

                foreach (var p in _selected.Good)
                {
                    g.FillRectangle(Brushes.Green, new Rectangle(p.X*Cell, p.Y*Cell, Cell, Cell));
                }
                foreach (var p in _selected.Bad)
                {
                    g.FillRectangle(Brushes.Red, new Rectangle(p.X*Cell, p.Y*Cell, Cell, Cell));
                }

                img.Image = pic;
            }

        }

        Point GetSelectedCell(int x, int y)
        {
            return new Point((int)Math.Truncate((double)x/(double)Cell),(int) Math.Truncate((double)y/(double)Cell));
        }

        private void img_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selected != null)
            {
                var p = GetSelectedCell(e.X, e.Y);
                DrawImg(_selected.Width, _selected.Height, p.X, p.Y);
            }
            
        }

        private void img_MouseLeave(object sender, EventArgs e)
        {
            if (_selected != null)
            {
                DrawImg(_selected.Width, _selected.Height, -1, -1);
            }
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void ReloadFont()
        {
            listBox1.Items.Clear();
            foreach (var s in _font.Symbols)
            {
                listBox1.Items.Add(s);
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = "*.pft|*.pft";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _font.Save(dlg.FileName);
            }
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "*.pft|*.pft";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _font = OCRFont.Load(dlg.FileName);
                ReloadFont();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                _selected = (OCRSymbol) listBox1.Items[listBox1.SelectedIndex];

                textBox1.Text = _selected.Width.ToString();
                textBox2.Text = _selected.Height.ToString();
                textBox3.Text = _selected.TopOffset.ToString();

                DrawImg(_selected.Width, _selected.Height, -1, -1);
            }
        }

        private void img_MouseDown(object sender, MouseEventArgs e)
        {
            var cell = GetSelectedCell(e.X, e.Y);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // Заливаем зеленым или разливаем если уже залито

                if (_selected.Good.Contains(cell))
                {
                    _selected.Good.Remove(cell);
                }
                else
                {
                    if (_selected.Bad.Contains(cell))
                        _selected.Bad.Remove(cell);

                    _selected.Good.Add(cell);
                }
                

            }
            else
            {
                // Заливаем красным или разливаем если уже залито

                if (_selected.Bad.Contains(cell))
                {
                    _selected.Bad.Remove(cell);
                }
                else
                {
                    if (_selected.Good.Contains(cell))
                        _selected.Good.Remove(cell);

                    _selected.Bad.Add(cell);
                }
                
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_selected != null)
            {
                try
                {
                    _selected.Width = Int32.Parse(textBox1.Text);
                    DrawImg(_selected.Width, _selected.Height, -1, -1);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка ввода");
                    textBox1.Text = _selected.Width.ToString();
                }
                
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _selected.Width = Int32.Parse(textBox1.Text);
                DrawImg(_selected.Width, _selected.Height, -1, -1);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода");
                textBox1.Text = _selected.Width.ToString();
            }
        }

        private void импортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var w = new ImportForm();
            w.ShowDialog();
            if (ImportForm.ClosedByOk)
            {
                _font = ImportForm.OCRFont;
                ReloadFont();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _selected.TopOffset = Int32.Parse(textBox3.Text);
                DrawImg(_selected.Width, _selected.Height, -1, -1);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода");
                textBox1.Text = _selected.TopOffset.ToString();
            }
        }


    }
}
