using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class List : Actor
    {
        protected List<ListRow> rows;
        private Button buttonDown;
        private Button buttonUp;
        private Viewport rowsViewport;

        public List(Rectangle rectangle) : base(rectangle)
        {
            NeedRefresh = true;
        }

        public bool AddRow(ListRow row)
        {
            return false;
        }

        public bool RemoveRow(ListRow row)
        {
            return false;
        }

        public override void Refresh()
        {
            base.Refresh();
            Viewport.Fill(Color.White);
            DrawingProcessor dp = new DrawingProcessor(Viewport.Surface);
            dp.DrawRectangle((0, 0, Viewport.Properties.w - 1, Viewport.Properties.h - 1), 1, CSSDL.Color.Black);
            Rectangle rowsViewportRectangle = (1, 1, Viewport.Properties.w - 2, childrens.OfType<ListRow>().Sum(p => p.Viewport.Properties.h));
            if (rowsViewport?.Surface?.handle != IntPtr.Zero)
                rowsViewport?.Dispose();
            rowsViewport = new Viewport(rowsViewportRectangle);
            int y = 0;
            foreach (Actor child in childrens)
            {
                child.MoveTo(0, y);
                if(child.NeedRefresh)
                    child.Refresh();
                y += child.Viewport.Properties.h;
                rowsViewport.Draw(child);
            }
            Viewport.Draw(rowsViewport);
        }

        public bool RemoveChild(string key)
        {
            ListRow row = childrens.OfType<ListRow>().Where(p => p.Key == key).FirstOrDefault();
            if (row != null)
            {
                childrens.Remove(row);
                NeedRefresh = true;
                return true;
            }
            return false;
        }
    }
}
