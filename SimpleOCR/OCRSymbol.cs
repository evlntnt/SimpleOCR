using System;
using System.Collections.Generic;
using System.Drawing;

namespace SimpleOCR
{
    [Serializable]
    public class OCRSymbol
    {
        public string Name { get; private set; }
        public int Width { get; set; }
        public int Height { get; private set; }
        public List<Point> Good { get; set; }
        public List<Point> Bad { get; set; }
        public int TopOffset { get; set; }

        public OCRSymbol(string name, int width = 12, int height = 12, int offset = 0, List<Point> good = null, List<Point> bad = null)
        {
            Name = name;
            Width = width;
            Height = height;
            TopOffset = offset;

            if (good == null)
                Good = new List<Point>();
            else
            {
                Good = good;
            }

            if (bad == null)
                Bad = new List<Point>();
            else
            {
                Bad = bad;
            }
        }

        public bool Compare(ref Bitmap img, Color color, bool isUseForeground)
        {
            if (isUseForeground)
            {
                foreach (var point in Bad)
                {
                    if ((img.Width > point.X) && (img.Height > point.Y))
                    {
                        var pix = img.GetPixel(point.X, point.Y);
                        if ((pix.R == color.R) && (pix.G == color.G) && (pix.B == color.B))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (var point in Good)
                {
                    if ((img.Width > point.X) && (img.Height > point.Y))
                    {
                        var pix = img.GetPixel(point.X, point.Y);
                        if ((pix.R != color.R) || (pix.G != color.G) || (pix.B != color.B))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

                return true;
            }
            else
            {
                foreach (var point in Bad)
                {
                    if ((img.Width > point.X) && (img.Height > point.Y))
                    {
                        var pix = img.GetPixel(point.X, point.Y);
                        if ((pix.R != color.R) || (pix.G != color.G) || (pix.B != color.B))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                foreach (var point in Good)
                {
                    if ((img.Width > point.X) && (img.Height > point.Y))
                    {
                        var pix = img.GetPixel(point.X, point.Y);
                        if ((pix.R == color.R) && (pix.G == color.G) && (pix.B == color.B))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }

                return true;
            }

           
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
