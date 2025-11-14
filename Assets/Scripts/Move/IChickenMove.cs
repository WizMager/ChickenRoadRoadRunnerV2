namespace Move
{
    public interface IChickenMove
    {
        void Initialize();
        void GoToLastCheckpoint();
        void RevertJump();
        void Reset();
    }
}