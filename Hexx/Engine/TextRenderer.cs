using CSSDL;
using Hexx.Engine.Types;
using Hexx.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine
{
    public class TextRenderer : Singleton<TextRenderer>
    {
        public TextRenderer()
        {
            TTF.InitTTF();
        }

        public Viewport RenderText(Font font, Types.Color color, string text)
        {
            Surface surface = TTF.RenderText(text, font, color.Struct);
            return new Viewport(surface);
        }

        public Viewport RenderText(Font font, Types.Color color, string text, uint maxWidth)
        {
            Surface surface = TTF.RenderText(text, font, color.Struct, maxWidth);
            return new Viewport(surface);
        }

        public Viewport RenderText(string fontName, int fontSize, Types.Color color, string text)
        {
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            Font font = fontLibrary.GetFont(fontName, fontSize);
            return RenderText(font, color, text);
        }

        public Viewport RenderText(string fontName, int fontSize, Types.Color color, string text, uint maxWidth)
        {
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            Font font = fontLibrary.GetFont(fontName, fontSize);
            return RenderText(font, color, text, maxWidth);
        }

        public int GetRenderedTextWidth(string text, Font font)
        {
            return TTF.GetTextWidth(text, font);
        }
    }
}
