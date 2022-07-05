using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public class Panel : Actor, IInteractable
    {
        public bool CanInteract
        {
            get => IsVisible && childrens.OfType<IInteractable>().Any(p => p.CanInteract);
            set
            {
                foreach (IInteractable interactable in childrens.OfType<IInteractable>())
                    interactable.CanInteract = value;
            }
        }

        public Panel(Rectangle rectangle) : base(rectangle)
        {
            Viewport = new Viewport(rectangle);
        }

        public IEnumerable<Actor> GetActors()
        {
            return childrens.ToArray();
        }

        public override void Refresh()
        {
            base.Refresh();
            if (!IsVisible)
                return;
            if(childrens.Any(p=>p.NeedRefresh))
                Viewport.Clear();
            foreach (Actor child in childrens)
            {
                if (child.NeedRefresh)
                    child.Refresh();
                Viewport.Draw(child);
            }
        }
    }
}
