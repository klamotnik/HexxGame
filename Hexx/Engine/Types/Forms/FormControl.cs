using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexx.Engine.Types
{
    public abstract class FormControl : Actor, IActivatable, IInteractable
    {
        public bool Active { get; set; }
        public bool CanInteract { get; set; }
        public object Value { get; protected set; }
        public string Name { get; private set; }


        public FormControl(string name) : base()
        {
            Name = name;
            CanInteract = true;
        }

        public FormControl(Viewport viewport, string name) : base(viewport)
        {
            Name = name;
            CanInteract = true;
        }
    }
}
