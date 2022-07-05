using System.Collections.Generic;
using System.Linq;

namespace Hexx.Engine.Types
{
    public abstract class Animation : ITickable
    {
        public bool CanTick { get; set; }
        public bool Pause { get; set; }
        protected List<Actor> actorsToManipulation;

        protected Animation(Actor parent)
        {
            actorsToManipulation = new List<Actor>();
            actorsToManipulation.Add(parent);
        }

        protected Animation(IEnumerable<Actor> parents)
        {
            actorsToManipulation = parents.ToList();
        }

        public abstract void Tick(int deltaTime);

        public abstract bool IsPending();

        public abstract bool IsCompleted();

        public void Start()
        {
            CanTick = true;
        }

        public void Stop()
        {
            CanTick = false;
        }
    }
}
