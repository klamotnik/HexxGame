using Hexx.Engine.Types;

namespace Hexx.Engine.Types
{
    public interface IRenderable
    {
        Viewport Viewport { get; }
        void Refresh();
        bool NeedRefresh { get; }
    }
}
