using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleOCR
{
    [Serializable]
    public class OCRFont
    {
        public List<OCRSymbol> Symbols { get; set; }
        public int MaxFontWidth { get; private set; }
        public int MaxFontHeight { get; private set; }

        public OCRFont(int width = 15, int height = 15)
        {
            MaxFontHeight = height;
            MaxFontWidth = width;
            Symbols = new List<OCRSymbol>();
        }

        public void Save(string fn)
        {
            var bs = new BinaryFormatter();
            var fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
            bs.Serialize(fs, this);
            fs.Close();
        }

        public static OCRFont Load(string fn)
        {
            var bs = new BinaryFormatter();
            if (!File.Exists(fn))
                throw new FileNotFoundException(fn);
            var fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
            var font = (OCRFont) bs.Deserialize(fs);
            fs.Close();
            return font;
        }

    }
}
