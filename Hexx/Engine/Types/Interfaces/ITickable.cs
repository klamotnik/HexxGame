namespace Hexx.Engine.Types
{
    public interface ITickable
    {
        bool CanTick { get; }
        void Tick(int deltaTime);
    }
}
