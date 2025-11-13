namespace Move
{
    public interface IChickenMove
    {
        void GoToLastCheckpoint();
        void RevertJump();
        void Reset();
    }
}