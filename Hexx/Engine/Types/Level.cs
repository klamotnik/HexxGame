using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public abstract class Level : IRenderable, ITickable, IDisposable
    {
        public bool CanTick { get; set; }
        public Viewport Viewport { get; }
        public bool NeedRefresh { get; protected set; }
        public List<Actor> actorList = new List<Actor>();
        private Queue<MessageBox> messageBoxQueue = new Queue<MessageBox>();
        private MessageBox activeMessageBox;
        
        protected Level(Viewport viewport)
        {
            Viewport = viewport;
            CanTick = true;
            NeedRefresh = true;
            MessageBoxService.Register(this, PushMessageBoxIntoQueue);
        }

        public abstract void Dispose();
        
        public virtual void Refresh()
        {
            NeedRefresh = false;
            Viewport.Clear();
            foreach (Actor actor in actorList.Where(p=>p.IsVisible))
            {
                if(actor.NeedRefresh)
                    actor.Refresh();
                Viewport.Draw(actor);
                NeedRefresh = true;
            }
            if (activeMessageBox == null && messageBoxQueue.Count > 0)
            {
                activeMessageBox = messageBoxQueue.Dequeue();
                activeMessageBox.Refresh();
                NeedRefresh = true;
            }
            if (activeMessageBox != null)
            {
                if (activeMessageBox.IsVisible)
                    Viewport.Draw(activeMessageBox);
                else
                    activeMessageBox = null;
            }
        }

        public virtual void Tick(int deltaTime)
        {
            foreach (Actor actor in actorList.Where(p=>p.CanTick))
                actor.Tick(deltaTime);
        }

        public void AddActor(Actor actor)
        {
            if (!actorList.Contains(actor))
                actorList.Add(actor);
            actor.MoveTo(actor.Viewport.Properties.x, actor.Viewport.Properties.y);
        }

        public void RemoveActor(Actor actor)
        {
            if (actorList.Contains(actor))
                actorList.Remove(actor);
        }

        private void PushMessageBoxIntoQueue(MessageBox messageBox)
        {
            messageBoxQueue.Enqueue(messageBox);
        }
    }
}
