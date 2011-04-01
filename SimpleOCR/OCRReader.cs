using System.Drawing;
using System.Linq;
using AForge.Imaging.Filters;

namespace SimpleOCR
{
    public class OCRReader
    {
        public OCRFont Font { get; private set; }
        public Color BgOrForeColor { get; private set; }
        public bool UseForeGround { get; private set; }

        public int PlayerStackDollarWidth { get; private set; }
        public int PlayerStackDotWidth { get; private set; }

        public OCRReader(OCRFont font, Color bgOrFontColor, bool useForeColor)
        {
            BgOrForeColor = bgOrFontColor;
            UseForeGround = useForeColor;
            Font = font;

            if (Font.Symbols.Select(c=>c.Name).Contains("$"))
            {
                PlayerStackDollarWidth = Font.Symbols.Where(c => c.Name == "$").Single().Width;
                PlayerStackDotWidth = Font.Symbols.Where(c => c.Name == ".").Single().Width;
            }
        }

        public Bitmap Crop(Bitmap img)
        {
            if (UseForeGround)
            {

                var topCrop = 0;
                for (int i = 0; i < img.Height; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Width; j++)
                    {
                        var pix = img.GetPixel(j, i);
                        if ((pix.R == BgOrForeColor.R) && (pix.G == BgOrForeColor.G) && (pix.B == BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        topCrop = i;
                        break;
                    }
                }

                var bottomCrop = 0;
                for (int i = 0; i < img.Height; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Width; j++)
                    {
                        var pix = img.GetPixel(j, img.Height - i - 1);
                        if ((pix.R == BgOrForeColor.R) && (pix.G == BgOrForeColor.G) && (pix.B == BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        bottomCrop = i;
                        break;
                    }
                }

                var leftCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(i, j);
                        if ((pix.R == BgOrForeColor.R) && (pix.G == BgOrForeColor.G) && (pix.B == BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        leftCrop = i;
                        break;
                    }
                }

                var rightCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(img.Width - i - 1, j);
                        if ((pix.R == BgOrForeColor.R) && (pix.G == BgOrForeColor.G) && (pix.B == BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        rightCrop = i;
                        break;
                    }
                }

                var cr =
                    new Crop(new Rectangle(leftCrop, topCrop, img.Width - leftCrop - rightCrop,
                                           img.Height - topCrop - bottomCrop));

                return cr.Apply(img);

            }
            else
            {
                var topCrop = 0;
                for (int i = 0; i < img.Height; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Width; j++)
                    {
                        var pix = img.GetPixel(j, i);
                        if ((pix.R != BgOrForeColor.R) || (pix.G != BgOrForeColor.G) || (pix.B != BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        topCrop = i;
                        break;
                    }
                }

                var bottomCrop = 0;
                for (int i = 0; i < img.Height; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Width; j++)
                    {
                        var pix = img.GetPixel(j, img.Height - i - 1);
                        if ((pix.R != BgOrForeColor.R) || (pix.G != BgOrForeColor.G) || (pix.B != BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        bottomCrop = i;
                        break;
                    }
                }

                var leftCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(i, j);
                        if ((pix.R != BgOrForeColor.R) || (pix.G != BgOrForeColor.G) || (pix.B != BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        leftCrop = i;
                        break;
                    }
                }

                var rightCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(img.Width - i - 1, j);
                        if ((pix.R != BgOrForeColor.R) || (pix.G != BgOrForeColor.G) || (pix.B != BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        rightCrop = i;
                        break;
                    }
                }

                var cr =
                    new Crop(new Rectangle(leftCrop, topCrop, img.Width - leftCrop - rightCrop,
                                           img.Height - topCrop - bottomCrop));

                return cr.Apply(img);
            }
        }

        Bitmap CropLeft(Bitmap img)
        {
            if (UseForeGround)
            {
                var leftCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(i, j);
                        if ((pix.R == BgOrForeColor.R) && (pix.G == BgOrForeColor.G) && (pix.B == BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        leftCrop = i;
                        break;
                    }
                }

                var cr =
                    new Crop(new Rectangle(leftCrop, 0, img.Width - leftCrop,
                                           img.Height));

                return cr.Apply(img);
            }
            else
            {
                var leftCrop = 0;
                for (int i = 0; i < img.Width; i++)
                {
                    var crop = true;
                    for (int j = 0; j < img.Height; j++)
                    {
                        var pix = img.GetPixel(i, j);
                        if ((pix.R != BgOrForeColor.R) || (pix.G != BgOrForeColor.G) || (pix.B != BgOrForeColor.B))
                        {
                            crop = false;
                            break;
                        }
                    }
                    if (crop == false)
                    {
                        leftCrop = i;
                        break;
                    }
                }

                var cr =
                    new Crop(new Rectangle(leftCrop, 0, img.Width - leftCrop,
                                           img.Height));

                return cr.Apply(img);
            }


        }

        private OCRSymbol RecognizeSymbol(ref Bitmap img)
        {
            foreach (var s in Font.Symbols)
            {
                var cr = new Crop(new Rectangle(0, s.TopOffset, s.Width, s.Height + s.TopOffset));
                var cropedImg = cr.Apply(img);

                if (s.Compare(ref cropedImg, BgOrForeColor, UseForeGround))
                {
                    return s;
                }
            }
            return null;
        }

        public string Recognize(ref Bitmap img, out Bitmap last)
        {
            var cr = new Crop(new Rectangle(0,0,img.Width,img.Height));
            var curImg = cr.Apply(Crop(img));

            var stack="";

            while (true)
            {
                var symb = RecognizeSymbol(ref curImg);
                if (symb != null)
                {
                    stack += symb;
                    cr = new Crop(new Rectangle(symb.Width, 0, curImg.Width - symb.Width, curImg.Height));
                    
                    if (symb.Width == curImg.Width)
                    {
                        last = curImg;
                        return stack;
                    }

                    curImg = CropLeft(cr.Apply(curImg));
                    last = curImg;
                }
                else
                {
                    last = curImg;
                    return stack;
                }
            }
        }

        public string Recognize(ref Bitmap img)
        {
            var curImg = Crop(img);
            var stack = "";
            Crop cr;

            while (true)
            {
                var symb = RecognizeSymbol(ref curImg);
                if (symb != null)
                {
                    stack += symb;
                    cr = new Crop(new Rectangle(symb.Width, 0, curImg.Width - symb.Width, curImg.Height));
                    if (symb.Width == curImg.Width)
                    {
                        return stack;
                    }
                    curImg = CropLeft(cr.Apply(curImg));
                }
                else
                {
                    return stack;
                }
            }
        }

    }
}
