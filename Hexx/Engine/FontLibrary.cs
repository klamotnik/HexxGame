using CSSDL;
using Hexx.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine
{
    public class FontLibrary : Singleton<FontLibrary>
    {
        private List<Font> fonts = new List<Font>();

        ~FontLibrary()
        {
            foreach (Font font in fonts)
                font.Dispose();
        }

        public Font GetFont(string name, int size)
        {
            Font font = fonts.Where(p => p.Name == name && p.Size == size).FirstOrDefault();
            if (font == null)
            {
                font = new Font("fonts/" + name, size);
                fonts.Add(font);
            }
            return font;
        }
    }
}
