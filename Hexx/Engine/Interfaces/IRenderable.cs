using Hexx.Engine.Types;

namespace Hexx.Engine.Interfaces
{
    public interface IRenderable
    {
        Viewport Viewport { get; }
        void Refresh();
        bool NeedRefresh();
    }
}
