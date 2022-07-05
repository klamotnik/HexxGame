using CSSDL;
using CSSDL.Events;
using System;
using System.Linq;

namespace Hexx.Engine.Types
{
    public enum FieldType { Text, Password }

    public class InputField : FormControl
    {
        public int MaxValueLength { get; private set; }
        public FieldType Type { get; private set; }
        protected Rectangle fieldArea;
        private string text;
        private bool active;
        private int cursorPosition;
        private Viewport textField;
        private Viewport cursor;
        private bool cursorVisible = true;
        private int timeFromChange;
        private int textOffset;
        
        public InputField(Rectangle rectangle, string name, int maxValueLength, FieldType type = FieldType.Text) : base(name)
        {
            MaxValueLength = maxValueLength;
            text = string.Empty;
            Value = text;
            Type = type;

            Viewport = new Viewport(rectangle);
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawRoundedBorder(Color.Black);
            dp.FillPolygon(20, 20, Color.White);
            NeedRefresh = true;
            cursor = new Viewport((0, 2, 1, 20));
            cursor.Fill(Color.White);
            textField = new Viewport((2, 2, Viewport.Properties.w - 3, Viewport.Properties.h - 3));
            textField.Fill(Color.White);

            fieldArea = AbsolutePosition;

            EventManager.AddKeyDownListener(ReadKey);
            EventManager.AddMouseButtonDownListener(MouseClick);
        }

        public override void Tick(int deltaTime)
        {
            if (!active)
                return;
            timeFromChange += deltaTime;
            if(timeFromChange > 1000)
            {
                timeFromChange = 0;
                cursor.Fill(cursorVisible ? Color.Black: Color.White);
                cursorVisible = !cursorVisible;
                NeedRefresh = true;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            textField.Fill(Color.White);
            textField.Draw(cursor);
            if (!string.IsNullOrEmpty(text))
            {
                Viewport renderedText = GetRenderedText();
                if (textField.Properties.w + textOffset - GetCursorOffset() < 0)
                    textOffset = Math.Abs(textField.Properties.w - GetCursorOffset());
                textField.Draw(renderedText, (textOffset, renderedText.Properties.y, textField.Properties.w, renderedText.Properties.h));
                renderedText.Dispose();
            }
            Viewport.Draw(textField, null, (textField.Properties.x, Viewport.Properties.h - textField.Properties.h - textField.Properties.y, textField.Properties.w, textField.Properties.h));
        }

        private string GetStringToRender()
        {
            string text = "";
            if (Type == FieldType.Password)
                for (int i = 0, j = this.text.Length; i < j; i++)
                    text += '*';
            else
                text = this.text;
            return text;
        }

        private Viewport GetRenderedText()
        {
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            Font font = fontLibrary.GetFont("xolonium.ttf", 20);
            return TextRenderer.GetInstance().RenderText(font, Color.Black, GetStringToRender());
        }

        private int GetCursorOffset()
        {
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            Font font = fontLibrary.GetFont("xolonium.ttf", 20);
            TextRenderer textRenderer = TextRenderer.GetInstance();
            return textRenderer.GetRenderedTextWidth(GetStringToRender().Substring(0, cursorPosition), font);
        }

        private int GetTextLength()
        {
            FontLibrary fontLibrary = FontLibrary.GetInstance();
            Font font = fontLibrary.GetFont("xolonium.ttf", 20);
            TextRenderer textRenderer = TextRenderer.GetInstance();
            return textRenderer.GetRenderedTextWidth(GetStringToRender(), font);
        }

        private void ReadKey(KeyboardEvent e)
        {
            if (!active || !IsVisible || !CanInteract)
                return;
            if ((int)e.Key.KeyCode >= 32 && (int)e.Key.KeyCode <= 126)
            {
                if (text.Length >= MaxValueLength)
                    return;
                string character = ((char)e.Key.KeyCode).ToString();
                if (e.Key.Modifiers == CSSDL.Events.Structures.KeyModifier.LeftShift || e.Key.Modifiers == CSSDL.Events.Structures.KeyModifier.RightShift)
                    character = TextProcessor.GetShiftedCharacter(character);
                if (e.Key.Modifiers == CSSDL.Events.Structures.KeyModifier.Caps)
                    character = character.ToUpper();
                text = text.Substring(0, cursorPosition) + character + text.Substring(cursorPosition);
                ++cursorPosition;
                NeedRefresh = true;
            }
            else if (e.Key.KeyCode == CSSDL.Events.Structures.KeyCode.Left && cursorPosition > 0)
                --cursorPosition;
            else if (e.Key.KeyCode == CSSDL.Events.Structures.KeyCode.Right && cursorPosition < text.Length)
                ++cursorPosition;
            else if (e.Key.KeyCode == CSSDL.Events.Structures.KeyCode.Backspace && cursorPosition > 0)
            {
                text = text.Substring(0, cursorPosition - 1) + text.Substring(cursorPosition);
                --cursorPosition;
                NeedRefresh = true;
            }
            else if ((int)e.Key.KeyCode == 127 && cursorPosition < text.Length)
                text = text.Substring(0, cursorPosition) + text.Substring(cursorPosition + 1);
            UpdateFieldAfterKey();
            Value = text;
        }

        private void UpdateFieldAfterKey()
        {
            int cursorOffset = GetCursorOffset();
            int textLength = GetTextLength();
            if(cursorOffset - textOffset < 0)
                textOffset -= textOffset - cursorOffset;
            else if (textOffset + textField.Properties.w > textLength)
                textOffset = textLength - textField.Properties.w >= 0 ? textLength - textField.Properties.w : 0;
            cursor.MoveTo(cursorOffset > textField.Properties.w + textOffset - 1 ? textField.Properties.w - 1 : cursorOffset - textOffset, cursor.Properties.y);
            cursorVisible = true;
            cursor.Fill(Color.Black);
            timeFromChange = 0;
            NeedRefresh = true;
        }

        private void MouseClick(MouseButtonEvent e)
        {
            if (!IsVisible || !CanInteract)
                return;
            if (AbsolutePosition.IsPointInRectangle(e.X, e.Y))
            {
                active = true;
                cursorVisible = true;
                timeFromChange = 1000;
                cursor.Surface.Fill(Color.Black);
            }
            else if (active)
            {
                cursor.Surface.Fill(Color.White);
                NeedRefresh = true;
                active = false;
            }
        }
    }
}