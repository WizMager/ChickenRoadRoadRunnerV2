using System;

namespace Car
{
    public interface ICarController
    {
        bool IsSaveJump { get; }
        Action OnChickenHit { get; set; }
        
        void Initialize();
        void Reset();
    }
}