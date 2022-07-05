using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hexx.Engine.Types;

namespace Hexx.Components.Animations
{
    public class BackgroundAnimation : Animation
    {
        public BackgroundAnimation(Actor parent) : base(parent)
        {

        }

        public override void Tick(int deltaTime)
        {
            throw new NotImplementedException();
        }

        public override bool IsCompleted()
        {
            throw new NotImplementedException();
        }

        public override bool IsPending()
        {
            throw new NotImplementedException();
        }
    }
}
