using System;

namespace Move
{
    public interface IChickenMove
    {
        Action OnFinalMoveEnd { get; set; }
        
        void Initialize();
    }
}