using CSSDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public abstract class Actor : IRenderable, ITickable
    {
        public bool CanTick { get; set; }
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                isVisible = value;
                foreach (Actor child in childrens)
                    child.IsVisible = value;
                NeedRefresh = true;
            }
        }
        public bool NeedRefresh
        {
            get => needRefresh || childrens.Any(p => p.NeedRefresh);
             set => needRefresh = value;
        }
        public Viewport Viewport { get; protected set; }
        public Rectangle AbsolutePosition { get; private set; }
        public Rectangle Position
        {
            get => Viewport.Properties;
        }
        protected List<Actor> childrens = new List<Actor>();
        private bool needRefresh;
        private bool isVisible;

        public Actor()
        {
            IsVisible = true;
            NeedRefresh = true;
            CanTick = true;
        }

        public Actor(Rectangle rectangle) : this()
        {
            Viewport = new Viewport(rectangle);
            AbsolutePosition = (rectangle.x, rectangle.y, rectangle.w, rectangle.h);
        }

        public Actor(Viewport viewport) : this()
        {
            Viewport = viewport;
        }

        public virtual bool AddChild(Actor child)
        {
            childrens.Add(child);
            if (Viewport != null)
                child.AbsolutePosition = (child.Viewport.Properties.x + AbsolutePosition.x, child.Viewport.Properties.y + AbsolutePosition.y, child.Viewport.Properties.w, child.Viewport.Properties.h);
            return true;
        }

        public virtual bool AddChild(Actor[] childs)
        {
            childrens.AddRange(childs);
            if (Viewport != null)
                foreach(Actor child in childs)
                    child.AbsolutePosition = (child.Viewport.Properties.x + AbsolutePosition.x, child.Viewport.Properties.y + AbsolutePosition.y, child.Viewport.Properties.w, child.Viewport.Properties.h);
            return true;
        }

        public virtual void Tick(int deltaTime)
        {
            foreach (Actor child in childrens.Where(p=>p.CanTick))
                child.Tick(deltaTime);
        }
        
        public virtual void Refresh()
        {
            NeedRefresh = false;
            if (!IsVisible)
                Viewport.Clear();
        }

        public void Move(int offsetX, int offsetY)
        {
            Viewport.MoveTo(Viewport.Properties.x + offsetX, Viewport.Properties.y + offsetY);
            AbsolutePosition = (AbsolutePosition.x + offsetX, AbsolutePosition.y + offsetY, AbsolutePosition.w, AbsolutePosition.h);
            foreach(Actor child in childrens)
                child.AbsolutePosition = (child.AbsolutePosition.x + offsetX, child.AbsolutePosition.y + offsetY, child.AbsolutePosition.w, child.AbsolutePosition.h);
        }

        public void MoveTo(int newX, int newY)
        {
            AbsolutePosition = (AbsolutePosition.x + newX - Viewport.Properties.x, 
                                AbsolutePosition.y + newY - Viewport.Properties.y,
                                Viewport.Properties.w,
                                Viewport.Properties.h);
            foreach (Actor child in childrens)
            {
                child.AbsolutePosition = (child.AbsolutePosition.x + newX - child.Viewport.Properties.x, child.AbsolutePosition.y + newY - child.Viewport.Properties.y, child.AbsolutePosition.w, child.AbsolutePosition.h);
                child.PropagateAbsolutePositionToChild(newX, newY);
            }
            Viewport.MoveTo(newX, newY);
        }

        private void PropagateAbsolutePositionToChild(int parentAbsoluteX, int parentAbsoluteY)
        {
            AbsolutePosition = (parentAbsoluteX + Viewport.Properties.x,
                                parentAbsoluteY + Viewport.Properties.y,
                                AbsolutePosition.w,
                                AbsolutePosition.h);
            foreach (Actor child in childrens)
            {
                child.PropagateAbsolutePositionToChild(AbsolutePosition.x, AbsolutePosition.y);
            }
        }
    }
}
