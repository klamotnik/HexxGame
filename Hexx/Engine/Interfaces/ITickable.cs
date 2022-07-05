namespace Hexx.Engine.Interfaces
{
    public interface ITickable
    {
        bool CanTick { get; }
        void Tick(int deltaTime);
    }
}
